using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using APKExtractor.Models;

namespace APKExtractor
{
    /// <summary>
    /// ADB工具辅助类，封装所有ADB命令调用
    /// </summary>
    public class AdbHelper
    {
        private string _adbPath;
        private string _aaptPath;
        private const string DefaultAdbName = "adb.exe";

        /// <summary>日志输出回调</summary>
        public event Action<string> OnLogMessage;

        /// <summary>导出进度回调 (current, total)</summary>
        public event Action<int, int> OnExportProgress;

        /// <summary>获取显示名称进度回调 (current, total, packageName)</summary>
        public event Action<int, int, string> OnLabelProgress;

        public AdbHelper()
        {
            _adbPath = FindAdbPath();
            _aaptPath = FindAaptPath();
        }

        /// <summary>
        /// 使用配置初始化
        /// </summary>
        public AdbHelper(AppConfig config) : this()
        {
            if (config != null)
            {
                // 配置中的路径优先于自动查找
                if (!string.IsNullOrWhiteSpace(config.AdbPath))
                    _adbPath = config.AdbPath.Trim();
                if (!string.IsNullOrWhiteSpace(config.AaptPath))
                    _aaptPath = config.AaptPath.Trim();
            }
        }

        /// <summary>
        /// 手动设置ADB路径
        /// </summary>
        public void SetAdbPath(string path)
        {
            if (!string.IsNullOrWhiteSpace(path))
                _adbPath = path.Trim();
        }

        /// <summary>
        /// 手动设置aapt路径
        /// </summary>
        public void SetAaptPath(string path)
        {
            if (!string.IsNullOrWhiteSpace(path))
                _aaptPath = path.Trim();
            else
                _aaptPath = FindAaptPath();
        }

        /// <summary>
        /// 获取ADB路径
        /// </summary>
        public string AdbPath => _adbPath;

        /// <summary>
        /// 获取aapt路径
        /// </summary>
        public string AaptPath => _aaptPath;

        /// <summary>
        /// aapt工具是否可用
        /// </summary>
        public bool HasAapt => !string.IsNullOrEmpty(_aaptPath) && File.Exists(_aaptPath);

        /// <summary>
        /// 查找ADB可执行文件路径
        /// </summary>
        private string FindAdbPath()
        {
            // 1. 程序目录下的adb.exe
            string localAdb = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DefaultAdbName);
            if (File.Exists(localAdb))
                return localAdb;

            // 2. ANDROID_HOME / ANDROID_SDK_ROOT 环境变量
            string androidHome = Environment.GetEnvironmentVariable("ANDROID_HOME")
                              ?? Environment.GetEnvironmentVariable("ANDROID_SDK_ROOT");
            if (!string.IsNullOrEmpty(androidHome))
            {
                string sdkAdb = Path.Combine(androidHome, "platform-tools", DefaultAdbName);
                if (File.Exists(sdkAdb))
                    return sdkAdb;
            }

            // 3. PATH中的adb
            return DefaultAdbName;
        }

        /// <summary>
        /// 查找aapt可执行文件路径（用于获取应用显示名称）
        /// </summary>
        private string FindAaptPath()
        {
            // 1. 程序目录下的aapt.exe
            string localAapt = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "aapt.exe");
            if (File.Exists(localAapt))
                return localAapt;

