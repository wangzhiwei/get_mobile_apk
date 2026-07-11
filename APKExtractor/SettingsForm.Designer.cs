namespace APKExtractor
{
    partial class SettingsForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblAdbPath;
        private System.Windows.Forms.TextBox txtAdbPath;
        private System.Windows.Forms.Button btnBrowseAdb;
        private System.Windows.Forms.Button btnAutoDetectAdb;
        private System.Windows.Forms.Label lblAaptPath;
        private System.Windows.Forms.TextBox txtAaptPath;
        private System.Windows.Forms.Button btnBrowseAapt;
        private System.Windows.Forms.Button btnAutoDetectAapt;
        private System.Windows.Forms.Label lblExportPath;
        private System.Windows.Forms.TextBox txtExportPath;
        private System.Windows.Forms.Button btnBrowseExport;
        private System.Windows.Forms.Button btnResetExport;
        private System.Windows.Forms.GroupBox grpStatus;
        private System.Windows.Forms.Label lblAdbStatus;
        private System.Windows.Forms.Label lblAaptStatus;
        private System.Windows.Forms.Button btnTestAdb;
        private System.Windows.Forms.Button btnTestAapt;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblHint;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblAdbPath = new System.Windows.Forms.Label();
            this.txtAdbPath = new System.Windows.Forms.TextBox();
            this.btnBrowseAdb = new System.Windows.Forms.Button();
            this.btnAutoDetectAdb = new System.Windows.Forms.Button();
            this.lblAaptPath = new System.Windows.Forms.Label();
            this.txtAaptPath = new System.Windows.Forms.TextBox();
            this.btnBrowseAapt = new System.Windows.Forms.Button();
            this.btnAutoDetectAapt = new System.Windows.Forms.Button();
            this.lblExportPath = new System.Windows.Forms.Label();
            this.txtExportPath = new System.Windows.Forms.TextBox();
            this.btnBrowseExport = new System.Windows.Forms.Button();
            this.btnResetExport = new System.Windows.Forms.Button();
            this.grpStatus = new System.Windows.Forms.GroupBox();
            this.lblAdbStatus = new System.Windows.Forms.Label();
            this.lblAaptStatus = new System.Windows.Forms.Label();
            this.btnTestAdb = new System.Windows.Forms.Button();
            this.btnTestAapt = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblHint = new System.Windows.Forms.Label();
            this.grpStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblAdbPath
            // 
            this.lblAdbPath.AutoSize = true;
            this.lblAdbPath.Location = new System.Drawing.Point(12, 15);
            this.lblAdbPath.Name = "lblAdbPath";
            this.lblAdbPath.Size = new System.Drawing.Size(71, 12);
            this.lblAdbPath.Text = "ADB路径：";
            // 
            // txtAdbPath
            // 
            this.txtAdbPath.Location = new System.Drawing.Point(90, 12);
            this.txtAdbPath.Name = "txtAdbPath";
            this.txtAdbPath.Size = new System.Drawing.Size(340, 21);
            // 
            // btnBrowseAdb
            // 
            this.btnBrowseAdb.Location = new System.Drawing.Point(436, 10);
            this.btnBrowseAdb.Name = "btnBrowseAdb";
            this.btnBrowseAdb.Size = new System.Drawing.Size(70, 25);
            this.btnBrowseAdb.Text = "浏览...";
            this.btnBrowseAdb.UseVisualStyleBackColor = true;
            this.btnBrowseAdb.Click += new System.EventHandler(this.btnBrowseAdb_Click);
            // 
            // btnAutoDetectAdb
            // 
            this.btnAutoDetectAdb.Location = new System.Drawing.Point(512, 10);
            this.btnAutoDetectAdb.Name = "btnAutoDetectAdb";
            this.btnAutoDetectAdb.Size = new System.Drawing.Size(70, 25);
            this.btnAutoDetectAdb.Text = "自动检测";
            this.btnAutoDetectAdb.UseVisualStyleBackColor = true;
            this.btnAutoDetectAdb.Click += new System.EventHandler(this.btnAutoDetectAdb_Click);
            // 
            // lblAaptPath
            // 
            this.lblAaptPath.AutoSize = true;
            this.lblAaptPath.Location = new System.Drawing.Point(12, 45);
            this.lblAaptPath.Name = "lblAaptPath";
            this.lblAaptPath.Size = new System.Drawing.Size(77, 12);
            this.lblAaptPath.Text = "aapt路径：";
            // 
            // txtAaptPath
            // 
            this.txtAaptPath.Location = new System.Drawing.Point(90, 42);
            this.txtAaptPath.Name = "txtAaptPath";
            this.txtAaptPath.Size = new System.Drawing.Size(340, 21);
            // 
            // btnBrowseAapt
            // 
            this.btnBrowseAapt.Location = new System.Drawing.Point(436, 40);
            this.btnBrowseAapt.Name = "btnBrowseAapt";
            this.btnBrowseAapt.Size = new System.Drawing.Size(70, 25);
            this.btnBrowseAapt.Text = "浏览...";
            this.btnBrowseAapt.UseVisualStyleBackColor = true;
            this.btnBrowseAapt.Click += new System.EventHandler(this.btnBrowseAapt_Click);
            // 
            // btnAutoDetectAapt
            // 
            this.btnAutoDetectAapt.Location = new System.Drawing.Point(512, 40);
            this.btnAutoDetectAapt.Name = "btnAutoDetectAapt";
            this.btnAutoDetectAapt.Size = new System.Drawing.Size(70, 25);
            this.btnAutoDetectAapt.Text = "自动检测";
            this.btnAutoDetectAapt.UseVisualStyleBackColor = true;
            this.btnAutoDetectAapt.Click += new System.EventHandler(this.btnAutoDetectAapt_Click);
            // 
            // lblExportPath
            // 
            this.lblExportPath.AutoSize = true;
            this.lblExportPath.Location = new System.Drawing.Point(12, 75);
            this.lblExportPath.Name = "lblExportPath";
            this.lblExportPath.Size = new System.Drawing.Size(71, 12);
            this.lblExportPath.Text = "导出路径：";
            // 
            // txtExportPath
            // 
            this.txtExportPath.Location = new System.Drawing.Point(90, 72);
            this.txtExportPath.Name = "txtExportPath";
            this.txtExportPath.Size = new System.Drawing.Size(340, 21);
            // 
            // btnBrowseExport
            // 
            this.btnBrowseExport.Location = new System.Drawing.Point(436, 70);
            this.btnBrowseExport.Name = "btnBrowseExport";
            this.btnBrowseExport.Size = new System.Drawing.Size(70, 25);
            this.btnBrowseExport.Text = "浏览...";
            this.btnBrowseExport.UseVisualStyleBackColor = true;
            this.btnBrowseExport.Click += new System.EventHandler(this.btnBrowseExport_Click);
            // 
            // btnResetExport
            // 
            this.btnResetExport.Location = new System.Drawing.Point(512, 70);
            this.btnResetExport.Name = "btnResetExport";
            this.btnResetExport.Size = new System.Drawing.Size(70, 25);
            this.btnResetExport.Text = "默认";
            this.btnResetExport.UseVisualStyleBackColor = true;
            this.btnResetExport.Click += new System.EventHandler(this.btnResetExport_Click);
            // 
            // grpStatus
            // 
            this.grpStatus.Controls.Add(this.lblAdbStatus);
            this.grpStatus.Controls.Add(this.btnTestAdb);
            this.grpStatus.Controls.Add(this.lblAaptStatus);
            this.grpStatus.Controls.Add(this.btnTestAapt);
            this.grpStatus.Location = new System.Drawing.Point(12, 105);
            this.grpStatus.Name = "grpStatus";
            this.grpStatus.Size = new System.Drawing.Size(570, 85);
            this.grpStatus.TabIndex = 10;
            this.grpStatus.TabStop = false;
            this.grpStatus.Text = "工具状态";
            // 
            // lblAdbStatus
            // 
            this.lblAdbStatus.AutoSize = true;
            this.lblAdbStatus.Location = new System.Drawing.Point(15, 25);
            this.lblAdbStatus.Name = "lblAdbStatus";
            this.lblAdbStatus.Size = new System.Drawing.Size(65, 12);
            this.lblAdbStatus.Text = "ADB: 检测中";
            // 
            // btnTestAdb
            // 
            this.btnTestAdb.Location = new System.Drawing.Point(470, 20);
            this.btnTestAdb.Name = "btnTestAdb";
            this.btnTestAdb.Size = new System.Drawing.Size(80, 25);
            this.btnTestAdb.Text = "测试ADB";
            this.btnTestAdb.UseVisualStyleBackColor = true;
            this.btnTestAdb.Click += new System.EventHandler(this.btnTestAdb_Click);
            // 
            // lblAaptStatus
            // 
            this.lblAaptStatus.AutoSize = true;
            this.lblAaptStatus.Location = new System.Drawing.Point(15, 55);
            this.lblAaptStatus.Name = "lblAaptStatus";
            this.lblAaptStatus.Size = new System.Drawing.Size(71, 12);
            this.lblAaptStatus.Text = "aapt: 检测中";
            // 
            // btnTestAapt
            // 
            this.btnTestAapt.Location = new System.Drawing.Point(470, 50);
            this.btnTestAapt.Name = "btnTestAapt";
            this.btnTestAapt.Size = new System.Drawing.Size(80, 25);
            this.btnTestAapt.Text = "测试aapt";
            this.btnTestAapt.UseVisualStyleBackColor = true;
            this.btnTestAapt.Click += new System.EventHandler(this.btnTestAapt_Click);
            // 
            // lblHint
            // 
            this.lblHint.Location = new System.Drawing.Point(12, 198);
            this.lblHint.Name = "lblHint";
            this.lblHint.Size = new System.Drawing.Size(570, 30);
            this.lblHint.ForeColor = System.Drawing.Color.Gray;
            this.lblHint.Text = "提示：留空路径将自动查找。ADB来自Platform Tools，aapt来自Build Tools。\r\n配置保存在程序目录下 config.json 文件中。";
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(380, 235);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 30);
            this.btnSave.TabIndex = 11;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(480, 235);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 30);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(594, 280);
            this.Controls.Add(this.lblAdbPath);
            this.Controls.Add(this.txtAdbPath);
            this.Controls.Add(this.btnBrowseAdb);
            this.Controls.Add(this.btnAutoDetectAdb);
            this.Controls.Add(this.lblAaptPath);
            this.Controls.Add(this.txtAaptPath);
            this.Controls.Add(this.btnBrowseAapt);
            this.Controls.Add(this.btnAutoDetectAapt);
            this.Controls.Add(this.lblExportPath);
            this.Controls.Add(this.txtExportPath);
            this.Controls.Add(this.btnBrowseExport);
            this.Controls.Add(this.btnResetExport);
            this.Controls.Add(this.grpStatus);
            this.Controls.Add(this.lblHint);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "设置 - 工具路径配置";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.grpStatus.ResumeLayout(false);
            this.grpStatus.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
