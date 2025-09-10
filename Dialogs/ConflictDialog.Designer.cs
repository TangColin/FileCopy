using System;
using System.Drawing;
using System.Windows.Forms;

namespace FileHelper
{
    partial class ConflictDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.groupBoxSource = new System.Windows.Forms.GroupBox();
            this.lblSourceDate = new System.Windows.Forms.Label();
            this.lblSourceSize = new System.Windows.Forms.Label();
            this.lblSourcePath = new System.Windows.Forms.Label();
            this.groupBoxTarget = new System.Windows.Forms.GroupBox();
            this.lblTargetDate = new System.Windows.Forms.Label();
            this.lblTargetSize = new System.Windows.Forms.Label();
            this.lblTargetPath = new System.Windows.Forms.Label();
            this.btnReplace = new System.Windows.Forms.Button();
            this.btnSkip = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBoxSource.SuspendLayout();
            this.groupBoxTarget.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(60, 11);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(386, 18);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "检测到文件冲突";
            // 
            // groupBoxSource
            // 
            this.groupBoxSource.Controls.Add(this.lblSourceDate);
            this.groupBoxSource.Controls.Add(this.lblSourceSize);
            this.groupBoxSource.Controls.Add(this.lblSourcePath);
            this.groupBoxSource.Location = new System.Drawing.Point(13, 49);
            this.groupBoxSource.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxSource.Name = "groupBoxSource";
            this.groupBoxSource.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxSource.Size = new System.Drawing.Size(446, 71);
            this.groupBoxSource.TabIndex = 2;
            this.groupBoxSource.TabStop = false;
            this.groupBoxSource.Text = "源文件";
            // 
            // lblSourceDate
            // 
            this.lblSourceDate.Location = new System.Drawing.Point(9, 53);
            this.lblSourceDate.Name = "lblSourceDate";
            this.lblSourceDate.Size = new System.Drawing.Size(257, 14);
            this.lblSourceDate.TabIndex = 2;
            this.lblSourceDate.Text = "修改时间: ";
            // 
            // lblSourceSize
            // 
            this.lblSourceSize.Location = new System.Drawing.Point(9, 35);
            this.lblSourceSize.Name = "lblSourceSize";
            this.lblSourceSize.Size = new System.Drawing.Size(171, 14);
            this.lblSourceSize.TabIndex = 1;
            this.lblSourceSize.Text = "大小: ";
            // 
            // lblSourcePath
            // 
            this.lblSourcePath.AutoEllipsis = true;
            this.lblSourcePath.Location = new System.Drawing.Point(9, 18);
            this.lblSourcePath.Name = "lblSourcePath";
            this.lblSourcePath.Size = new System.Drawing.Size(429, 14);
            this.lblSourcePath.TabIndex = 0;
            this.lblSourcePath.Text = "路径: ";
            // 
            // groupBoxTarget
            // 
            this.groupBoxTarget.Controls.Add(this.lblTargetDate);
            this.groupBoxTarget.Controls.Add(this.lblTargetSize);
            this.groupBoxTarget.Controls.Add(this.lblTargetPath);
            this.groupBoxTarget.Location = new System.Drawing.Point(13, 127);
            this.groupBoxTarget.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxTarget.Name = "groupBoxTarget";
            this.groupBoxTarget.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxTarget.Size = new System.Drawing.Size(446, 71);
            this.groupBoxTarget.TabIndex = 3;
            this.groupBoxTarget.TabStop = false;
            this.groupBoxTarget.Text = "目标文件 (已存在)";
            // 
            // lblTargetDate
            // 
            this.lblTargetDate.Location = new System.Drawing.Point(9, 53);
            this.lblTargetDate.Name = "lblTargetDate";
            this.lblTargetDate.Size = new System.Drawing.Size(257, 14);
            this.lblTargetDate.TabIndex = 2;
            this.lblTargetDate.Text = "修改时间: ";
            // 
            // lblTargetSize
            // 
            this.lblTargetSize.Location = new System.Drawing.Point(9, 35);
            this.lblTargetSize.Name = "lblTargetSize";
            this.lblTargetSize.Size = new System.Drawing.Size(171, 14);
            this.lblTargetSize.TabIndex = 1;
            this.lblTargetSize.Text = "大小: ";
            // 
            // lblTargetPath
            // 
            this.lblTargetPath.AutoEllipsis = true;
            this.lblTargetPath.Location = new System.Drawing.Point(9, 18);
            this.lblTargetPath.Name = "lblTargetPath";
            this.lblTargetPath.Size = new System.Drawing.Size(429, 14);
            this.lblTargetPath.TabIndex = 0;
            this.lblTargetPath.Text = "路径: ";
            // 
            // btnReplace
            // 
            this.btnReplace.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.btnReplace.ForeColor = System.Drawing.Color.White;
            this.btnReplace.Location = new System.Drawing.Point(46, 208);
            this.btnReplace.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnReplace.Name = "btnReplace";
            this.btnReplace.Size = new System.Drawing.Size(106, 35);
            this.btnReplace.TabIndex = 4;
            this.btnReplace.Text = "替换";
            this.btnReplace.UseVisualStyleBackColor = false;
            // 
            // btnSkip
            // 
            this.btnSkip.Location = new System.Drawing.Point(174, 208);
            this.btnSkip.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSkip.Name = "btnSkip";
            this.btnSkip.Size = new System.Drawing.Size(106, 35);
            this.btnSkip.TabIndex = 5;
            this.btnSkip.Text = "跳过";
            this.btnSkip.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(304, 208);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(106, 35);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(13, 7);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(41, 34);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // ConflictDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(471, 261);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSkip);
            this.Controls.Add(this.btnReplace);
            this.Controls.Add(this.groupBoxTarget);
            this.Controls.Add(this.groupBoxSource);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConflictDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "文件冲突";
            this.TopMost = true;
            this.groupBoxSource.ResumeLayout(false);
            this.groupBoxTarget.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Label lblTitle;
        private GroupBox groupBoxSource;
        private Label lblSourceDate;
        private Label lblSourceSize;
        private Label lblSourcePath;
        private GroupBox groupBoxTarget;
        private Label lblTargetDate;
        private Label lblTargetSize;
        private Label lblTargetPath;
        private Button btnReplace;
        private Button btnSkip;
        private Button btnCancel;
        private PictureBox pictureBox1;
    }
}