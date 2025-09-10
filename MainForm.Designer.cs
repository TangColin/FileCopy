using System;
using System.Drawing;
using System.Windows.Forms;

namespace FileHelper
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
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageCopy = new System.Windows.Forms.TabPage();
            this.groupBoxSource = new System.Windows.Forms.GroupBox();
            this.btnBrowseSource = new System.Windows.Forms.Button();
            this.txtSourcePath = new System.Windows.Forms.TextBox();
            this.groupBoxTargets = new System.Windows.Forms.GroupBox();
            this.txtTargetPaths = new System.Windows.Forms.TextBox();
            this.groupBoxBlacklist = new System.Windows.Forms.GroupBox();
            this.txtBlacklist = new System.Windows.Forms.TextBox();
            this.groupBoxConfig = new System.Windows.Forms.GroupBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnHelp = new System.Windows.Forms.Button();
            this.lblProgress = new System.Windows.Forms.Label();
            this.progressBarCopy = new System.Windows.Forms.ProgressBar();
            this.btnNewConfig = new System.Windows.Forms.Button();
            this.btnDeleteConfig = new System.Windows.Forms.Button();
            this.btnStartCopy = new System.Windows.Forms.Button();
            this.cmbConfigurations = new System.Windows.Forms.ComboBox();
            this.btnSaveConfig = new System.Windows.Forms.Button();
            this.lblConfig = new System.Windows.Forms.Label();
            this.tabPageLink = new System.Windows.Forms.TabPage();
            this.groupBoxLinkType = new System.Windows.Forms.GroupBox();
            this.radioHardLink = new System.Windows.Forms.RadioButton();
            this.radioSymbolic = new System.Windows.Forms.RadioButton();
            this.radioJunction = new System.Windows.Forms.RadioButton();
            this.groupBoxLinkSource = new System.Windows.Forms.GroupBox();
            this.btnBrowseLinkSource = new System.Windows.Forms.Button();
            this.txtLinkSourcePath = new System.Windows.Forms.TextBox();
            this.groupBoxLinkTargets = new System.Windows.Forms.GroupBox();
            this.txtLinkTargetPaths = new System.Windows.Forms.TextBox();
            this.groupBoxLinkActions = new System.Windows.Forms.GroupBox();
            this.btnLinkHelp = new System.Windows.Forms.Button();
            this.lblLinkProgress = new System.Windows.Forms.Label();
            this.progressBarLink = new System.Windows.Forms.ProgressBar();
            this.btnDeleteLink = new System.Windows.Forms.Button();
            this.btnValidateLink = new System.Windows.Forms.Button();
            this.btnCreateLink = new System.Windows.Forms.Button();
            this.cmbLinkConfigurations = new System.Windows.Forms.ComboBox();
            this.btnLinkDeleteConfig = new System.Windows.Forms.Button();
            this.btnLinkNewConfig = new System.Windows.Forms.Button();
            this.btnLinkSaveConfig = new System.Windows.Forms.Button();
            this.lblLinkConfig = new System.Windows.Forms.Label();
            this.tabControlMain.SuspendLayout();
            this.tabPageCopy.SuspendLayout();
            this.groupBoxSource.SuspendLayout();
            this.groupBoxTargets.SuspendLayout();
            this.groupBoxBlacklist.SuspendLayout();
            this.groupBoxConfig.SuspendLayout();
            this.tabPageLink.SuspendLayout();
            this.groupBoxLinkType.SuspendLayout();
            this.groupBoxLinkSource.SuspendLayout();
            this.groupBoxLinkTargets.SuspendLayout();
            this.groupBoxLinkActions.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPageCopy);
            this.tabControlMain.Controls.Add(this.tabPageLink);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.Location = new System.Drawing.Point(0, 0);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(517, 541);
            this.tabControlMain.TabIndex = 0;
            // 
            // tabPageCopy
            // 
            this.tabPageCopy.Controls.Add(this.groupBoxSource);
            this.tabPageCopy.Controls.Add(this.groupBoxTargets);
            this.tabPageCopy.Controls.Add(this.groupBoxBlacklist);
            this.tabPageCopy.Controls.Add(this.groupBoxConfig);
            this.tabPageCopy.Location = new System.Drawing.Point(4, 22);
            this.tabPageCopy.Name = "tabPageCopy";
            this.tabPageCopy.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCopy.Size = new System.Drawing.Size(509, 515);
            this.tabPageCopy.TabIndex = 0;
            this.tabPageCopy.Text = "文件复制";
            this.tabPageCopy.UseVisualStyleBackColor = true;
            // 
            // groupBoxSource
            // 
            this.groupBoxSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxSource.Controls.Add(this.btnBrowseSource);
            this.groupBoxSource.Controls.Add(this.txtSourcePath);
            this.groupBoxSource.Location = new System.Drawing.Point(14, 8);
            this.groupBoxSource.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxSource.Name = "groupBoxSource";
            this.groupBoxSource.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxSource.Size = new System.Drawing.Size(480, 52);
            this.groupBoxSource.TabIndex = 0;
            this.groupBoxSource.TabStop = false;
            this.groupBoxSource.Text = "源文件夹（拖拽或浏览设置路径）";
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
            this.txtSourcePath.TextChanged += new System.EventHandler(this.txtSourcePath_TextChanged);
            // 
            // groupBoxTargets
            // 
            this.groupBoxTargets.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxTargets.Controls.Add(this.txtTargetPaths);
            this.groupBoxTargets.Location = new System.Drawing.Point(14, 64);
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
            this.txtTargetPaths.Size = new System.Drawing.Size(459, 170);
            this.txtTargetPaths.TabIndex = 1;
            this.txtTargetPaths.WordWrap = false;
            // 
            // groupBoxBlacklist
            // 
            this.groupBoxBlacklist.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxBlacklist.Controls.Add(this.txtBlacklist);
            this.groupBoxBlacklist.Location = new System.Drawing.Point(14, 272);
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
            this.txtBlacklist.Size = new System.Drawing.Size(458, 74);
            this.txtBlacklist.TabIndex = 1;
            this.txtBlacklist.WordWrap = false;
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
            this.groupBoxConfig.Location = new System.Drawing.Point(14, 382);
            this.groupBoxConfig.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxConfig.Name = "groupBoxConfig";
            this.groupBoxConfig.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxConfig.Size = new System.Drawing.Size(480, 123);
            this.groupBoxConfig.TabIndex = 3;
            this.groupBoxConfig.TabStop = false;
            this.groupBoxConfig.Text = "操作面板";
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
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Location = new System.Drawing.Point(16, 94);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(53, 12);
            this.lblProgress.TabIndex = 0;
            this.lblProgress.Text = "准备就绪";
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
            // btnStartCopy
            // 
            this.btnStartCopy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.btnStartCopy.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartCopy.ForeColor = System.Drawing.Color.White;
            this.btnStartCopy.Location = new System.Drawing.Point(238, 21);
            this.btnStartCopy.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnStartCopy.Name = "btnStartCopy";
            this.btnStartCopy.Size = new System.Drawing.Size(118, 57);
            this.btnStartCopy.TabIndex = 0;
            this.btnStartCopy.Text = "开始复制";
            this.btnStartCopy.UseVisualStyleBackColor = false;
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
            // tabPageLink
            // 
            this.tabPageLink.Controls.Add(this.groupBoxLinkType);
            this.tabPageLink.Controls.Add(this.groupBoxLinkSource);
            this.tabPageLink.Controls.Add(this.groupBoxLinkTargets);
            this.tabPageLink.Controls.Add(this.groupBoxLinkActions);
            this.tabPageLink.Location = new System.Drawing.Point(4, 22);
            this.tabPageLink.Name = "tabPageLink";
            this.tabPageLink.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageLink.Size = new System.Drawing.Size(509, 515);
            this.tabPageLink.TabIndex = 1;
            this.tabPageLink.Text = "文件链接";
            this.tabPageLink.UseVisualStyleBackColor = true;
            // 
            // groupBoxLinkType
            // 
            this.groupBoxLinkType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxLinkType.Controls.Add(this.radioHardLink);
            this.groupBoxLinkType.Controls.Add(this.radioSymbolic);
            this.groupBoxLinkType.Controls.Add(this.radioJunction);
            this.groupBoxLinkType.Location = new System.Drawing.Point(14, 64);
            this.groupBoxLinkType.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxLinkType.Name = "groupBoxLinkType";
            this.groupBoxLinkType.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxLinkType.Size = new System.Drawing.Size(480, 52);
            this.groupBoxLinkType.TabIndex = 0;
            this.groupBoxLinkType.TabStop = false;
            this.groupBoxLinkType.Text = "链接类型";
            // 
            // radioHardLink
            // 
            this.radioHardLink.AutoSize = true;
            this.radioHardLink.Location = new System.Drawing.Point(352, 23);
            this.radioHardLink.Name = "radioHardLink";
            this.radioHardLink.Size = new System.Drawing.Size(119, 16);
            this.radioHardLink.TabIndex = 2;
            this.radioHardLink.Text = "硬链接（仅文件）";
            this.radioHardLink.UseVisualStyleBackColor = true;
            // 
            // radioSymbolic
            // 
            this.radioSymbolic.AutoSize = true;
            this.radioSymbolic.Location = new System.Drawing.Point(170, 23);
            this.radioSymbolic.Name = "radioSymbolic";
            this.radioSymbolic.Size = new System.Drawing.Size(155, 16);
            this.radioSymbolic.TabIndex = 1;
            this.radioSymbolic.Text = "符号链接（文件或目录）";
            this.radioSymbolic.UseVisualStyleBackColor = true;
            // 
            // radioJunction
            // 
            this.radioJunction.AutoSize = true;
            this.radioJunction.Checked = true;
            this.radioJunction.Location = new System.Drawing.Point(13, 23);
            this.radioJunction.Name = "radioJunction";
            this.radioJunction.Size = new System.Drawing.Size(131, 16);
            this.radioJunction.TabIndex = 0;
            this.radioJunction.TabStop = true;
            this.radioJunction.Text = "目录链接（仅目录）";
            this.radioJunction.UseVisualStyleBackColor = true;
            // 
            // groupBoxLinkSource
            // 
            this.groupBoxLinkSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxLinkSource.Controls.Add(this.btnBrowseLinkSource);
            this.groupBoxLinkSource.Controls.Add(this.txtLinkSourcePath);
            this.groupBoxLinkSource.Location = new System.Drawing.Point(14, 8);
            this.groupBoxLinkSource.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxLinkSource.Name = "groupBoxLinkSource";
            this.groupBoxLinkSource.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxLinkSource.Size = new System.Drawing.Size(480, 52);
            this.groupBoxLinkSource.TabIndex = 1;
            this.groupBoxLinkSource.TabStop = false;
            this.groupBoxLinkSource.Text = "源文件或文件夹（拖拽或浏览设置路径）";
            // 
            // btnBrowseLinkSource
            // 
            this.btnBrowseLinkSource.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseLinkSource.Location = new System.Drawing.Point(403, 14);
            this.btnBrowseLinkSource.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnBrowseLinkSource.Name = "btnBrowseLinkSource";
            this.btnBrowseLinkSource.Size = new System.Drawing.Size(68, 30);
            this.btnBrowseLinkSource.TabIndex = 2;
            this.btnBrowseLinkSource.Text = "浏览...";
            this.btnBrowseLinkSource.UseVisualStyleBackColor = true;
            // 
            // txtLinkSourcePath
            // 
            this.txtLinkSourcePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLinkSourcePath.BackColor = System.Drawing.Color.White;
            this.txtLinkSourcePath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLinkSourcePath.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLinkSourcePath.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtLinkSourcePath.Location = new System.Drawing.Point(13, 18);
            this.txtLinkSourcePath.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtLinkSourcePath.Name = "txtLinkSourcePath";
            this.txtLinkSourcePath.Size = new System.Drawing.Size(384, 23);
            this.txtLinkSourcePath.TabIndex = 1;
            // 
            // groupBoxLinkTargets
            // 
            this.groupBoxLinkTargets.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxLinkTargets.Controls.Add(this.txtLinkTargetPaths);
            this.groupBoxLinkTargets.Location = new System.Drawing.Point(14, 120);
            this.groupBoxLinkTargets.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxLinkTargets.Name = "groupBoxLinkTargets";
            this.groupBoxLinkTargets.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxLinkTargets.Size = new System.Drawing.Size(480, 258);
            this.groupBoxLinkTargets.TabIndex = 2;
            this.groupBoxLinkTargets.TabStop = false;
            this.groupBoxLinkTargets.Text = "目标路径（拖拽或输入路径，每行一个）";
            // 
            // txtLinkTargetPaths
            // 
            this.txtLinkTargetPaths.AcceptsReturn = true;
            this.txtLinkTargetPaths.AllowDrop = true;
            this.txtLinkTargetPaths.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLinkTargetPaths.BackColor = System.Drawing.Color.White;
            this.txtLinkTargetPaths.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLinkTargetPaths.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLinkTargetPaths.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtLinkTargetPaths.Location = new System.Drawing.Point(13, 18);
            this.txtLinkTargetPaths.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtLinkTargetPaths.Multiline = true;
            this.txtLinkTargetPaths.Name = "txtLinkTargetPaths";
            this.txtLinkTargetPaths.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLinkTargetPaths.Size = new System.Drawing.Size(459, 226);
            this.txtLinkTargetPaths.TabIndex = 1;
            this.txtLinkTargetPaths.WordWrap = false;
            // 
            // groupBoxLinkActions
            // 
            this.groupBoxLinkActions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxLinkActions.Controls.Add(this.btnLinkHelp);
            this.groupBoxLinkActions.Controls.Add(this.lblLinkProgress);
            this.groupBoxLinkActions.Controls.Add(this.progressBarLink);
            this.groupBoxLinkActions.Controls.Add(this.btnDeleteLink);
            this.groupBoxLinkActions.Controls.Add(this.btnValidateLink);
            this.groupBoxLinkActions.Controls.Add(this.btnCreateLink);
            this.groupBoxLinkActions.Controls.Add(this.cmbLinkConfigurations);
            this.groupBoxLinkActions.Controls.Add(this.btnLinkDeleteConfig);
            this.groupBoxLinkActions.Controls.Add(this.btnLinkNewConfig);
            this.groupBoxLinkActions.Controls.Add(this.btnLinkSaveConfig);
            this.groupBoxLinkActions.Controls.Add(this.lblLinkConfig);
            this.groupBoxLinkActions.Location = new System.Drawing.Point(14, 382);
            this.groupBoxLinkActions.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxLinkActions.Name = "groupBoxLinkActions";
            this.groupBoxLinkActions.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxLinkActions.Size = new System.Drawing.Size(480, 122);
            this.groupBoxLinkActions.TabIndex = 3;
            this.groupBoxLinkActions.TabStop = false;
            this.groupBoxLinkActions.Text = "操作面板";
            // 
            // btnLinkHelp
            // 
            this.btnLinkHelp.Location = new System.Drawing.Point(420, 51);
            this.btnLinkHelp.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnLinkHelp.Name = "btnLinkHelp";
            this.btnLinkHelp.Size = new System.Drawing.Size(51, 27);
            this.btnLinkHelp.TabIndex = 10;
            this.btnLinkHelp.Text = "帮助";
            this.btnLinkHelp.UseVisualStyleBackColor = true;
            // 
            // lblLinkProgress
            // 
            this.lblLinkProgress.AutoSize = true;
            this.lblLinkProgress.Location = new System.Drawing.Point(16, 94);
            this.lblLinkProgress.Name = "lblLinkProgress";
            this.lblLinkProgress.Size = new System.Drawing.Size(53, 12);
            this.lblLinkProgress.TabIndex = 0;
            this.lblLinkProgress.Text = "准备就绪";
            // 
            // progressBarLink
            // 
            this.progressBarLink.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarLink.Location = new System.Drawing.Point(13, 85);
            this.progressBarLink.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.progressBarLink.Name = "progressBarLink";
            this.progressBarLink.Size = new System.Drawing.Size(458, 29);
            this.progressBarLink.TabIndex = 1;
            // 
            // btnDeleteLink
            // 
            this.btnDeleteLink.Location = new System.Drawing.Point(362, 21);
            this.btnDeleteLink.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnDeleteLink.Name = "btnDeleteLink";
            this.btnDeleteLink.Size = new System.Drawing.Size(109, 27);
            this.btnDeleteLink.TabIndex = 9;
            this.btnDeleteLink.Text = "删除链接";
            this.btnDeleteLink.UseVisualStyleBackColor = true;
            // 
            // btnValidateLink
            // 
            this.btnValidateLink.Location = new System.Drawing.Point(362, 51);
            this.btnValidateLink.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnValidateLink.Name = "btnValidateLink";
            this.btnValidateLink.Size = new System.Drawing.Size(51, 27);
            this.btnValidateLink.TabIndex = 8;
            this.btnValidateLink.Text = "验证";
            this.btnValidateLink.UseVisualStyleBackColor = true;
            this.btnValidateLink.Click += new System.EventHandler(this.btnValidateLink_Click_1);
            // 
            // btnCreateLink
            // 
            this.btnCreateLink.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.btnCreateLink.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCreateLink.ForeColor = System.Drawing.Color.White;
            this.btnCreateLink.Location = new System.Drawing.Point(238, 21);
            this.btnCreateLink.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCreateLink.Name = "btnCreateLink";
            this.btnCreateLink.Size = new System.Drawing.Size(118, 57);
            this.btnCreateLink.TabIndex = 7;
            this.btnCreateLink.Text = "创建链接";
            this.btnCreateLink.UseVisualStyleBackColor = false;
            // 
            // cmbLinkConfigurations
            // 
            this.cmbLinkConfigurations.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLinkConfigurations.Location = new System.Drawing.Point(70, 21);
            this.cmbLinkConfigurations.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbLinkConfigurations.Name = "cmbLinkConfigurations";
            this.cmbLinkConfigurations.Size = new System.Drawing.Size(147, 20);
            this.cmbLinkConfigurations.TabIndex = 1;
            // 
            // btnLinkDeleteConfig
            // 
            this.btnLinkDeleteConfig.Location = new System.Drawing.Point(153, 50);
            this.btnLinkDeleteConfig.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnLinkDeleteConfig.Name = "btnLinkDeleteConfig";
            this.btnLinkDeleteConfig.Size = new System.Drawing.Size(64, 28);
            this.btnLinkDeleteConfig.TabIndex = 6;
            this.btnLinkDeleteConfig.Text = "删除";
            this.btnLinkDeleteConfig.UseVisualStyleBackColor = true;
            // 
            // btnLinkNewConfig
            // 
            this.btnLinkNewConfig.Location = new System.Drawing.Point(13, 50);
            this.btnLinkNewConfig.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnLinkNewConfig.Name = "btnLinkNewConfig";
            this.btnLinkNewConfig.Size = new System.Drawing.Size(64, 28);
            this.btnLinkNewConfig.TabIndex = 5;
            this.btnLinkNewConfig.Text = "新增";
            this.btnLinkNewConfig.UseVisualStyleBackColor = true;
            // 
            // btnLinkSaveConfig
            // 
            this.btnLinkSaveConfig.Location = new System.Drawing.Point(83, 50);
            this.btnLinkSaveConfig.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnLinkSaveConfig.Name = "btnLinkSaveConfig";
            this.btnLinkSaveConfig.Size = new System.Drawing.Size(64, 28);
            this.btnLinkSaveConfig.TabIndex = 3;
            this.btnLinkSaveConfig.Text = "保存";
            this.btnLinkSaveConfig.UseVisualStyleBackColor = true;
            // 
            // lblLinkConfig
            // 
            this.lblLinkConfig.AutoSize = true;
            this.lblLinkConfig.Location = new System.Drawing.Point(11, 25);
            this.lblLinkConfig.Name = "lblLinkConfig";
            this.lblLinkConfig.Size = new System.Drawing.Size(59, 12);
            this.lblLinkConfig.TabIndex = 0;
            this.lblLinkConfig.Text = "配置方案:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(517, 541);
            this.Controls.Add(this.tabControlMain);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MinimumSize = new System.Drawing.Size(533, 580);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "无极文件助手 v3.0 作者：唐小布 无极交流群1016741666";
            this.tabControlMain.ResumeLayout(false);
            this.tabPageCopy.ResumeLayout(false);
            this.groupBoxSource.ResumeLayout(false);
            this.groupBoxSource.PerformLayout();
            this.groupBoxTargets.ResumeLayout(false);
            this.groupBoxTargets.PerformLayout();
            this.groupBoxBlacklist.ResumeLayout(false);
            this.groupBoxBlacklist.PerformLayout();
            this.groupBoxConfig.ResumeLayout(false);
            this.groupBoxConfig.PerformLayout();
            this.tabPageLink.ResumeLayout(false);
            this.groupBoxLinkType.ResumeLayout(false);
            this.groupBoxLinkType.PerformLayout();
            this.groupBoxLinkSource.ResumeLayout(false);
            this.groupBoxLinkSource.PerformLayout();
            this.groupBoxLinkTargets.ResumeLayout(false);
            this.groupBoxLinkTargets.PerformLayout();
            this.groupBoxLinkActions.ResumeLayout(false);
            this.groupBoxLinkActions.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TabControl tabControlMain;
        private TabPage tabPageCopy;
        private TabPage tabPageLink;
        
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
        
        // 文件链接相关控件
        private GroupBox groupBoxLinkType;
        private RadioButton radioJunction;
        private RadioButton radioSymbolic;
        private RadioButton radioHardLink;
        
        private GroupBox groupBoxLinkSource;
        private TextBox txtLinkSourcePath;
        private Button btnBrowseLinkSource;
        
        private GroupBox groupBoxLinkTargets;
        private TextBox txtLinkTargetPaths;
        
        private GroupBox groupBoxLinkActions;
        private Button btnCreateLink;
        private Button btnValidateLink;
        private Button btnDeleteLink;
        private Button btnLinkHelp;
        private ProgressBar progressBarLink;
        private Label lblLinkProgress;
        
        // 文件链接配置管理
        private ComboBox cmbLinkConfigurations;
        private Button btnLinkSaveConfig;
        private Button btnLinkNewConfig;
        private Button btnLinkDeleteConfig;
        private Label lblLinkConfig;
    }
}
