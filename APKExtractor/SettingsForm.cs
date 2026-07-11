using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using APKExtractor.Models;

namespace APKExtractor
{
    public partial class SettingsForm : Form
    {
        private readonly AppConfig _config;
        private readonly AdbHelper _adb;

        /// <summary>配置是否已保存</summary>
        public bool Saved { get; private set; }

        /// <summary>外部设置导出路径（在ShowDialog前调用）</summary>
        public void SetExportPath(string path)
        {
            _presetExportPath = path;
        }

        private string _presetExportPath;

        public SettingsForm(AppConfig config)
        {
            InitializeComponent();
            _config = config ?? new AppConfig();
            _adb = new AdbHelper();
        }

        // ==================== 窗体加载 ====================

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            txtAdbPath.Text = _config.AdbPath ?? "";
            txtAaptPath.Text = _config.AaptPath ?? "";

            // 优先使用预设置路径，其次使用配置中的路径
            txtExportPath.Text = !string.IsNullOrEmpty(_presetExportPath)
                ? _presetExportPath
                : (_config.ExportPath ?? "");

            UpdateStatus();
        }

        // ==================== 浏览按钮 ====================

        private void btnBrowseAdb_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Title = "选择 adb.exe";
                dlg.Filter = "可执行文件 (*.exe)|*.exe|所有文件 (*.*)|*.*";
                dlg.FileName = "adb.exe";
                if (!string.IsNullOrEmpty(txtAdbPath.Text) && File.Exists(txtAdbPath.Text))
                    dlg.InitialDirectory = Path.GetDirectoryName(txtAdbPath.Text);

                if (dlg.ShowDialog() == DialogResult.OK)
                    txtAdbPath.Text = dlg.FileName;
            }
            UpdateStatus();
        }

        private void btnBrowseAapt_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Title = "选择 aapt.exe";
                dlg.Filter = "可执行文件 (*.exe)|*.exe|所有文件 (*.*)|*.*";
                dlg.FileName = "aapt.exe";
                if (!string.IsNullOrEmpty(txtAaptPath.Text) && File.Exists(txtAaptPath.Text))
                    dlg.InitialDirectory = Path.GetDirectoryName(txtAaptPath.Text);

                if (dlg.ShowDialog() == DialogResult.OK)
                    txtAaptPath.Text = dlg.FileName;
            }
            UpdateStatus();
        }

        private void btnBrowseExport_Click(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog())
            {
                dlg.Description = "选择默认导出目录";
                dlg.ShowNewFolderButton = true;
                if (!string.IsNullOrEmpty(txtExportPath.Text) && Directory.Exists(txtExportPath.Text))
                    dlg.SelectedPath = txtExportPath.Text;

                if (dlg.ShowDialog() == DialogResult.OK)
                    txtExportPath.Text = dlg.SelectedPath;
            }
        }

        // ==================== 自动检测 ====================

        private void btnAutoDetectAdb_Click(object sender, EventArgs e)
        {
            txtAdbPath.Text = "";
            UpdateStatus();
        }

        private void btnAutoDetectAapt_Click(object sender, EventArgs e)
        {
            txtAaptPath.Text = "";
            UpdateStatus();
        }

        private void btnResetExport_Click(object sender, EventArgs e)
        {
            txtExportPath.Text = "";
        }

        // ==================== 测试按钮 ====================

        private async void btnTestAdb_Click(object sender, EventArgs e)
        {
            btnTestAdb.Enabled = false;
            lblAdbStatus.Text = "ADB: 测试中...";
            lblAdbStatus.ForeColor = Color.Gray;

            try
            {
                var adb = new AdbHelper();
                if (!string.IsNullOrWhiteSpace(txtAdbPath.Text))
                    adb.SetAdbPath(txtAdbPath.Text.Trim());

                bool ok = await adb.CheckAdbAvailableAsync();
                if (ok)
                {
                    lblAdbStatus.Text = $"ADB: 可用 ✓";
                    lblAdbStatus.ForeColor = Color.Green;
                }
                else
                {
                    lblAdbStatus.Text = "ADB: 不可用 ✗";
                    lblAdbStatus.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                lblAdbStatus.Text = $"ADB: 错误 - {ex.Message}";
                lblAdbStatus.ForeColor = Color.Red;
            }
            finally
            {
                btnTestAdb.Enabled = true;
            }
        }

        private void btnTestAapt_Click(object sender, EventArgs e)
        {
            string path = txtAaptPath.Text.Trim();
            if (string.IsNullOrEmpty(path))
            {
                var adb = new AdbHelper();
                path = adb.AaptPath;
            }

            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                lblAaptStatus.Text = "aapt: 未找到 ✗";
                lblAaptStatus.ForeColor = Color.Red;
                return;
            }

            lblAaptStatus.Text = $"aapt: 可用 ✓ ({Path.GetFileName(path)})";
            lblAaptStatus.ForeColor = Color.Green;
        }

        // ==================== 状态更新 ====================

        private void UpdateStatus()
        {
            // ADB状态
            string adbPath = txtAdbPath.Text.Trim();
            if (string.IsNullOrEmpty(adbPath))
            {
                lblAdbStatus.Text = $"ADB: 自动查找 -> {(_adb.AdbPath != "adb.exe" && File.Exists(_adb.AdbPath) ? "已找到 ✓" : "未找到 ✗")}";
                lblAdbStatus.ForeColor = File.Exists(_adb.AdbPath) ? Color.Green : Color.OrangeRed;
            }
            else
            {
                bool exists = File.Exists(adbPath);
                lblAdbStatus.Text = $"ADB: {(exists ? "路径有效 ✓" : "路径无效 ✗")}";
                lblAdbStatus.ForeColor = exists ? Color.Green : Color.Red;
            }

            // aapt状态
            string aaptPath = txtAaptPath.Text.Trim();
            if (string.IsNullOrEmpty(aaptPath))
            {
                bool found = !string.IsNullOrEmpty(_adb.AaptPath) && File.Exists(_adb.AaptPath);
                lblAaptStatus.Text = $"aapt: 自动查找 -> {(found ? "已找到 ✓" : "未找到 ✗")}";
                lblAaptStatus.ForeColor = found ? Color.Green : Color.OrangeRed;
            }
            else
            {
                bool exists = File.Exists(aaptPath);
                lblAaptStatus.Text = $"aapt: {(exists ? "路径有效 ✓" : "路径无效 ✗")}";
                lblAaptStatus.ForeColor = exists ? Color.Green : Color.Red;
            }
        }

        // ==================== 保存/取消 ====================

        private void btnSave_Click(object sender, EventArgs e)
        {
            _config.AdbPath = string.IsNullOrWhiteSpace(txtAdbPath.Text) ? null : txtAdbPath.Text.Trim();
            _config.AaptPath = string.IsNullOrWhiteSpace(txtAaptPath.Text) ? null : txtAaptPath.Text.Trim();
            _config.ExportPath = string.IsNullOrWhiteSpace(txtExportPath.Text) ? null : txtExportPath.Text.Trim();

            _config.Save();
            Saved = true;

            MessageBox.Show("配置已保存！\n部分设置将在下次启动或刷新后生效。",
                "保存成功", MessageBoxButtons.OK, MessageBoxIcon.Information);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
