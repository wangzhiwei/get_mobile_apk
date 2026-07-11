using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using APKExtractor.Models;

namespace APKExtractor
{
    public partial class MainForm : Form
    {
        private readonly AdbHelper _adb;
        private readonly AppConfig _config;
        private List<AndroidDevice> _devices = new List<AndroidDevice>();
        private CancellationTokenSource _exportCts;

        public MainForm()
        {
            InitializeComponent();
            _config = AppConfig.Load();
            
            // 设置初始语言
            LanguageManager.CurrentLanguage = _config.Language;
            cmbLanguage.SelectedIndex = _config.Language == "zh" ? 0 : 1;
            
            _adb = new AdbHelper(_config);
            _adb.OnLogMessage += AppendLog;
            _adb.OnExportProgress += UpdateProgress;
            _adb.OnLabelProgress += UpdateLabelProgress;
        }

        // ==================== 窗体加载 ====================

        private async void MainForm_Load(object sender, EventArgs e)
        {
            // 使用配置中的导出路径，或默认路径
            txtExportPath.Text = !string.IsNullOrEmpty(_config.ExportPath)
                ? _config.ExportPath
                : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "APK_Export");

            AppendLog("===== APK导出工具启动 =====");
            AppendLog($"[信息] ADB路径: {_adb.AdbPath}");
            AppendLog($"[信息] aapt路径: {(_adb.HasAapt ? _adb.AaptPath : "未找到")}");

