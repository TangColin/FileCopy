using System;
using System.Linq;
using System.Windows.Forms;

namespace FileCopyHelper
{
    public partial class SaveConfigDialog : Form
    {
        public string ConfigName { get; private set; } = string.Empty;

        public SaveConfigDialog(string defaultName = "")
        {
            InitializeComponent();
            
            if (!string.IsNullOrWhiteSpace(defaultName))
            {
                txtConfigName.Text = defaultName;
                txtConfigName.SelectAll();
            }
            else
            {
                txtConfigName.Text = $"配置方案_{DateTime.Now:yyyyMMdd_HHmmss}";
                txtConfigName.SelectAll();
            }

            SetupEvents();
        }

        private void SetupEvents()
        {
            btnOK.Click += BtnOK_Click;
            btnCancel.Click += BtnCancel_Click;
            
            txtConfigName.KeyDown += (s, e) => {
                if (e.KeyCode == Keys.Enter)
                {
                    BtnOK_Click(s, e);
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    BtnCancel_Click(s, e);
                }
            };

            txtConfigName.TextChanged += (s, e) => {
                btnOK.Enabled = !string.IsNullOrWhiteSpace(txtConfigName.Text.Trim());
            };

            this.Load += (s, e) => {
                txtConfigName.Focus();
                txtConfigName.SelectAll();
            };
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            string configName = txtConfigName.Text.Trim();
            
            if (string.IsNullOrWhiteSpace(configName))
            {
                MessageBox.Show("请输入配置方案名称", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtConfigName.Focus();
                return;
            }

            // 检查名称长度
            if (configName.Length > 100)
            {
                MessageBox.Show("配置方案名称不能超过100个字符", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtConfigName.Focus();
                return;
            }

            // 检查是否包含无效字符
            char[] invalidChars = System.IO.Path.GetInvalidFileNameChars();
            foreach (char invalidChar in invalidChars)
            {
                if (configName.Contains(invalidChar))
                {
                    MessageBox.Show($"配置方案名称不能包含以下字符: {string.Join(" ", invalidChars.Select(c => c.ToString()).ToArray())}", 
                        "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtConfigName.Focus();
                    return;
                }
            }

            ConfigName = configName;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}