            // 2. ANDROID_HOME/build-tools/<version>/aapt.exe
            string androidHome = Environment.GetEnvironmentVariable("ANDROID_HOME")
                              ?? Environment.GetEnvironmentVariable("ANDROID_SDK_ROOT");
            if (!string.IsNullOrEmpty(androidHome))
            {
                string buildToolsDir = Path.Combine(androidHome, "build-tools");
                if (Directory.Exists(buildToolsDir))
                {
                    // 查找最新版本的aapt
                    var versionDirs = Directory.GetDirectories(buildToolsDir)
                        .OrderByDescending(d => Path.GetFileName(d));
                    foreach (string dir in versionDirs)
                    {
                        string aapt = Path.Combine(dir, "aapt.exe");
                        if (File.Exists(aapt))
                            return aapt;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 检查ADB是否可用
        /// </summary>
        public async Task<bool> CheckAdbAvailableAsync()
        {
            try
            {
                string result = await RunAdbCommandAsync("version");
                return result != null && result.Contains("Android Debug Bridge");
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取已连接的设备列表
        /// </summary>
        public async Task<List<AndroidDevice>> GetDevicesAsync()
        {
            var devices = new List<AndroidDevice>();

            string output = await RunAdbCommandAsync("devices -l");
            if (string.IsNullOrWhiteSpace(output))
                return devices;

            string[] lines = output.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in lines)
            {
                string trimmed = line.Trim();
                if (trimmed.StartsWith("List of devices") || string.IsNullOrEmpty(trimmed))
                    continue;

                // 格式: serial    state    model:xxx device:xxx ...
                string[] parts = trimmed.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length < 2)
                    continue;

                var device = new AndroidDevice
                {
                    SerialNumber = parts[0],
                    State = parts[1]
                };

                // 从额外字段解析型号
                foreach (string part in parts.Skip(2))
                {
                    if (part.StartsWith("model:"))
                        device.Model = part.Substring(6);
                    else if (part.StartsWith("device:"))
                    {
                        if (string.IsNullOrEmpty(device.Model))
                            device.Model = part.Substring(7);
                    }
                }

                // 仅对正常状态的设备补充获取详细信息
                if (device.State == "device")
                {
                    try
                    {
                        if (string.IsNullOrEmpty(device.Model))
                            device.Model = SafeTrim(await RunAdbCommandAsync($"-s {device.SerialNumber} shell getprop ro.product.model"));

                        device.AndroidVersion = SafeTrim(await RunAdbCommandAsync($"-s {device.SerialNumber} shell getprop ro.build.version.release"));
                        device.Manufacturer = SafeTrim(await RunAdbCommandAsync($"-s {device.SerialNumber} shell getprop ro.product.manufacturer"));
                    }
                    catch
                    {
                        // 忽略单个属性获取失败
                    }
                }

                if (string.IsNullOrEmpty(device.Model))
                    device.Model = "Unknown";
                if (string.IsNullOrEmpty(device.Manufacturer))
                    device.Manufacturer = "";
                if (string.IsNullOrEmpty(device.AndroidVersion))
                    device.AndroidVersion = "";

                devices.Add(device);
            }

            return devices;
        }

        /// <summary>
        /// 获取设备上的已安装第三方应用列表（过滤系统应用）
        /// </summary>
        public async Task<List<AppInfo>> GetInstalledAppsAsync(string serialNumber)
        {
            var apps = new List<AppInfo>();

            // -3 表示第三方应用，-f 显示APK路径
            string packageOutput = await RunAdbCommandAsync($"-s {serialNumber} shell pm list packages -3 -f");
            if (string.IsNullOrWhiteSpace(packageOutput))
                return apps;

            string[] lines = packageOutput.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line) || !line.StartsWith("package:"))
                    continue;

                // 格式: package:/data/app/.../base.apk=com.example.app
                // 注意：APK路径中可能包含=号（如Vivo设备的加密路径），
                // 包名不含=号，所以必须用最后一个=来分割
                var app = new AppInfo { IsSystemApp = false };

                string content = line.Substring("package:".Length);
                int lastEq = content.LastIndexOf('=');
                if (lastEq < 0)
                    continue;

                app.ApkPath = content.Substring(0, lastEq).Trim();
                app.PackageName = content.Substring(lastEq + 1).Trim();

                // 尝试获取应用名称和版本信息
                try
                {
                    string infoOutput = await RunAdbCommandAsync(
                        $"-s {serialNumber} shell dumpsys package {app.PackageName}", 10000);

                    if (!string.IsNullOrWhiteSpace(infoOutput))
                    {
                        // 尝试解析应用显示名称
                        // 注意：nonLocalizedLabel=null 表示标签定义在资源文件中，需要通过aapt获取
                        Match nameMatch = Regex.Match(infoOutput, @"nonLocalizedLabel=(?!null)([^\r\n]+)");
                        if (nameMatch.Success)
                        {
                            string label = nameMatch.Groups[1].Value.Trim().Trim('"');
                            if (!string.IsNullOrEmpty(label) && label != "null")
                                app.AppName = label;
                        }

                        Match verMatch = Regex.Match(infoOutput, @"versionName=(\S+)");
                        if (verMatch.Success)
                            app.VersionName = verMatch.Groups[1].Value.Trim();

                        Match codeMatch = Regex.Match(infoOutput, @"versionCode=(\d+)");
                        if (codeMatch.Success)
                            app.VersionCode = codeMatch.Groups[1].Value;
                    }
                }
                catch
                {
                    // 忽略单个应用信息获取失败
                }

                if (string.IsNullOrEmpty(app.AppName))
                    app.AppName = app.PackageName;

                apps.Add(app);
            }

            return apps.OrderBy(a => a.AppName).ToList();
        }

