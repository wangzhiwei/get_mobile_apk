using System;
using System.Collections.Generic;

namespace APKExtractor
{
    /// <summary>
    /// 语言管理器 - 支持中英文切换
    /// </summary>
    public static class LanguageManager
    {
        private static string _currentLanguage = "zh";
        
        public static string CurrentLanguage
        {
            get => _currentLanguage;
            set
            {
                if (value == "zh" || value == "en")
                {
                    _currentLanguage = value;
                }
            }
        }

        private static readonly Dictionary<string, Dictionary<string, string>> Strings = new Dictionary<string, Dictionary<string, string>>
        {
            ["zh"] = new Dictionary<string, string>
            {
                // 主窗体
                ["MainForm_Title"] = "APK导出工具 v1.0",
                ["MainForm_GroupBoxDevices"] = "已连接设备",
                ["MainForm_GroupBoxApps"] = "可导出应用列表（勾选需要导出的应用）",
                ["MainForm_BtnRefreshDevices"] = "刷新设备",
                ["MainForm_BtnRefreshApps"] = "刷新应用",
                ["MainForm_BtnSelectAll"] = "全选",
                ["MainForm_BtnDeselectAll"] = "取消全选",
                ["MainForm_BtnFetchLabels"] = "获取显示名称",
                ["MainForm_BtnSettings"] = "⚙ 设置",
                ["MainForm_LblExportPath"] = "导出路径：",
                ["MainForm_BtnBrowse"] = "浏览...",
                ["MainForm_BtnExport"] = "导出选中",
                ["MainForm_BtnCancel"] = "取消",
                ["MainForm_LblDeviceCount"] = "设备数: {0}",
                ["MainForm_LblAppCount"] = "应用数: {0}",
                ["MainForm_LblProgress"] = "就绪",
                ["MainForm_LblProgress_Exporting"] = "导出进度: {0} / {1}",
                ["MainForm_LblProgress_Complete"] = "导出完成",
                ["MainForm_LblProgress_FetchingLabels"] = "获取显示名称: {0}/{1} - {2}",
                
                // 列标题
                ["MainForm_ColDeviceModel"] = "设备型号",
                ["MainForm_ColDeviceSerial"] = "序列号",
                ["MainForm_ColAppName"] = "应用名称",
                ["MainForm_ColPackageName"] = "包名",
                ["MainForm_ColVersion"] = "版本",
                ["MainForm_ColApkPath"] = "APK路径",
                
                // 日志消息
                ["MainForm_Log_Startup"] = "===== APK导出工具启动 =====",
                ["MainForm_Log_AdbPath"] = "[信息] ADB路径: {0}",
                ["MainForm_Log_AaptPath"] = "[信息] aapt路径: {0}",
                ["MainForm_Log_AaptNotFound"] = "未找到",
                ["MainForm_Log_AdbNotFound"] = "[错误] 未找到ADB工具！",
                ["MainForm_Log_AdbNotFoundHint"] = "请通过 [设置] 按钮配置ADB路径，或将adb.exe放在以下位置之一：",
                ["MainForm_Log_AdbNotFoundHint1"] = "  1. 程序运行目录",
                ["MainForm_Log_AdbNotFoundHint2"] = "  2. ANDROID_HOME/platform-tools/",
                ["MainForm_Log_AdbNotFoundHint3"] = "  3. 系统PATH环境变量中",
                ["MainForm_Log_DetectedDevices"] = "[信息] 检测到 {0} 台设备",
                ["MainForm_Log_NoDevice"] = "[提示] 未检测到设备，请连接设备后点击刷新",
                ["MainForm_Log_DetectingDevices"] = "正在检测...",
                ["MainForm_Log_DetectFailed"] = "检测失败",
                ["MainForm_Log_GettingApps"] = "[信息] 正在获取设备 {0} 的应用列表...",
                ["MainForm_Log_GotApps"] = "[信息] 获取到 {0} 个可导出应用",
                ["MainForm_Log_StartExport"] = "========== 开始批量导出 ==========",
                ["MainForm_Log_ExportTarget"] = "目标目录: {0}",
                ["MainForm_Log_ExportCount"] = "应用数量: {0}",
                ["MainForm_Log_ExportCancelled"] = "========== 导出已取消 ==========",
                ["MainForm_Log_ExportComplete"] = "========== 导出完成：成功 {0}，失败 {1} ==========",
                ["MainForm_Log_StartFetchLabels"] = "========== 开始获取显示名称 ==========",
                ["MainForm_Log_FetchLabelsCount"] = "共 {0} 个应用，将使用 aapt 解析APK获取真实显示名称",
                ["MainForm_Log_FetchLabelsCancelled"] = "========== 获取显示名称已取消 ==========",
                ["MainForm_Log_FetchLabelsComplete"] = "========== 获取完成：成功获取 {0} 个应用的显示名称 ==========",
                ["MainForm_Log_ConfigUpdated"] = "[信息] 配置已更新",
                ["MainForm_Log_DeviceStateWarning"] = "[警告] 设备 {0} 状态为 {1}，无法操作",
                
                // 消息框
                ["MainForm_Msg_AdbNotFound"] = "未检测到ADB（Android Debug Bridge）工具！\n\n请点击 [设置] 按钮手动指定 adb.exe 路径，\n或将 adb.exe 放在程序目录下。\n下载地址：https://developer.android.com/studio/releases/platform-tools",
                ["MainForm_Msg_AdbNotFoundTitle"] = "ADB未找到",
                ["MainForm_Msg_SelectDeviceFirst"] = "请先在左侧选择一个设备。",
                ["MainForm_Msg_SelectDeviceFirstTitle"] = "提示",
                ["MainForm_Msg_DeviceUnavailable"] = "所选设备不可用。",
                ["MainForm_Msg_DeviceUnavailableTitle"] = "错误",
                ["MainForm_Msg_DeviceStateError"] = "设备状态异常：{0}\n请检查USB调试是否开启，并在手机上授权调试。",
                ["MainForm_Msg_DeviceStateErrorTitle"] = "设备不可用",
                ["MainForm_Msg_AppListEmpty"] = "应用列表为空，请先获取应用列表。",
                ["MainForm_Msg_AaptNotFound"] = "未找到 aapt 工具，无法获取应用显示名称。\n\naapt 是 Android SDK Build Tools 中的工具，用于解析APK文件。\n请将 aapt.exe 放在以下位置之一：\n  1. 程序运行目录\n  2. ANDROID_HOME/build-tools/<版本>/ 目录下\n\n下载地址：https://developer.android.com/studio/releases/build-tools",
                ["MainForm_Msg_AaptNotFoundTitle"] = "aapt未找到",
                ["MainForm_Msg_SelectAppsFirst"] = "请勾选需要导出的应用。",
                ["MainForm_Msg_ExportPathInvalid"] = "导出路径无效：{0}",
                ["MainForm_Msg_ExportPathInvalidTitle"] = "错误",
                ["MainForm_Msg_ExportCancelled"] = "导出操作已取消。",
                ["MainForm_Msg_ExportCancelledTitle"] = "已取消",
                ["MainForm_Msg_ExportComplete"] = "全部 {0} 个APK导出成功！\n\n导出目录：{1}",
                ["MainForm_Msg_ExportCompleteTitle"] = "导出完成",
                ["MainForm_Msg_ExportPartial"] = "导出完成。\n成功：{0} 个\n失败：{1} 个\n\n导出目录：{2}\n\n请查看日志了解失败详情。",
                ["MainForm_Msg_ExportPartialTitle"] = "导出完成（部分失败）",
                ["MainForm_Msg_ExportError"] = "导出过程发生异常：{0}",
                ["MainForm_Msg_ExportErrorTitle"] = "错误",
                ["MainForm_Msg_FetchLabelsComplete"] = "获取完成！\n成功获取 {0} 个应用的显示名称。\n（已有名称的应用已跳过）",
                ["MainForm_Msg_FetchLabelsCompleteTitle"] = "获取完成",
                ["MainForm_Msg_FetchLabelsError"] = "获取显示名称时发生异常：{0}",
                ["MainForm_Msg_FetchLabelsErrorTitle"] = "错误",
                ["MainForm_Msg_ConfirmExit"] = "导出正在进行中，确定要退出吗？",
                ["MainForm_Msg_ConfirmExitTitle"] = "确认退出",
                ["MainForm_Msg_DetectError"] = "检测设备失败：{0}",
                ["MainForm_Msg_DetectErrorTitle"] = "错误",
                ["MainForm_Msg_GetAppsError"] = "获取应用列表失败：{0}",
                ["MainForm_Msg_GetAppsErrorTitle"] = "错误",
                
                // 设置窗体
                ["SettingsForm_Title"] = "设置 - 工具路径配置",
                ["SettingsForm_LblAdbPath"] = "ADB路径：",
                ["SettingsForm_LblAaptPath"] = "aapt路径：",
                ["SettingsForm_LblExportPath"] = "导出路径：",
                ["SettingsForm_BtnBrowseAdb"] = "浏览...",
                ["SettingsForm_BtnBrowseAapt"] = "浏览...",
                ["SettingsForm_BtnBrowseExport"] = "浏览...",
                ["SettingsForm_BtnAutoDetectAdb"] = "自动检测",
                ["SettingsForm_BtnAutoDetectAapt"] = "自动检测",
                ["SettingsForm_BtnResetExport"] = "默认",
                ["SettingsForm_GrpStatus"] = "工具状态",
                ["SettingsForm_LblAdbStatus"] = "ADB: 检测中",
                ["SettingsForm_LblAaptStatus"] = "aapt: 检测中",
                ["SettingsForm_BtnTestAdb"] = "测试ADB",
                ["SettingsForm_BtnTestAapt"] = "测试aapt",
                ["SettingsForm_LblHint"] = "提示：留空路径将自动查找。ADB来自Platform Tools，aapt来自Build Tools。\n配置保存在程序目录下 config.json 文件中。",
                ["SettingsForm_BtnSave"] = "保存",
                ["SettingsForm_BtnCancel"] = "取消",
                ["SettingsForm_BrowseAdbTitle"] = "选择 adb.exe",
                ["SettingsForm_BrowseAaptTitle"] = "选择 aapt.exe",
                ["SettingsForm_BrowseExportTitle"] = "选择默认导出目录",
                ["SettingsForm_FileFilter"] = "可执行文件 (*.exe)|*.exe|所有文件 (*.*)|*.*",
                ["SettingsForm_AdbStatus_Auto"] = "ADB: 自动查找 -> {0}",
                ["SettingsForm_AdbStatus_AutoFound"] = "已找到 ✓",
                ["SettingsForm_AdbStatus_AutoNotFound"] = "未找到 ✗",
                ["SettingsForm_AdbStatus_Manual"] = "ADB: {0}",
                ["SettingsForm_AdbStatus_Valid"] = "路径有效 ✓",
                ["SettingsForm_AdbStatus_Invalid"] = "路径无效 ✗",
                ["SettingsForm_AdbStatus_Testing"] = "ADB: 测试中...",
                ["SettingsForm_AdbStatus_Available"] = "ADB: 可用 ✓",
                ["SettingsForm_AdbStatus_Unavailable"] = "ADB: 不可用 ✗",
                ["SettingsForm_AdbStatus_Error"] = "ADB: 错误 - {0}",
                ["SettingsForm_AaptStatus_Auto"] = "aapt: 自动查找 -> {0}",
                ["SettingsForm_AaptStatus_AutoFound"] = "已找到 ✓",
                ["SettingsForm_AaptStatus_AutoNotFound"] = "未找到 ✗",
                ["SettingsForm_AaptStatus_Manual"] = "aapt: {0}",
                ["SettingsForm_AaptStatus_Valid"] = "路径有效 ✓",
                ["SettingsForm_AaptStatus_Invalid"] = "路径无效 ✗",
                ["SettingsForm_AaptStatus_NotFound"] = "aapt: 未找到 ✗",
                ["SettingsForm_AaptStatus_Available"] = "aapt: 可用 ✓ ({0})",
                ["SettingsForm_SaveSuccess"] = "配置已保存！\n部分设置将在下次启动或刷新后生效。",
                ["SettingsForm_SaveSuccessTitle"] = "保存成功",
            },
            
            ["en"] = new Dictionary<string, string>
            {
                // Main Form
                ["MainForm_Title"] = "APK Extractor v1.0",
                ["MainForm_GroupBoxDevices"] = "Connected Devices",
                ["MainForm_GroupBoxApps"] = "Exportable Apps (Check apps to export)",
                ["MainForm_BtnRefreshDevices"] = "Refresh Devices",
                ["MainForm_BtnRefreshApps"] = "Refresh Apps",
                ["MainForm_BtnSelectAll"] = "Select All",
                ["MainForm_BtnDeselectAll"] = "Deselect All",
                ["MainForm_BtnFetchLabels"] = "Fetch Labels",
                ["MainForm_BtnSettings"] = "⚙ Settings",
                ["MainForm_LblExportPath"] = "Export Path:",
                ["MainForm_BtnBrowse"] = "Browse...",
                ["MainForm_BtnExport"] = "Export Selected",
                ["MainForm_BtnCancel"] = "Cancel",
                ["MainForm_LblDeviceCount"] = "Devices: {0}",
                ["MainForm_LblAppCount"] = "Apps: {0}",
                ["MainForm_LblProgress"] = "Ready",
                ["MainForm_LblProgress_Exporting"] = "Export Progress: {0} / {1}",
                ["MainForm_LblProgress_Complete"] = "Export Complete",
                ["MainForm_LblProgress_FetchingLabels"] = "Fetching Labels: {0}/{1} - {2}",
                
                // Column Headers
                ["MainForm_ColDeviceModel"] = "Device Model",
                ["MainForm_ColDeviceSerial"] = "Serial Number",
                ["MainForm_ColAppName"] = "App Name",
                ["MainForm_ColPackageName"] = "Package Name",
                ["MainForm_ColVersion"] = "Version",
                ["MainForm_ColApkPath"] = "APK Path",
                
                // Log Messages
                ["MainForm_Log_Startup"] = "===== APK Extractor Started =====",
                ["MainForm_Log_AdbPath"] = "[Info] ADB Path: {0}",
                ["MainForm_Log_AaptPath"] = "[Info] aapt Path: {0}",
                ["MainForm_Log_AaptNotFound"] = "Not Found",
                ["MainForm_Log_AdbNotFound"] = "[Error] ADB tool not found!",
                ["MainForm_Log_AdbNotFoundHint"] = "Please configure ADB path via [Settings] button, or place adb.exe in one of the following locations:",
                ["MainForm_Log_AdbNotFoundHint1"] = "  1. Program directory",
                ["MainForm_Log_AdbNotFoundHint2"] = "  2. ANDROID_HOME/platform-tools/",
                ["MainForm_Log_AdbNotFoundHint3"] = "  3. System PATH environment variable",
                ["MainForm_Log_DetectedDevices"] = "[Info] Detected {0} device(s)",
                ["MainForm_Log_NoDevice"] = "[Hint] No device detected, please connect device and click refresh",
                ["MainForm_Log_DetectingDevices"] = "Detecting...",
                ["MainForm_Log_DetectFailed"] = "Detection failed",
                ["MainForm_Log_GettingApps"] = "[Info] Getting app list for device {0}...",
                ["MainForm_Log_GotApps"] = "[Info] Got {0} exportable app(s)",
                ["MainForm_Log_StartExport"] = "========== Starting Batch Export ==========",
                ["MainForm_Log_ExportTarget"] = "Target Directory: {0}",
                ["MainForm_Log_ExportCount"] = "App Count: {0}",
                ["MainForm_Log_ExportCancelled"] = "========== Export Cancelled ==========",
                ["MainForm_Log_ExportComplete"] = "========== Export Complete: Success {0}, Failed {1} ==========",
                ["MainForm_Log_StartFetchLabels"] = "========== Starting to Fetch Display Names ==========",
                ["MainForm_Log_FetchLabelsCount"] = "Total {0} app(s), will use aapt to parse APK for real display names",
                ["MainForm_Log_FetchLabelsCancelled"] = "========== Fetch Display Names Cancelled ==========",
                ["MainForm_Log_FetchLabelsComplete"] = "========== Fetch Complete: Successfully got {0} app display names ==========",
                ["MainForm_Log_ConfigUpdated"] = "[Info] Configuration updated",
                ["MainForm_Log_DeviceStateWarning"] = "[Warning] Device {0} state is {1}, cannot operate",
                
                // Message Boxes
                ["MainForm_Msg_AdbNotFound"] = "ADB (Android Debug Bridge) tool not detected!\n\nPlease click [Settings] button to manually specify adb.exe path,\nor place adb.exe in the program directory.\nDownload: https://developer.android.com/studio/releases/platform-tools",
                ["MainForm_Msg_AdbNotFoundTitle"] = "ADB Not Found",
                ["MainForm_Msg_SelectDeviceFirst"] = "Please select a device on the left first.",
                ["MainForm_Msg_SelectDeviceFirstTitle"] = "Hint",
                ["MainForm_Msg_DeviceUnavailable"] = "Selected device is unavailable.",
                ["MainForm_Msg_DeviceUnavailableTitle"] = "Error",
                ["MainForm_Msg_DeviceStateError"] = "Device state abnormal: {0}\nPlease check if USB debugging is enabled and authorize debugging on the phone.",
                ["MainForm_Msg_DeviceStateErrorTitle"] = "Device Unavailable",
                ["MainForm_Msg_AppListEmpty"] = "App list is empty, please get app list first.",
                ["MainForm_Msg_AaptNotFound"] = "aapt tool not found, cannot get app display names.\n\naapt is a tool in Android SDK Build Tools, used to parse APK files.\nPlease place aapt.exe in one of the following locations:\n  1. Program directory\n  2. ANDROID_HOME/build-tools/<version>/ directory\n\nDownload: https://developer.android.com/studio/releases/build-tools",
                ["MainForm_Msg_AaptNotFoundTitle"] = "aapt Not Found",
                ["MainForm_Msg_SelectAppsFirst"] = "Please check the apps to export.",
                ["MainForm_Msg_ExportPathInvalid"] = "Export path invalid: {0}",
                ["MainForm_Msg_ExportPathInvalidTitle"] = "Error",
                ["MainForm_Msg_ExportCancelled"] = "Export operation cancelled.",
                ["MainForm_Msg_ExportCancelledTitle"] = "Cancelled",
                ["MainForm_Msg_ExportComplete"] = "All {0} APK(s) exported successfully!\n\nExport directory: {1}",
                ["MainForm_Msg_ExportCompleteTitle"] = "Export Complete",
                ["MainForm_Msg_ExportPartial"] = "Export complete.\nSuccess: {0}\nFailed: {1}\n\nExport directory: {2}\n\nPlease check log for failure details.",
                ["MainForm_Msg_ExportPartialTitle"] = "Export Complete (Partial Failure)",
                ["MainForm_Msg_ExportError"] = "Exception occurred during export: {0}",
                ["MainForm_Msg_ExportErrorTitle"] = "Error",
                ["MainForm_Msg_FetchLabelsComplete"] = "Fetch complete!\nSuccessfully got {0} app display names.\n(Apps with existing names were skipped)",
                ["MainForm_Msg_FetchLabelsCompleteTitle"] = "Fetch Complete",
                ["MainForm_Msg_FetchLabelsError"] = "Exception occurred while fetching display names: {0}",
                ["MainForm_Msg_FetchLabelsErrorTitle"] = "Error",
                ["MainForm_Msg_ConfirmExit"] = "Export is in progress, are you sure you want to exit?",
                ["MainForm_Msg_ConfirmExitTitle"] = "Confirm Exit",
                ["MainForm_Msg_DetectError"] = "Failed to detect devices: {0}",
                ["MainForm_Msg_DetectErrorTitle"] = "Error",
                ["MainForm_Msg_GetAppsError"] = "Failed to get app list: {0}",
                ["MainForm_Msg_GetAppsErrorTitle"] = "Error",
                
                // Settings Form
                ["SettingsForm_Title"] = "Settings - Tool Path Configuration",
                ["SettingsForm_LblAdbPath"] = "ADB Path:",
                ["SettingsForm_LblAaptPath"] = "aapt Path:",
                ["SettingsForm_LblExportPath"] = "Export Path:",
                ["SettingsForm_BtnBrowseAdb"] = "Browse...",
                ["SettingsForm_BtnBrowseAapt"] = "Browse...",
                ["SettingsForm_BtnBrowseExport"] = "Browse...",
                ["SettingsForm_BtnAutoDetectAdb"] = "Auto Detect",
                ["SettingsForm_BtnAutoDetectAapt"] = "Auto Detect",
                ["SettingsForm_BtnResetExport"] = "Default",
                ["SettingsForm_GrpStatus"] = "Tool Status",
                ["SettingsForm_LblAdbStatus"] = "ADB: Detecting",
                ["SettingsForm_LblAaptStatus"] = "aapt: Detecting",
                ["SettingsForm_BtnTestAdb"] = "Test ADB",
                ["SettingsForm_BtnTestAapt"] = "Test aapt",
                ["SettingsForm_LblHint"] = "Hint: Empty paths will be auto-detected. ADB from Platform Tools, aapt from Build Tools.\nConfiguration saved in config.json in program directory.",
                ["SettingsForm_BtnSave"] = "Save",
                ["SettingsForm_BtnCancel"] = "Cancel",
                ["SettingsForm_BrowseAdbTitle"] = "Select adb.exe",
                ["SettingsForm_BrowseAaptTitle"] = "Select aapt.exe",
                ["SettingsForm_BrowseExportTitle"] = "Select Default Export Directory",
                ["SettingsForm_FileFilter"] = "Executable Files (*.exe)|*.exe|All Files (*.*)|*.*",
                ["SettingsForm_AdbStatus_Auto"] = "ADB: Auto Detect -> {0}",
                ["SettingsForm_AdbStatus_AutoFound"] = "Found ✓",
                ["SettingsForm_AdbStatus_AutoNotFound"] = "Not Found ✗",
                ["SettingsForm_AdbStatus_Manual"] = "ADB: {0}",
                ["SettingsForm_AdbStatus_Valid"] = "Path Valid ✓",
                ["SettingsForm_AdbStatus_Invalid"] = "Path Invalid ✗",
                ["SettingsForm_AdbStatus_Testing"] = "ADB: Testing...",
                ["SettingsForm_AdbStatus_Available"] = "ADB: Available ✓",
                ["SettingsForm_AdbStatus_Unavailable"] = "ADB: Unavailable ✗",
                ["SettingsForm_AdbStatus_Error"] = "ADB: Error - {0}",
                ["SettingsForm_AaptStatus_Auto"] = "aapt: Auto Detect -> {0}",
                ["SettingsForm_AaptStatus_AutoFound"] = "Found ✓",
                ["SettingsForm_AaptStatus_AutoNotFound"] = "Not Found ✗",
                ["SettingsForm_AaptStatus_Manual"] = "aapt: {0}",
                ["SettingsForm_AaptStatus_Valid"] = "Path Valid ✓",
                ["SettingsForm_AaptStatus_Invalid"] = "Path Invalid ✗",
                ["SettingsForm_AaptStatus_NotFound"] = "aapt: Not Found ✗",
                ["SettingsForm_AaptStatus_Available"] = "aapt: Available ✓ ({0})",
                ["SettingsForm_SaveSuccess"] = "Configuration saved!\nSome settings will take effect after next startup or refresh.",
                ["SettingsForm_SaveSuccessTitle"] = "Save Successful",
            }
        };

        public static string Get(string key)
        {
            if (Strings.ContainsKey(_currentLanguage) && Strings[_currentLanguage].ContainsKey(key))
            {
                return Strings[_currentLanguage][key];
            }
            // Fallback to Chinese if key not found
            if (Strings.ContainsKey("zh") && Strings["zh"].ContainsKey(key))
            {
                return Strings["zh"][key];
            }
            return key; // Return key itself if not found
        }

        public static string Get(string key, params object[] args)
        {
            string format = Get(key);
            try
            {
                return string.Format(format, args);
            }
            catch
            {
                return format;
            }
        }
    }
}
