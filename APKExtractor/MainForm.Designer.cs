namespace APKExtractor
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        // 主布局
        private System.Windows.Forms.SplitContainer mainSplitContainer;
        private System.Windows.Forms.TableLayoutPanel bottomLayoutPanel;

        // 左侧：设备列表
        private System.Windows.Forms.GroupBox grpDevices;
        private System.Windows.Forms.ListView lvwDevices;
        private System.Windows.Forms.Button btnRefreshDevices;
        private System.Windows.Forms.Label lblDeviceCount;
        private System.Windows.Forms.Button btnSettings;

        // 右侧：应用列表
        private System.Windows.Forms.GroupBox grpApps;
        private System.Windows.Forms.ListView lvwApps;
        private System.Windows.Forms.Panel pnlAppButtons;
        private System.Windows.Forms.Button btnRefreshApps;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Button btnDeselectAll;
        private System.Windows.Forms.Button btnFetchLabels;
        private System.Windows.Forms.Label lblAppCount;

        // 底部：导出控制 + 进度 + 日志
        private System.Windows.Forms.Label lblExportPath;
        private System.Windows.Forms.TextBox txtExportPath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnCancelExport;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.TextBox txtLog;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.mainSplitContainer = new System.Windows.Forms.SplitContainer();
            this.grpDevices = new System.Windows.Forms.GroupBox();
            this.lvwDevices = new System.Windows.Forms.ListView();
            this.btnRefreshDevices = new System.Windows.Forms.Button();
            this.lblDeviceCount = new System.Windows.Forms.Label();
            this.btnSettings = new System.Windows.Forms.Button();
            this.grpApps = new System.Windows.Forms.GroupBox();
            this.lvwApps = new System.Windows.Forms.ListView();
            this.pnlAppButtons = new System.Windows.Forms.Panel();
            this.btnRefreshApps = new System.Windows.Forms.Button();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.btnDeselectAll = new System.Windows.Forms.Button();
            this.btnFetchLabels = new System.Windows.Forms.Button();
            this.lblAppCount = new System.Windows.Forms.Label();
            this.bottomLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.lblExportPath = new System.Windows.Forms.Label();
            this.txtExportPath = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnCancelExport = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblProgress = new System.Windows.Forms.Label();
            this.txtLog = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).BeginInit();
            this.mainSplitContainer.Panel1.SuspendLayout();
            this.mainSplitContainer.Panel2.SuspendLayout();
            this.mainSplitContainer.SuspendLayout();
            this.grpDevices.SuspendLayout();
            this.grpApps.SuspendLayout();
            this.pnlAppButtons.SuspendLayout();
            this.bottomLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainSplitContainer
            // 
            this.mainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.mainSplitContainer.Name = "mainSplitContainer";
            this.mainSplitContainer.Panel1.Controls.Add(this.grpDevices);
            this.mainSplitContainer.Panel2.Controls.Add(this.grpApps);
            this.mainSplitContainer.Size = new System.Drawing.Size(1000, 400);
            this.mainSplitContainer.SplitterDistance = 330;
            this.mainSplitContainer.SplitterWidth = 5;
            this.mainSplitContainer.TabIndex = 0;
            // 
            // grpDevices
            // 
            this.grpDevices.Controls.Add(this.lvwDevices);
            this.grpDevices.Controls.Add(this.btnRefreshDevices);
            this.grpDevices.Controls.Add(this.lblDeviceCount);
            this.grpDevices.Controls.Add(this.btnSettings);
            this.grpDevices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpDevices.Location = new System.Drawing.Point(0, 0);
            this.grpDevices.Name = "grpDevices";
            this.grpDevices.Size = new System.Drawing.Size(330, 400);
            this.grpDevices.TabIndex = 0;
            this.grpDevices.TabStop = false;
            this.grpDevices.Text = "已连接设备";
            // 
            // lvwDevices
            // 
            this.lvwDevices.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwDevices.Columns.Add("设备型号", 160);
            this.lvwDevices.Columns.Add("序列号", 150);
            this.lvwDevices.FullRowSelect = true;
            this.lvwDevices.GridLines = true;
            this.lvwDevices.HideSelection = false;
            this.lvwDevices.Location = new System.Drawing.Point(8, 22);
            this.lvwDevices.MultiSelect = false;
            this.lvwDevices.Name = "lvwDevices";
            this.lvwDevices.Size = new System.Drawing.Size(314, 340);
            this.lvwDevices.TabIndex = 0;
            this.lvwDevices.UseCompatibleStateImageBehavior = false;
            this.lvwDevices.View = System.Windows.Forms.View.Details;
            this.lvwDevices.SelectedIndexChanged += new System.EventHandler(this.lvwDevices_SelectedIndexChanged);
            // 
            // btnRefreshDevices
            // 
            this.btnRefreshDevices.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefreshDevices.Location = new System.Drawing.Point(8, 370);
            this.btnRefreshDevices.Name = "btnRefreshDevices";
            this.btnRefreshDevices.Size = new System.Drawing.Size(85, 25);
            this.btnRefreshDevices.TabIndex = 1;
            this.btnRefreshDevices.Text = "刷新设备";
            this.btnRefreshDevices.UseVisualStyleBackColor = true;
            this.btnRefreshDevices.Click += new System.EventHandler(this.btnRefreshDevices_Click);
            // 
            // lblDeviceCount
            // 
            this.lblDeviceCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblDeviceCount.AutoSize = true;
            this.lblDeviceCount.Location = new System.Drawing.Point(99, 375);
            this.lblDeviceCount.Name = "lblDeviceCount";
            this.lblDeviceCount.Size = new System.Drawing.Size(53, 12);
            this.lblDeviceCount.TabIndex = 2;
            this.lblDeviceCount.Text = "设备数: 0";
            // 
            // btnSettings
            // 
            this.btnSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSettings.Location = new System.Drawing.Point(244, 370);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(78, 25);
            this.btnSettings.TabIndex = 3;
            this.btnSettings.Text = "⚙ 设置";
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // grpApps
            // 
            this.grpApps.Controls.Add(this.lvwApps);
            this.grpApps.Controls.Add(this.pnlAppButtons);
            this.grpApps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpApps.Location = new System.Drawing.Point(0, 0);
            this.grpApps.Name = "grpApps";
            this.grpApps.Size = new System.Drawing.Size(665, 400);
            this.grpApps.TabIndex = 0;
            this.grpApps.TabStop = false;
            this.grpApps.Text = "可导出应用列表（勾选需要导出的应用）";
            // 
            // lvwApps
            // 
            this.lvwApps.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwApps.CheckBoxes = true;
            this.lvwApps.Columns.Add("应用名称", 200);
            this.lvwApps.Columns.Add("包名", 240);
            this.lvwApps.Columns.Add("版本", 80);
            this.lvwApps.Columns.Add("APK路径", 180);
            this.lvwApps.FullRowSelect = true;
            this.lvwApps.GridLines = true;
            this.lvwApps.HideSelection = false;
            this.lvwApps.Location = new System.Drawing.Point(8, 22);
            this.lvwApps.Name = "lvwApps";
            this.lvwApps.Size = new System.Drawing.Size(649, 340);
            this.lvwApps.TabIndex = 0;
            this.lvwApps.UseCompatibleStateImageBehavior = false;
            this.lvwApps.View = System.Windows.Forms.View.Details;
            // 
            // pnlAppButtons
            // 
            this.pnlAppButtons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlAppButtons.Controls.Add(this.btnRefreshApps);
            this.pnlAppButtons.Controls.Add(this.btnSelectAll);
            this.pnlAppButtons.Controls.Add(this.btnDeselectAll);
            this.pnlAppButtons.Controls.Add(this.btnFetchLabels);
            this.pnlAppButtons.Controls.Add(this.lblAppCount);
            this.pnlAppButtons.Location = new System.Drawing.Point(8, 368);
            this.pnlAppButtons.Name = "pnlAppButtons";
            this.pnlAppButtons.Size = new System.Drawing.Size(649, 30);
            this.pnlAppButtons.TabIndex = 1;
            // 
            // btnRefreshApps
            // 
            this.btnRefreshApps.Location = new System.Drawing.Point(0, 2);
            this.btnRefreshApps.Name = "btnRefreshApps";
            this.btnRefreshApps.Size = new System.Drawing.Size(85, 25);
            this.btnRefreshApps.TabIndex = 0;
            this.btnRefreshApps.Text = "刷新应用";
            this.btnRefreshApps.UseVisualStyleBackColor = true;
            this.btnRefreshApps.Click += new System.EventHandler(this.btnRefreshApps_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Location = new System.Drawing.Point(91, 2);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(60, 25);
            this.btnSelectAll.TabIndex = 1;
            this.btnSelectAll.Text = "全选";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnDeselectAll
            // 
            this.btnDeselectAll.Location = new System.Drawing.Point(157, 2);
            this.btnDeselectAll.Name = "btnDeselectAll";
            this.btnDeselectAll.Size = new System.Drawing.Size(60, 25);
            this.btnDeselectAll.TabIndex = 2;
            this.btnDeselectAll.Text = "取消全选";
            this.btnDeselectAll.UseVisualStyleBackColor = true;
            this.btnDeselectAll.Click += new System.EventHandler(this.btnDeselectAll_Click);
            // 
            // btnFetchLabels
            // 
            this.btnFetchLabels.Location = new System.Drawing.Point(223, 2);
            this.btnFetchLabels.Name = "btnFetchLabels";
            this.btnFetchLabels.Size = new System.Drawing.Size(100, 25);
            this.btnFetchLabels.TabIndex = 4;
            this.btnFetchLabels.Text = "获取显示名称";
            this.btnFetchLabels.UseVisualStyleBackColor = true;
            this.btnFetchLabels.Click += new System.EventHandler(this.btnFetchLabels_Click);
            // 
            // lblAppCount
            // 
            this.lblAppCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAppCount.AutoSize = true;
            this.lblAppCount.Location = new System.Drawing.Point(580, 7);
            this.lblAppCount.Name = "lblAppCount";
            this.lblAppCount.Size = new System.Drawing.Size(53, 12);
            this.lblAppCount.TabIndex = 3;
            this.lblAppCount.Text = "应用数: 0";
            // 
            // bottomLayoutPanel
            // 
            this.bottomLayoutPanel.ColumnCount = 5;
            this.bottomLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.bottomLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.bottomLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.bottomLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.bottomLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.bottomLayoutPanel.Controls.Add(this.lblExportPath, 0, 0);
            this.bottomLayoutPanel.Controls.Add(this.txtExportPath, 1, 0);
            this.bottomLayoutPanel.Controls.Add(this.btnBrowse, 2, 0);
            this.bottomLayoutPanel.Controls.Add(this.btnExport, 3, 0);
            this.bottomLayoutPanel.Controls.Add(this.btnCancelExport, 4, 0);
            this.bottomLayoutPanel.Controls.Add(this.progressBar, 0, 1);
            this.bottomLayoutPanel.Controls.Add(this.lblProgress, 0, 2);
            this.bottomLayoutPanel.Controls.Add(this.txtLog, 0, 3);
            this.bottomLayoutPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomLayoutPanel.Location = new System.Drawing.Point(0, 400);
            this.bottomLayoutPanel.Name = "bottomLayoutPanel";
            this.bottomLayoutPanel.RowCount = 4;
            this.bottomLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.bottomLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.bottomLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.bottomLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.bottomLayoutPanel.Size = new System.Drawing.Size(1000, 220);
            this.bottomLayoutPanel.TabIndex = 1;
            // 
            // lblExportPath
            // 
            this.lblExportPath.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblExportPath.AutoSize = true;
            this.lblExportPath.Location = new System.Drawing.Point(14, 11);
            this.lblExportPath.Name = "lblExportPath";
            this.lblExportPath.Size = new System.Drawing.Size(63, 12);
            this.lblExportPath.TabIndex = 0;
            this.lblExportPath.Text = "导出路径：";
            // 
            // txtExportPath
            // 
            this.txtExportPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtExportPath.Location = new System.Drawing.Point(83, 7);
            this.txtExportPath.Name = "txtExportPath";
            this.txtExportPath.Size = new System.Drawing.Size(744, 21);
            this.txtExportPath.TabIndex = 1;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnBrowse.Location = new System.Drawing.Point(833, 5);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(70, 25);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "浏览...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnExport
            // 
            this.btnExport.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnExport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnExport.FlatAppearance.BorderSize = 0;
            this.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExport.ForeColor = System.Drawing.Color.White;
            this.btnExport.Location = new System.Drawing.Point(919, 5);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(80, 25);
            this.btnExport.TabIndex = 3;
            this.btnExport.Text = "导出选中";
            this.btnExport.UseVisualStyleBackColor = false;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnCancelExport
            // 
            this.btnCancelExport.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCancelExport.Enabled = false;
            this.btnCancelExport.ForeColor = System.Drawing.Color.Red;
            this.btnCancelExport.Location = new System.Drawing.Point(1011, 5);
            this.btnCancelExport.Name = "btnCancelExport";
            this.btnCancelExport.Size = new System.Drawing.Size(60, 25);
            this.btnCancelExport.TabIndex = 4;
            this.btnCancelExport.Text = "取消";
            this.btnCancelExport.UseVisualStyleBackColor = true;
            this.btnCancelExport.Click += new System.EventHandler(this.btnCancelExport_Click);
            // 
            // progressBar
            // 
            this.bottomLayoutPanel.SetColumnSpan(this.progressBar, 5);
            this.progressBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBar.Location = new System.Drawing.Point(3, 38);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(994, 19);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar.TabIndex = 5;
            // 
            // lblProgress
            // 
            this.bottomLayoutPanel.SetColumnSpan(this.lblProgress, 5);
            this.lblProgress.Location = new System.Drawing.Point(3, 63);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(994, 17);
            this.lblProgress.TabIndex = 6;
            this.lblProgress.Text = "就绪";
            // 
            // txtLog
            // 
            this.bottomLayoutPanel.SetColumnSpan(this.txtLog, 5);
            this.txtLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtLog.ForeColor = System.Drawing.Color.LightGreen;
            this.txtLog.Location = new System.Drawing.Point(3, 83);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(994, 134);
            this.txtLog.TabIndex = 7;
            this.txtLog.WordWrap = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 620);
            this.Controls.Add(this.mainSplitContainer);
            this.Controls.Add(this.bottomLayoutPanel);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.MinimumSize = new System.Drawing.Size(800, 500);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "APK导出工具 v1.0";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.mainSplitContainer.Panel1.ResumeLayout(false);
            this.mainSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).EndInit();
            this.mainSplitContainer.ResumeLayout(false);
            this.grpDevices.ResumeLayout(false);
            this.grpDevices.PerformLayout();
            this.grpApps.ResumeLayout(false);
            this.pnlAppButtons.ResumeLayout(false);
            this.pnlAppButtons.PerformLayout();
            this.bottomLayoutPanel.ResumeLayout(false);
            this.bottomLayoutPanel.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}
