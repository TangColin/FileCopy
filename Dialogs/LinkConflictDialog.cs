using System;
using System.Windows.Forms;

namespace FileHelper.Dialogs
{
    public partial class LinkConflictDialog : Form
    {
        public enum ConflictAction
        {
            Replace,
            Skip,
            Cancel
        }

        public ConflictAction Action { get; private set; } = ConflictAction.Cancel;

        public LinkConflictDialog(string targetPath)
        {
            InitializeComponent();
            
            // 设置目标路径显示
            lblTargetPath.Text = targetPath;
            
            // 设置默认焦点
            btnReplace.Focus();
        }

        private void btnReplace_Click(object sender, EventArgs e)
        {
            Action = ConflictAction.Replace;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnSkip_Click(object sender, EventArgs e)
        {
            Action = ConflictAction.Skip;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Action = ConflictAction.Cancel;
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}