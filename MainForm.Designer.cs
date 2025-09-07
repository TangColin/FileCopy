using System;
using System.Drawing;
using System.Windows.Forms;

namespace FileCopyHelper
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBoxSource = new System.Windows.Forms.GroupBox();
            this.btnBrowseSource = new System.Windows.Forms.Button();
            this.txtSourcePath = new System.Windows.Forms.TextBox();
            this.groupBoxTargets = new System.Windows.Forms.GroupBox();
            this.txtTargetPaths = new System.Windows.Forms.TextBox();
            this.groupBoxBlacklist = new System.Windows.Forms.GroupBox();
            this.txtBlacklist = new System.Windows.Forms.TextBox();
            this.progressBarCopy = new System.Windows.Forms.ProgressBar();
            this.lblProgress = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnStartCopy = new System.Windows.Forms.Button();
            this.btnHelp = new System.Windows.Forms.Button();
            this.groupBoxConfig = new System.Windows.Forms.GroupBox();
            this.btnNewConfig = new System.Windows.Forms.Button();
            this.btnDeleteConfig = new System.Windows.Forms.Button();
            this.cmbConfigurations = new System.Windows.Forms.ComboBox();
            this.btnSaveConfig = new System.Windows.Forms.Button();
            this.lblConfig = new System.Windows.Forms.Label();
            this.groupBoxSource.SuspendLayout();
            this.groupBoxTargets.SuspendLayout();
            this.groupBoxBlacklist.SuspendLayout();
            this.groupBoxConfig.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxSource
            // 
            this.groupBoxSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxSource.Controls.Add(this.btnBrowseSource);
            this.groupBoxSource.Controls.Add(this.txtSourcePath);
            this.groupBoxSource.Location = new System.Drawing.Point(10, 8);
            this.groupBoxSource.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxSource.Name = "groupBoxSource";
            this.groupBoxSource.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxSource.Size = new System.Drawing.Size(480, 52);
            this.groupBoxSource.TabIndex = 0;
            this.groupBoxSource.TabStop = false;
            this.groupBoxSource.Text = "源文件夹（拖拽或浏览设置目录）";
            // 
            // btnBrowseSource
            // 
            this.btnBrowseSource.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseSource.Location = new System.Drawing.Point(403, 14);
            this.btnBrowseSource.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnBrowseSource.Name = "btnBrowseSource";
            this.btnBrowseSource.Size = new System.Drawing.Size(68, 30);
            this.btnBrowseSource.TabIndex = 2;
            this.btnBrowseSource.Text = "浏览...";
            this.btnBrowseSource.UseVisualStyleBackColor = true;
            this.btnBrowseSource.Click += new System.EventHandler(this.btnBrowseSource_Click_1);
            // 
            // txtSourcePath
            // 
            this.txtSourcePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSourcePath.BackColor = System.Drawing.Color.White;
            this.txtSourcePath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSourcePath.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSourcePath.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtSourcePath.Location = new System.Drawing.Point(13, 18);
            this.txtSourcePath.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtSourcePath.Name = "txtSourcePath";
            this.txtSourcePath.Size = new System.Drawing.Size(384, 23);
            this.txtSourcePath.TabIndex = 1;
            // 
            // groupBoxTargets
            // 
            this.groupBoxTargets.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxTargets.Controls.Add(this.txtTargetPaths);
            this.groupBoxTargets.Location = new System.Drawing.Point(10, 64);
            this.groupBoxTargets.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxTargets.Name = "groupBoxTargets";
            this.groupBoxTargets.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxTargets.Size = new System.Drawing.Size(480, 204);
            this.groupBoxTargets.TabIndex = 1;
            this.groupBoxTargets.TabStop = false;
            this.groupBoxTargets.Text = "目标路径（拖拽或输入路径，每行一个）";
            // 
            // txtTargetPaths
            // 
            this.txtTargetPaths.AcceptsReturn = true;
            this.txtTargetPaths.AllowDrop = true;
            this.txtTargetPaths.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTargetPaths.BackColor = System.Drawing.Color.White;
            this.txtTargetPaths.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTargetPaths.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTargetPaths.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtTargetPaths.Location = new System.Drawing.Point(13, 18);
            this.txtTargetPaths.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtTargetPaths.Multiline = true;
            this.txtTargetPaths.Name = "txtTargetPaths";
            this.txtTargetPaths.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtTargetPaths.Size = new System.Drawing.Size(459, 177);
            this.txtTargetPaths.TabIndex = 1;
            this.txtTargetPaths.WordWrap = false;
            // 
            // groupBoxBlacklist
            // 
            this.groupBoxBlacklist.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxBlacklist.Controls.Add(this.txtBlacklist);
            this.groupBoxBlacklist.Location = new System.Drawing.Point(10, 272);
            this.groupBoxBlacklist.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxBlacklist.Name = "groupBoxBlacklist";
            this.groupBoxBlacklist.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxBlacklist.Size = new System.Drawing.Size(480, 106);
            this.groupBoxBlacklist.TabIndex = 2;
            this.groupBoxBlacklist.TabStop = false;
            this.groupBoxBlacklist.Text = "黑名单设置（忽略源文件夹中的文件/文件夹，每行一个）";
            // 
            // txtBlacklist
            // 
            this.txtBlacklist.AcceptsReturn = true;
            this.txtBlacklist.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBlacklist.BackColor = System.Drawing.Color.White;
            this.txtBlacklist.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBlacklist.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBlacklist.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtBlacklist.Location = new System.Drawing.Point(13, 18);
            this.txtBlacklist.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtBlacklist.Multiline = true;
            this.txtBlacklist.Name = "txtBlacklist";
            this.txtBlacklist.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtBlacklist.Size = new System.Drawing.Size(458, 77);
            this.txtBlacklist.TabIndex = 1;
            this.txtBlacklist.WordWrap = false;
            // 
            // progressBarCopy
            // 
            this.progressBarCopy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarCopy.Location = new System.Drawing.Point(13, 85);
            this.progressBarCopy.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.progressBarCopy.Name = "progressBarCopy";
            this.progressBarCopy.Size = new System.Drawing.Size(458, 29);
            this.progressBarCopy.TabIndex = 1;
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Location = new System.Drawing.Point(16, 94);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(53, 12);
            this.lblProgress.TabIndex = 0;
            this.lblProgress.Text = "准备就绪";
            // 
            // btnCancel
            // 
            this.btnCancel.Enabled = false;
            this.btnCancel.Location = new System.Drawing.Point(362, 21);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(109, 27);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "取消复制";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnStartCopy
            // 
            this.btnStartCopy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.btnStartCopy.ForeColor = System.Drawing.Color.White;
            this.btnStartCopy.Location = new System.Drawing.Point(238, 21);
            this.btnStartCopy.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnStartCopy.Name = "btnStartCopy";
            this.btnStartCopy.Size = new System.Drawing.Size(118, 57);
            this.btnStartCopy.TabIndex = 0;
            this.btnStartCopy.Text = "开始复制";
            this.btnStartCopy.UseVisualStyleBackColor = false;
            // 
            // btnHelp
            // 
            this.btnHelp.Location = new System.Drawing.Point(362, 51);
            this.btnHelp.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(109, 27);
            this.btnHelp.TabIndex = 7;
            this.btnHelp.Text = "使用帮助";
            this.btnHelp.UseVisualStyleBackColor = true;
            this.btnHelp.Click += new System.EventHandler(this.BtnHelp_Click);
            // 
            // groupBoxConfig
            // 
            this.groupBoxConfig.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxConfig.Controls.Add(this.btnCancel);
            this.groupBoxConfig.Controls.Add(this.btnHelp);
            this.groupBoxConfig.Controls.Add(this.lblProgress);
            this.groupBoxConfig.Controls.Add(this.progressBarCopy);
            this.groupBoxConfig.Controls.Add(this.btnNewConfig);
            this.groupBoxConfig.Controls.Add(this.btnDeleteConfig);
            this.groupBoxConfig.Controls.Add(this.btnStartCopy);
            this.groupBoxConfig.Controls.Add(this.cmbConfigurations);
            this.groupBoxConfig.Controls.Add(this.btnSaveConfig);
            this.groupBoxConfig.Controls.Add(this.lblConfig);
            this.groupBoxConfig.Location = new System.Drawing.Point(10, 382);
            this.groupBoxConfig.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxConfig.Name = "groupBoxConfig";
            this.groupBoxConfig.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxConfig.Size = new System.Drawing.Size(480, 123);
            this.groupBoxConfig.TabIndex = 3;
            this.groupBoxConfig.TabStop = false;
            this.groupBoxConfig.Text = "配置管理";
            // 
            // btnNewConfig
            // 
            this.btnNewConfig.Location = new System.Drawing.Point(13, 50);
            this.btnNewConfig.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnNewConfig.Name = "btnNewConfig";
            this.btnNewConfig.Size = new System.Drawing.Size(64, 28);
            this.btnNewConfig.TabIndex = 5;
            this.btnNewConfig.Text = "新增";
            this.btnNewConfig.UseVisualStyleBackColor = true;
            this.btnNewConfig.Click += new System.EventHandler(this.BtnNewConfig_Click);
            // 
            // btnDeleteConfig
            // 
            this.btnDeleteConfig.Location = new System.Drawing.Point(153, 50);
            this.btnDeleteConfig.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnDeleteConfig.Name = "btnDeleteConfig";
            this.btnDeleteConfig.Size = new System.Drawing.Size(64, 28);
            this.btnDeleteConfig.TabIndex = 6;
            this.btnDeleteConfig.Text = "删除";
            this.btnDeleteConfig.UseVisualStyleBackColor = true;
            this.btnDeleteConfig.Click += new System.EventHandler(this.BtnDeleteConfig_Click);
            // 
            // cmbConfigurations
            // 
            this.cmbConfigurations.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbConfigurations.Location = new System.Drawing.Point(70, 21);
            this.cmbConfigurations.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbConfigurations.Name = "cmbConfigurations";
            this.cmbConfigurations.Size = new System.Drawing.Size(147, 20);
            this.cmbConfigurations.TabIndex = 1;
            this.cmbConfigurations.SelectedIndexChanged += new System.EventHandler(this.cmbConfigurations_SelectedIndexChanged);
            // 
            // btnSaveConfig
            // 
            this.btnSaveConfig.Location = new System.Drawing.Point(83, 50);
            this.btnSaveConfig.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSaveConfig.Name = "btnSaveConfig";
            this.btnSaveConfig.Size = new System.Drawing.Size(64, 28);
            this.btnSaveConfig.TabIndex = 3;
            this.btnSaveConfig.Text = "保存";
            this.btnSaveConfig.UseVisualStyleBackColor = true;
            // 
            // lblConfig
            // 
            this.lblConfig.AutoSize = true;
            this.lblConfig.Location = new System.Drawing.Point(11, 25);
            this.lblConfig.Name = "lblConfig";
            this.lblConfig.Size = new System.Drawing.Size(59, 12);
            this.lblConfig.TabIndex = 0;
            this.lblConfig.Text = "配置方案:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(501, 514);
            this.Controls.Add(this.groupBoxConfig);
            this.Controls.Add(this.groupBoxBlacklist);
            this.Controls.Add(this.groupBoxTargets);
            this.Controls.Add(this.groupBoxSource);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MinimumSize = new System.Drawing.Size(517, 223);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "文件复制助手 v2.0 作者：唐小布 无极交流群1016741666";
            this.groupBoxSource.ResumeLayout(false);
            this.groupBoxSource.PerformLayout();
            this.groupBoxTargets.ResumeLayout(false);
            this.groupBoxTargets.PerformLayout();
            this.groupBoxBlacklist.ResumeLayout(false);
            this.groupBoxBlacklist.PerformLayout();
            this.groupBoxConfig.ResumeLayout(false);
            this.groupBoxConfig.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox groupBoxSource;
        private Button btnBrowseSource;
        private TextBox txtSourcePath;

        private GroupBox groupBoxTargets;
        private TextBox txtTargetPaths;

        private GroupBox groupBoxBlacklist;

        private TextBox txtBlacklist;

        private ProgressBar progressBarCopy;
        private Label lblProgress;

        private Button btnCancel;
        private Button btnStartCopy;
        private Button btnHelp;

        private GroupBox groupBoxConfig;
        private Button btnSaveConfig;
        private Button btnNewConfig;
        private Button btnDeleteConfig;
        private ComboBox cmbConfigurations;
        private Label lblConfig;
    }
}