            bool adbOk = await _adb.CheckAdbAvailableAsync();
            if (!adbOk)
            {
                AppendLog("[错误] 未找到ADB工具！");
                AppendLog("请通过 [设置] 按钮配置ADB路径，或将adb.exe放在以下位置之一：");
                AppendLog("  1. 程序运行目录");
                AppendLog("  2. ANDROID_HOME/platform-tools/");
                AppendLog("  3. 系统PATH环境变量中");

                MessageBox.Show(
                    "未检测到ADB（Android Debug Bridge）工具！\n\n" +
                    "请点击 [设置] 按钮手动指定 adb.exe 路径，\n" +
                    "或将 adb.exe 放在程序目录下。\n" +
                    "下载地址：https://developer.android.com/studio/releases/platform-tools",
                    "ADB未找到", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            await RefreshDevicesAsync();
        }

        // ==================== 设备列表 ====================

        private async void btnRefreshDevices_Click(object sender, EventArgs e)
        {
            await RefreshDevicesAsync();
        }

        private async Task RefreshDevicesAsync()
        {
            btnRefreshDevices.Enabled = false;
            lvwDevices.Items.Clear();
            _devices.Clear();
            lblDeviceCount.Text = "正在检测...";

            try
            {
                _devices = await _adb.GetDevicesAsync();

                foreach (var dev in _devices)
                {
                    var item = new ListViewItem(dev.Model);
                    item.SubItems.Add(dev.SerialNumber);
                    item.Tag = dev;

                    if (dev.State != "device")
                        item.ForeColor = Color.Gray;

                    lvwDevices.Items.Add(item);
                }

                lblDeviceCount.Text = $"设备数: {_devices.Count}";
                AppendLog(_devices.Count > 0
                    ? $"[信息] 检测到 {_devices.Count} 台设备"
                    : "[提示] 未检测到设备，请连接设备后点击刷新");
            }
            catch (Exception ex)
            {
                lblDeviceCount.Text = "检测失败";
                AppendLog($"[错误] 检测设备异常: {ex.Message}");
                MessageBox.Show($"检测设备失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnRefreshDevices.Enabled = true;
            }
        }

        private async void lvwDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvwDevices.SelectedItems.Count == 0) return;

            var dev = lvwDevices.SelectedItems[0].Tag as AndroidDevice;
            if (dev == null) return;

            if (dev.State != "device")
            {
                AppendLog($"[警告] 设备 {dev.SerialNumber} 状态为 {dev.State}，无法操作");
                MessageBox.Show($"设备状态异常：{dev.State}\n请检查USB调试是否开启，并在手机上授权调试。",
                    "设备不可用", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            await LoadAppsAsync(dev.SerialNumber);
        }

        // ==================== 应用列表 ====================

        private async void btnRefreshApps_Click(object sender, EventArgs e)
        {
            if (lvwDevices.SelectedItems.Count == 0)
            {
                MessageBox.Show("请先在左侧选择一个设备。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var dev = lvwDevices.SelectedItems[0].Tag as AndroidDevice;
            if (dev != null && dev.State == "device")
                await LoadAppsAsync(dev.SerialNumber);
        }

        private async Task LoadAppsAsync(string serial)
        {
            btnRefreshApps.Enabled = false;
            lvwApps.Items.Clear();
            AppendLog($"[信息] 正在获取设备 {serial} 的应用列表...");

            try
            {
                var apps = await _adb.GetInstalledAppsAsync(serial);

                lvwApps.BeginUpdate();
                foreach (var app in apps)
                {
                    var item = new ListViewItem(app.AppName);
                    item.SubItems.Add(app.PackageName);
                    item.SubItems.Add(app.VersionName ?? "");
                    item.SubItems.Add(app.ApkPath);
                    item.Tag = app;
                    lvwApps.Items.Add(item);
                }
                lvwApps.EndUpdate();

                lblAppCount.Text = $"应用数: {apps.Count}";
                AppendLog($"[信息] 获取到 {apps.Count} 个可导出应用");
            }
            catch (Exception ex)
            {
                AppendLog($"[错误] 获取应用列表失败: {ex.Message}");
                MessageBox.Show($"获取应用列表失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnRefreshApps.Enabled = true;
            }
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvwApps.Items)
                item.Checked = true;
        }

        private void btnDeselectAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvwApps.Items)
                item.Checked = false;
        }

        // ==================== 获取显示名称 ====================

        private async void btnFetchLabels_Click(object sender, EventArgs e)
        {
            if (lvwDevices.SelectedItems.Count == 0)
            {
                MessageBox.Show("请先在左侧选择一个设备。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var dev = lvwDevices.SelectedItems[0].Tag as AndroidDevice;
            if (dev == null || dev.State != "device")
            {
                MessageBox.Show("所选设备不可用。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (lvwApps.Items.Count == 0)
            {
                MessageBox.Show("应用列表为空，请先获取应用列表。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!_adb.HasAapt)
            {
                MessageBox.Show(
                    "未找到 aapt 工具，无法获取应用显示名称。\n\n" +
                    "aapt 是 Android SDK Build Tools 中的工具，用于解析APK文件。\n" +
                    "请将 aapt.exe 放在以下位置之一：\n" +
                    "  1. 程序运行目录\n" +
                    "  2. ANDROID_HOME/build-tools/<版本>/ 目录下\n\n" +
                    "下载地址：https://developer.android.com/studio/releases/build-tools",
                    "aapt未找到", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 收集所有应用信息
            var apps = new List<AppInfo>();
            foreach (ListViewItem item in lvwApps.Items)
            {
                if (item.Tag is AppInfo app)
                    apps.Add(app);
            }

            await FetchLabelsAsync(dev.SerialNumber, apps);
        }

        private async Task FetchLabelsAsync(string serial, List<AppInfo> apps)
        {
            SetFetchingLabelsState(true);
            progressBar.Value = 0;
            progressBar.Maximum = apps.Count;

            AppendLog("========== 开始获取显示名称 ==========");
            AppendLog($"共 {apps.Count} 个应用，将使用 aapt 解析APK获取真实显示名称");

            _exportCts = new CancellationTokenSource();

            try
            {
                int fetched = await _adb.FetchAppLabelsViaAaptAsync(serial, apps, _exportCts.Token);

                // 更新ListView显示
                lvwApps.BeginUpdate();
                foreach (ListViewItem item in lvwApps.Items)
                {
                    if (item.Tag is AppInfo app)
                    {
                        item.Text = app.AppName;
                    }
                }
                lvwApps.EndUpdate();

                if (_exportCts.IsCancellationRequested)
                {
                    AppendLog("========== 获取显示名称已取消 ==========");
                }
                else
                {
                    AppendLog($"========== 获取完成：成功获取 {fetched} 个应用的显示名称 ==========");
                    MessageBox.Show(
                        $"获取完成！\n成功获取 {fetched} 个应用的显示名称。\n" +
                        $"（已有名称的应用已跳过）",
                        "获取完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                AppendLog($"[严重错误] {ex.Message}");
                MessageBox.Show($"获取显示名称时发生异常：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetFetchingLabelsState(false);
                _exportCts?.Dispose();
                _exportCts = null;
                lblProgress.Text = "就绪";
            }
        }

        private void SetFetchingLabelsState(bool fetching)
        {
            btnFetchLabels.Enabled = !fetching;
            btnExport.Enabled = !fetching;
            btnRefreshDevices.Enabled = !fetching;
            btnRefreshApps.Enabled = !fetching;
            lvwDevices.Enabled = !fetching;
            lvwApps.Enabled = !fetching;
            btnCancelExport.Enabled = fetching;
        }

        // ==================== 导出功能 ====================

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog())
            {
                dlg.Description = "选择APK导出目录";
                dlg.ShowNewFolderButton = true;
                if (Directory.Exists(txtExportPath.Text))
                    dlg.SelectedPath = txtExportPath.Text;

                if (dlg.ShowDialog() == DialogResult.OK)
                    txtExportPath.Text = dlg.SelectedPath;
            }
        }

        private async void btnExport_Click(object sender, EventArgs e)
        {
            if (lvwDevices.SelectedItems.Count == 0)
            {
                MessageBox.Show("请先选择一个设备。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var dev = lvwDevices.SelectedItems[0].Tag as AndroidDevice;
            if (dev == null || dev.State != "device")
            {
                MessageBox.Show("所选设备不可用。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var selectedApps = new List<AppInfo>();
            foreach (ListViewItem item in lvwApps.Items)
            {
                if (item.Checked && item.Tag is AppInfo app)
                    selectedApps.Add(app);
            }

            if (selectedApps.Count == 0)
            {
                MessageBox.Show("请勾选需要导出的应用。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string path = txtExportPath.Text.Trim();
            if (string.IsNullOrEmpty(path))
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "APK_Export");

            try
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导出路径无效：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            await StartExportAsync(dev.SerialNumber, selectedApps, path);
        }

        private async Task StartExportAsync(string serial, List<AppInfo> apps, string path)
        {
            SetExportingState(true);
            progressBar.Value = 0;
            progressBar.Maximum = apps.Count;
            lblProgress.Text = $"准备导出 {apps.Count} 个应用...";

            AppendLog($"========== 开始批量导出 ==========");
            AppendLog($"目标目录: {path}");
            AppendLog($"应用数量: {apps.Count}");

            _exportCts = new CancellationTokenSource();

            try
            {
                int success = await _adb.BatchExportApkAsync(serial, apps, path, _exportCts.Token);

                if (_exportCts.IsCancellationRequested)
                {
                    AppendLog("========== 导出已取消 ==========");
                    MessageBox.Show("导出操作已取消。", "已取消", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    int failed = apps.Count - success;
                    AppendLog($"========== 导出完成：成功 {success}，失败 {failed} ==========");

                    if (failed == 0)
                    {
                        MessageBox.Show($"全部 {success} 个APK导出成功！\n\n导出目录：{path}",
                            "导出完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"导出完成。\n成功：{success} 个\n失败：{failed} 个\n\n导出目录：{path}\n\n请查看日志了解失败详情。",
                            "导出完成（部分失败）", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                AppendLog($"[严重错误] {ex.Message}");
                MessageBox.Show($"导出过程发生异常：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetExportingState(false);
                _exportCts?.Dispose();
                _exportCts = null;
            }
        }

        private void btnCancelExport_Click(object sender, EventArgs e)
        {
            if (_exportCts != null && !_exportCts.IsCancellationRequested)
            {
                _exportCts.Cancel();
                AppendLog("[操作] 正在取消导出...");
                btnCancelExport.Enabled = false;
            }
        }

        private void SetExportingState(bool exporting)
        {
            btnExport.Enabled = !exporting;
            btnBrowse.Enabled = !exporting;
            btnRefreshDevices.Enabled = !exporting;
            btnRefreshApps.Enabled = !exporting;
            lvwDevices.Enabled = !exporting;
            lvwApps.Enabled = !exporting;
            btnCancelExport.Enabled = exporting;
        }

        // ==================== 进度与日志 ====================

        private void UpdateProgress(int current, int total)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateProgress(current, total)));
                return;
            }

            if (progressBar.Maximum != total)
                progressBar.Maximum = total;
            progressBar.Value = Math.Min(current, total);
            lblProgress.Text = current >= total && total > 0
                ? "导出完成"
                : $"导出进度: {current} / {total}";
        }

        private void UpdateLabelProgress(int current, int total, string packageName)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateLabelProgress(current, total, packageName)));
                return;
            }

            if (progressBar.Maximum != total)
                progressBar.Maximum = total;
            progressBar.Value = Math.Min(current, total);
            lblProgress.Text = string.IsNullOrEmpty(packageName)
                ? $"准备获取显示名称... {current}/{total}"
                : $"获取显示名称: {current}/{total} - {packageName}";
        }

        private void AppendLog(string msg)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => AppendLog(msg)));
                return;
            }

            string time = DateTime.Now.ToString("HH:mm:ss");
            txtLog.AppendText($"[{time}] {msg}{Environment.NewLine}");
            txtLog.SelectionStart = txtLog.TextLength;
            txtLog.ScrollToCaret();
        }

        // ==================== 设置 ====================

        private void btnSettings_Click(object sender, EventArgs e)
        {
            using (var form = new SettingsForm(_config))
            {
                // 同步当前导出路径到设置窗体
                if (!string.IsNullOrEmpty(txtExportPath.Text))
                    form.SetExportPath(txtExportPath.Text);

                if (form.ShowDialog() == DialogResult.OK && form.Saved)
                {
                    // 重新加载配置
                    var newConfig = AppConfig.Load();
                    _config.AdbPath = newConfig.AdbPath;
                    _config.AaptPath = newConfig.AaptPath;
                    _config.ExportPath = newConfig.ExportPath;

                    // 更新ADB/aapt路径
                    _adb.SetAdbPath(_config.AdbPath);
                    _adb.SetAaptPath(_config.AaptPath);

                    // 更新导出路径
                    if (!string.IsNullOrEmpty(_config.ExportPath))
                        txtExportPath.Text = _config.ExportPath;

                    AppendLog("[信息] 配置已更新");
                    AppendLog($"[信息] ADB路径: {_adb.AdbPath}");
                    AppendLog($"[信息] aapt路径: {(_adb.HasAapt ? _adb.AaptPath : "未找到")}");

                    // 刷新设备列表
                    _ = RefreshDevicesAsync();
                }
            }
        }

        // ==================== 多语言支持 ====================

        private void cmbLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            string newLang = cmbLanguage.SelectedItem.ToString();
            string langCode = newLang == "English" ? "en" : "zh";
            
            if (LanguageManager.CurrentLanguage != langCode)
            {
                LanguageManager.CurrentLanguage = langCode;
                _config.Language = langCode;
                _config.Save();
                
                UpdateUIText();
                AppendLog(LanguageManager.Get("MainForm_Log_ConfigUpdated"));
            }
        }

        private void UpdateUIText()
        {
            // 更新主窗体标题
            this.Text = LanguageManager.Get("MainForm_Title");
            
            // 更新设备区域
            grpDevices.Text = LanguageManager.Get("MainForm_GroupBoxDevices");
            btnRefreshDevices.Text = LanguageManager.Get("MainForm_BtnRefreshDevices");
            btnSettings.Text = LanguageManager.Get("MainForm_BtnSettings");
            
            // 更新设备列表列标题
            if (lvwDevices.Columns.Count >= 2)
            {
                lvwDevices.Columns[0].Text = LanguageManager.Get("MainForm_ColDeviceModel");
                lvwDevices.Columns[1].Text = LanguageManager.Get("MainForm_ColDeviceSerial");
            }
            
            // 更新设备计数
            lblDeviceCount.Text = string.Format(LanguageManager.Get("MainForm_LblDeviceCount"), lvwDevices.Items.Count);
            
            // 更新应用区域
            grpApps.Text = LanguageManager.Get("MainForm_GroupBoxApps");
            btnRefreshApps.Text = LanguageManager.Get("MainForm_BtnRefreshApps");
            btnSelectAll.Text = LanguageManager.Get("MainForm_BtnSelectAll");
            btnDeselectAll.Text = LanguageManager.Get("MainForm_BtnDeselectAll");
            btnFetchLabels.Text = LanguageManager.Get("MainForm_BtnFetchLabels");
            
            // 更新应用列表列标题
            if (lvwApps.Columns.Count >= 4)
            {
                lvwApps.Columns[0].Text = LanguageManager.Get("MainForm_ColAppName");
                lvwApps.Columns[1].Text = LanguageManager.Get("MainForm_ColPackageName");
                lvwApps.Columns[2].Text = LanguageManager.Get("MainForm_ColVersion");
                lvwApps.Columns[3].Text = LanguageManager.Get("MainForm_ColApkPath");
            }
            
            // 更新应用计数
            lblAppCount.Text = string.Format(LanguageManager.Get("MainForm_LblAppCount"), lvwApps.Items.Count);
            
            // 更新底部区域
            lblExportPath.Text = LanguageManager.Get("MainForm_LblExportPath");
            btnBrowse.Text = LanguageManager.Get("MainForm_BtnBrowse");
            btnExport.Text = LanguageManager.Get("MainForm_BtnExport");
            btnCancelExport.Text = LanguageManager.Get("MainForm_BtnCancel");
            lblProgress.Text = LanguageManager.Get("MainForm_LblProgress");
        }

        // ==================== 窗体事件 ====================

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_exportCts != null && !_exportCts.IsCancellationRequested)
            {
                var r = MessageBox.Show("导出正在进行中，确定要退出吗？", "确认退出",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (r == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
                _exportCts.Cancel();
            }
        }
    }
}
