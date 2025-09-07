using FileCopyHelper.Models;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace FileCopyHelper
{
    public partial class ConflictDialog : Form
    {
        public ConflictAction SelectedAction { get; private set; } = ConflictAction.Cancel;

        public ConflictDialog(string sourceFile, string targetFile)
        {
            InitializeComponent();
            LoadFileInformation(sourceFile, targetFile);
            SetupEvents();
            SetupIcon();
        }

        private void LoadFileInformation(string sourceFile, string targetFile)
        {
            try
            {
                var sourceInfo = new FileInfo(sourceFile);
                var targetInfo = new FileInfo(targetFile);

                // 源文件信息
                lblSourcePath.Text = $"路径: {sourceFile}";
                lblSourceSize.Text = $"大小: {FormatFileSize(sourceInfo.Length)}";
                lblSourceDate.Text = $"修改时间: {sourceInfo.LastWriteTime:yyyy-MM-dd HH:mm:ss}";

                // 目标文件信息
                lblTargetPath.Text = $"路径: {targetFile}";
                lblTargetSize.Text = $"大小: {FormatFileSize(targetInfo.Length)}";
                lblTargetDate.Text = $"修改时间: {targetInfo.LastWriteTime:yyyy-MM-dd HH:mm:ss}";

                // 设置标题信息
                string fileName = Path.GetFileName(sourceFile);
                lblTitle.Text = $"文件 \"{fileName}\" 已存在，是否要替换？";
            }
            catch (Exception ex)
            {
                lblTitle.Text = "获取文件信息时出错";
                lblSourcePath.Text = $"错误: {ex.Message}";
            }
        }

        private void SetupEvents()
        {
            btnReplace.Click += (s, e) => {
                SelectedAction = ConflictAction.Replace;
                DialogResult = DialogResult.OK;
                Close();
            };

            btnSkip.Click += (s, e) => {
                SelectedAction = ConflictAction.Skip;
                DialogResult = DialogResult.OK;
                Close();
            };

            btnCancel.Click += (s, e) => {
                SelectedAction = ConflictAction.Cancel;
                DialogResult = DialogResult.Cancel;
                Close();
            };

            // 键盘快捷键
            this.KeyPreview = true;
            this.KeyDown += (s, e) => {
                switch (e.KeyCode)
                {
                    case Keys.R:
                        btnReplace.PerformClick();
                        break;
                    case Keys.S:
                        btnSkip.PerformClick();
                        break;
                    case Keys.Escape:
                        btnCancel.PerformClick();
                        break;
                }
            };
        }

        private void SetupIcon()
        {
            try
            {
                // 设置警告图标
                pictureBox1.Image = SystemIcons.Warning.ToBitmap();
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            catch
            {
                // 如果无法加载图标，隐藏PictureBox
                pictureBox1.Visible = false;
                lblTitle.Location = new Point(15, lblTitle.Location.Y);
            }
        }

        private string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }
    }
}