        /// <summary>
        /// 导出单个APK到指定目录
        /// </summary>
        public async Task<bool> ExportApkAsync(string serialNumber, AppInfo app, string destFolder, CancellationToken ct)
        {
            // 清理包名中的非法文件名字符，生成安全的文件名
            string safeFileName = SanitizeFileName(app.PackageName);
            string destFile = Path.Combine(destFolder, safeFileName + ".apk");
            string args = $"-s {serialNumber} pull \"{app.ApkPath}\" \"{destFile}\"";

            Log($"开始导出: {app.AppName} ({app.PackageName})");

            if (!Directory.Exists(destFolder))
            {
                try
                {
                    Directory.CreateDirectory(destFolder);
                }
                catch (Exception ex)
                {
                    Log($"[错误] 无法创建目录 {destFolder}: {ex.Message}");
                    return false;
                }
            }

            string output;
            try
            {
                output = await RunAdbCommandAsync(args, 180000, ct);
            }
            catch (OperationCanceledException)
            {
                Log($"[取消] 导出 {app.AppName} 已被取消");
                return false;
            }

            if (output != null && (output.Contains("1 file pulled") || output.Contains("100%")))
            {
                if (File.Exists(destFile))
                {
                    var fi = new FileInfo(destFile);
                    if (fi.Length > 0)
                    {
                        Log($"[成功] {app.AppName} -> {destFile} ({FormatFileSize(fi.Length)})");
                        return true;
                    }
                    else
                    {
                        try { File.Delete(destFile); } catch { }
                        Log($"[失败] {app.AppName} - 导出文件为空");
                        return false;
                    }
                }
            }

            Log($"[失败] {app.AppName} - ADB返回: {SafeTrim(output)}");
            return false;
        }

        /// <summary>
        /// 批量导出APK
        /// </summary>
        public async Task<int> BatchExportApkAsync(string serialNumber, List<AppInfo> apps, string destFolder, CancellationToken ct)
        {
            int successCount = 0;
            int total = apps.Count;
            OnExportProgress?.Invoke(0, total);

            for (int i = 0; i < apps.Count; i++)
            {
                if (ct.IsCancellationRequested)
                {
                    Log("[取消] 批量导出已被用户取消");
                    break;
                }

                bool success = await ExportApkAsync(serialNumber, apps[i], destFolder, ct);
                if (success)
                    successCount++;

                OnExportProgress?.Invoke(i + 1, total);
            }

            return successCount;
        }

        /// <summary>
        /// 通过aapt工具获取单个应用的显示名称
        /// 原理：将APK拉取到临时文件，用aapt dump badging解析应用标签
        /// </summary>
        public async Task<string> GetAppLabelViaAaptAsync(string serialNumber, string apkPath, CancellationToken ct = default)
        {
            if (!HasAapt)
                return null;

            string tempFile = Path.Combine(Path.GetTempPath(), $"apk_label_{Guid.NewGuid():N}.apk");

            try
            {
                // 拉取APK到临时文件
                string pullOutput = await RunAdbCommandAsync(
                    $"-s {serialNumber} pull \"{apkPath}\" \"{tempFile}\"", 120000, ct);

                if (!File.Exists(tempFile))
                    return null;

                // 运行aapt dump badging
                string aaptOutput = await RunProcessAsync(_aaptPath, $"dump badging \"{tempFile}\"", 15000);

                if (string.IsNullOrWhiteSpace(aaptOutput))
                    return null;

                // 优先查找中文标签，其次查找默认标签
                // 格式: application-label-zh-CN:'应用名称'  或  application-label:'App Name'
                Match zhCnMatch = Regex.Match(aaptOutput, @"application-label-zh-CN:'([^']*)'");
                if (zhCnMatch.Success && !string.IsNullOrEmpty(zhCnMatch.Groups[1].Value))
                    return zhCnMatch.Groups[1].Value;

                Match zhMatch = Regex.Match(aaptOutput, @"application-label-zh:'([^']*)'");
                if (zhMatch.Success && !string.IsNullOrEmpty(zhMatch.Groups[1].Value))
                    return zhMatch.Groups[1].Value;

                Match defaultMatch = Regex.Match(aaptOutput, @"application-label:'([^']*)'");
                if (defaultMatch.Success && !string.IsNullOrEmpty(defaultMatch.Groups[1].Value))
                    return defaultMatch.Groups[1].Value;

                return null;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Log($"[警告] aapt解析失败 ({apkPath}): {ex.Message}");
                return null;
            }
            finally
            {
                try { if (File.Exists(tempFile)) File.Delete(tempFile); } catch { }
            }
        }

