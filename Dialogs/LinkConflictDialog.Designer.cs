namespace FileHelper.Dialogs
{
    partial class LinkConflictDialog
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
            this.lblMessage = new System.Windows.Forms.Label();
            this.lblTargetPath = new System.Windows.Forms.Label();
            this.btnReplace = new System.Windows.Forms.Button();
            this.btnSkip = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Location = new System.Drawing.Point(12, 9);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(233, 12);
            this.lblMessage.TabIndex = 0;
            this.lblMessage.Text = "目标路径已存在，请选择如何处理：";
            // 
            // lblTargetPath
            // 
            this.lblTargetPath.AutoEllipsis = true;
            this.lblTargetPath.Location = new System.Drawing.Point(12, 30);
            this.lblTargetPath.Name = "lblTargetPath";
            this.lblTargetPath.Size = new System.Drawing.Size(360, 30);
            this.lblTargetPath.TabIndex = 1;
            this.lblTargetPath.Text = "目标路径";
            // 
            // btnReplace
            // 
            this.btnReplace.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.btnReplace.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnReplace.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReplace.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnReplace.ForeColor = System.Drawing.Color.White;
            this.btnReplace.Location = new System.Drawing.Point(12, 70);
            this.btnReplace.Name = "btnReplace";
            this.btnReplace.Size = new System.Drawing.Size(100, 35);
            this.btnReplace.TabIndex = 2;
            this.btnReplace.Text = "替换";
            this.btnReplace.UseVisualStyleBackColor = false;
            this.btnReplace.Click += new System.EventHandler(this.btnReplace_Click);
            // 
            // btnSkip
            // 
            this.btnSkip.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSkip.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSkip.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSkip.Location = new System.Drawing.Point(141, 70);
            this.btnSkip.Name = "btnSkip";
            this.btnSkip.Size = new System.Drawing.Size(100, 35);
            this.btnSkip.TabIndex = 3;
            this.btnSkip.Text = "跳过";
            this.btnSkip.UseVisualStyleBackColor = true;
            this.btnSkip.Click += new System.EventHandler(this.btnSkip_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancel.Location = new System.Drawing.Point(272, 70);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 35);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // LinkConflictDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 117);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSkip);
            this.Controls.Add(this.btnReplace);
            this.Controls.Add(this.lblTargetPath);
            this.Controls.Add(this.lblMessage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LinkConflictDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "链接冲突处理";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Label lblTargetPath;
        private System.Windows.Forms.Button btnReplace;
        private System.Windows.Forms.Button btnSkip;
        private System.Windows.Forms.Button btnCancel;
    }
}