        /// <summary>
        /// 批量通过aapt获取应用显示名称（仅对未获取到名称的应用）
        /// </summary>
        public async Task<int> FetchAppLabelsViaAaptAsync(string serialNumber, List<AppInfo> apps, CancellationToken ct)
        {
            if (!HasAapt)
            {
                Log("[错误] 未找到aapt工具，无法获取显示名称");
                return 0;
            }

            int total = apps.Count;
            int fetched = 0;
            OnLabelProgress?.Invoke(0, total, "");

            for (int i = 0; i < apps.Count; i++)
            {
                if (ct.IsCancellationRequested)
                {
                    Log("[取消] 获取显示名称已被用户取消");
                    break;
                }

                var app = apps[i];
                OnLabelProgress?.Invoke(i + 1, total, app.PackageName);

                // 跳过已获取到真实名称的应用（名称不等于包名）
                if (!string.IsNullOrEmpty(app.AppName) && app.AppName != app.PackageName)
                    continue;

                Log($"正在获取显示名称: {app.PackageName} ...");

                try
                {
                    string label = await GetAppLabelViaAaptAsync(serialNumber, app.ApkPath, ct);
                    if (!string.IsNullOrEmpty(label))
                    {
                        app.AppName = label;
                        fetched++;
                        Log($"[成功] {app.PackageName} -> {label}");
                    }
                    else
                    {
                        Log($"[跳过] {app.PackageName} - 未能解析显示名称");
                    }
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    Log($"[失败] {app.PackageName} - {ex.Message}");
                }
            }

            return fetched;
        }

        /// <summary>
        /// 运行ADB命令（核心方法）
        /// </summary>
        private async Task<string> RunAdbCommandAsync(string arguments, int timeoutMs = 15000, CancellationToken ct = default)
        {
            var psi = new ProcessStartInfo
            {
                FileName = _adbPath,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8
            };

            var process = new Process { StartInfo = psi };
            var outputBuilder = new StringBuilder();
            var errorBuilder = new StringBuilder();

            try
            {
                process.Start();
            }
            catch (Exception ex)
            {
                Log($"[错误] 启动ADB失败: {ex.Message}");
                throw;
            }

            // 异步读取输出
            var readOutputTask = Task.Run(() =>
            {
                using (var reader = process.StandardOutput)
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                        outputBuilder.AppendLine(line);
                }
            }, ct);

            var readErrorTask = Task.Run(() =>
            {
                using (var reader = process.StandardError)
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                        errorBuilder.AppendLine(line);
                }
            }, ct);

            // 等待进程退出，支持超时和取消
            using (var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(ct))
            {
                timeoutCts.CancelAfter(timeoutMs);

                try
                {
                    while (!process.HasExited)
                    {
                        await Task.Delay(100, timeoutCts.Token);
                    }
                }
                catch (OperationCanceledException)
                {
                    try { process.Kill(); } catch { }
                    if (ct.IsCancellationRequested)
                        throw;
                    Log($"[警告] ADB命令超时: adb {arguments}");
                    return string.Empty;
                }
            }

            await Task.WhenAll(readOutputTask, readErrorTask);

            string output = outputBuilder.ToString();
            string error = errorBuilder.ToString();

            // ADB pull 的进度信息输出在stderr，合并返回
            if (!string.IsNullOrEmpty(error) && (arguments.Contains("pull") || output.Trim().Length == 0))
                return output + error;

            return output;
        }

        private void Log(string message)
        {
            OnLogMessage?.Invoke(message);
        }

        /// <summary>
        /// 运行通用外部进程（如aapt）
        /// </summary>
        private async Task<string> RunProcessAsync(string fileName, string arguments, int timeoutMs = 10000)
        {
            var psi = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8
            };

            using (var process = new Process { StartInfo = psi })
            {
                var outputBuilder = new StringBuilder();
                var errorBuilder = new StringBuilder();

                process.Start();

                var readOutput = Task.Run(() =>
                {
                    string line;
                    while ((line = process.StandardOutput.ReadLine()) != null)
                        outputBuilder.AppendLine(line);
                });

                var readError = Task.Run(() =>
                {
                    string line;
                    while ((line = process.StandardError.ReadLine()) != null)
                        errorBuilder.AppendLine(line);
                });

                using (var cts = new CancellationTokenSource(timeoutMs))
                {
                    try
                    {
                        while (!process.HasExited)
                            await Task.Delay(50, cts.Token);
                    }
                    catch (OperationCanceledException)
                    {
                        try { process.Kill(); } catch { }
                        return outputBuilder.ToString();
                    }
                }

                await Task.WhenAll(readOutput, readError);
                return outputBuilder.ToString() + errorBuilder.ToString();
            }
        }

        private static string SafeTrim(string s)
        {
            return s?.Trim() ?? "";
        }

        /// <summary>
        /// 清理文件名中的非法字符
        /// </summary>
        private static string SanitizeFileName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return "unknown";

            char[] invalidChars = Path.GetInvalidFileNameChars();
            var sb = new StringBuilder();
            foreach (char c in name)
            {
                if (Array.IndexOf(invalidChars, c) >= 0)
                    sb.Append('_');
                else
                    sb.Append(c);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 格式化文件大小
        /// </summary>
        public static string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            int order = 0;
            double size = bytes;
            while (size >= 1024 && order < sizes.Length - 1)
            {
                order++;
                size /= 1024;
            }
            return $"{size:0.##} {sizes[order]}";
        }
    }
}
