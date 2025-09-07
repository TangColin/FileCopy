using FileCopyHelper.Models;
using FileCopyHelper.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileCopyHelper
{
    public partial class MainForm : Form
    {
        private readonly ConfigurationService _configService;
        private readonly FileCopyService _copyService;
        private CancellationTokenSource _cancellationTokenSource;
        private bool _isCopying = false;

        public MainForm()
        {
            InitializeComponent();
            // 设置程序图标（从嵌入资源加载）
            this.Icon = new Icon(typeof(MainForm).Assembly.GetManifestResourceStream("FileCopyHelper.FileCopy.ico"));
            _configService = new ConfigurationService();
            _copyService = new FileCopyService();

            InitializeForm();
            LoadConfigurations();
            
            // 自动加载上次使用的配置方案
            LoadLastUsedConfiguration();
        }
        
        private void LoadLastUsedConfiguration()
        {
            try
            {
                var allConfigs = _configService.GetAllConfigurations();
                if (allConfigs != null && allConfigs.Count > 0)
                {
                    // 获取最近修改的配置（GetAllConfigurations已按LastModified降序排列）
                    var lastUsedConfig = allConfigs.FirstOrDefault();
                    if (lastUsedConfig != null)
                    {
                        LoadConfiguration(lastUsedConfig);
                        // 选择对应的配置项
                        cmbConfigurations.SelectedItem = lastUsedConfig.Name;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"自动加载最近配置失败: {ex.Message}");
            }
        }

        private void InitializeForm()
        {
            // 设置拖拽功能
            this.AllowDrop = true;
            txtSourcePath.AllowDrop = true;
            txtTargetPaths.AllowDrop = true;
            txtBlacklist.AllowDrop = true;

            // 绑定事件
            this.DragEnter += MainForm_DragEnter;
            this.DragDrop += MainForm_DragDrop;
            txtSourcePath.DragEnter += MainForm_DragEnter;
            txtSourcePath.DragDrop += MainForm_DragDrop;
            txtTargetPaths.DragEnter += TxtTargetPaths_DragEnter;
            txtTargetPaths.DragDrop += TxtTargetPaths_DragDrop;
            txtBlacklist.DragEnter += TxtBlacklist_DragEnter;
            txtBlacklist.DragDrop += TxtBlacklist_DragDrop;

            btnBrowseSource.Click += BtnBrowseSource_Click;
            btnStartCopy.Click += BtnStartCopy_Click;
            btnCancel.Click += BtnCancel_Click;
            btnSaveConfig.Click += BtnSaveConfig_Click;
            // 移除重复的事件绑定，因为在InitializeComponent()中已经绑定
            // btnNewConfig.Click += BtnNewConfig_Click;

            // 设置默认黑名单
            txtBlacklist.Text = "ArkApi\r\nlogs\r\nShooterGame";
            
            // 设置初始状态提示
            lblProgress.Text = "准备就绪";
            progressBarCopy.Value = 0;
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data != null)
            {
                var files = e.Data.GetData(DataFormats.FileDrop) as string[];
                if (files != null && files.Length > 0)
                {
                    string path = files[0];
                    if (Directory.Exists(path))
                    {
                        txtSourcePath.Text = path;
                    }
                    else if (File.Exists(path))
                    {
                        var dirPath = Path.GetDirectoryName(path);
                        txtSourcePath.Text = dirPath ?? "";
                    }
                }
            }
        }

        private void BtnBrowseSource_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "选择源文件夹";
                
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    txtSourcePath.Text = dialog.SelectedPath;
                }
            }
        }

        private async void BtnStartCopy_Click(object sender, EventArgs e)
        {
            if (_isCopying)
                return;

            if (string.IsNullOrWhiteSpace(txtSourcePath.Text))
            {
                MessageBox.Show("请选择源文件夹", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 验证路径格式是否有效
            if (!IsValidPath(txtSourcePath.Text))
            {
                MessageBox.Show("源文件夹路径格式无效", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 验证路径是否存在且是目录
            if (!Directory.Exists(txtSourcePath.Text))
            {
                var result = MessageBox.Show("源文件夹不存在，是否要创建该目录？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        Directory.CreateDirectory(txtSourcePath.Text);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"创建目录失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    return;
                }
            }

            var targetPaths = GetTargetPaths();
            if (targetPaths.Count == 0)
            {
                MessageBox.Show("请至少输入一个目标路径", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 验证目标路径
            var invalidPaths = targetPaths.Where(path => !IsValidPath(path)).ToList();
            if (invalidPaths.Count > 0)
            {
                MessageBox.Show($"以下路径无效:\n{string.Join("\n", invalidPaths)}",
                    "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            await StartCopyOperation();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (_cancellationTokenSource != null)
                _cancellationTokenSource.Cancel();
        }

        private void BtnHelp_Click(object sender, EventArgs e)
        {
            // 创建使用帮助对话框内容
            var helpContent = "文件复制助手 v2.0 作者：唐小布 无极交流群1016741666\n\n" +
                              "【工具功能】\n" +
                              "- 将一个源文件夹中的文件同时复制到多个目标路径\n" +
                              "- 支持黑名单过滤，可排除特定文件或文件夹\n" +
                              "- 支持配置保存和加载，方便重复使用\n" +
                              "- 显示复制进度和详细信息\n" +
                              "- 处理文件冲突情况\n\n" +
                              "【使用方法】\n" +
                              "1. 源文件夹设置\n" +
                              "   - 在上方\"源文件夹\"文本框中输入或拖拽文件夹路径\n" +
                              "   - 点击\"浏览...\"按钮选择源文件夹\n\n" +
                              "2. 目标路径设置\n" +
                              "   - 在\"目标路径\"文本框中输入多个目标文件夹路径\n" +
                              "   - 每行一个路径，支持多个目标同时复制\n\n" +
                              "3. 黑名单设置\n" +
                              "   - 在\"黑名单设置\"文本框中输入要忽略的文件或文件夹名称\n" +
                              "   - 每行一个名称，支持通配符（*匹配任意字符，?匹配单个字符）\n" +
                              "   - 例如：*.tmp, Thumbs.db, .git\n\n" +
                              "4. 开始复制\n" +
                              "   - 点击\"开始复制\"按钮启动复制操作\n" +
                              "   - 复制过程中可点击\"取消复制\"按钮中断操作\n\n" +
                              "5. 配置管理\n" +
                              "   - 点击\"保存\"可保存当前设置为配置方案\n" +
                              "   - 点击\"新增\"可创建新的配置方案\n" +
                              "   - 点击\"删除\"可删除选中的配置方案\n" +
                              "   - 程序会自动加载上次使用的配置方案\n\n" +
                              "【提示】\n" +
                              "- 源文件夹路径在复制过程中会被锁定，防止修改\n" +
                              "- 黑名单可用于排除临时文件、系统文件或特定类型文件\n" +
                              "- 配置方案保存在本地，可随时调用重复使用";

            MessageBox.Show(helpContent, "使用帮助", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async Task StartCopyOperation()
        {
            _isCopying = true;
            _cancellationTokenSource = new CancellationTokenSource();

            SetUIState(false);

            try
            {
                var config = GetCurrentConfiguration();
                var progress = new Progress<(int current, int total, string currentFile, string targetFile, long currentBytes, long totalBytes)>(
                    value => UpdateProgress(value.current, value.total, value.currentFile, value.targetFile, value.currentBytes, value.totalBytes));

                // 重置冲突对话框状态，确保每次复制操作都能重新显示冲突对话框
                FileCopyService.ResetConflictDialogState();

                var result = await _copyService.CopyFilesAsync(
                    config.SourcePath,
                    config.TargetPaths,
                    config.BlackList,
                    progress,
                    _cancellationTokenSource.Token);

                if (result.Success)
                {
                    lblProgress.Text = "复制完成";
                    MessageBox.Show($"复制完成!\n\n成功复制: {result.CopiedFiles} 个文件\n跳过文件: {result.SkippedFiles} 个",
                        "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    lblProgress.Text = "复制失败";
                    MessageBox.Show($"复制失败: {result.ErrorMessage}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (OperationCanceledException)
            {
                lblProgress.Text = "复制已取消";
                progressBarCopy.Value = 0;
            }
            catch (Exception ex)
            {
                lblProgress.Text = "复制出错";
                MessageBox.Show($"复制过程中发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isCopying = false;
                SetUIState(true);
                
                // 重置进度条并将状态提示恢复为准备就绪
                progressBarCopy.Value = 0;
                lblProgress.Text = "准备就绪";
                
                if (_cancellationTokenSource != null)
                {
                    _cancellationTokenSource.Dispose();
                    _cancellationTokenSource = null;
                }
            }
        }

        private void UpdateProgress(int current, int total, string sourceFile, string targetFile, long currentBytes, long totalBytes)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateProgress(current, total, sourceFile, targetFile, currentBytes, totalBytes)));
                return;
            }

            if (total > 0)
            {
                int percentage = (int)((double)current / total * 100);
                progressBarCopy.Value = Math.Min(percentage, 100);
            }

            // 如果没有当前文件或已完成，则显示相应的状态
            if (string.IsNullOrEmpty(sourceFile))
            {
                return; // 不更新状态文本，由调用者处理
            }

            // 显示目标路径和源文件名
            string fileName = Path.GetFileName(sourceFile);
            if (!string.IsNullOrEmpty(targetFile))
            {
                // 显示正在复制到的具体目标路径
                lblProgress.Text = $"正在复制到 {targetFile}: {fileName}";
            }
            else
            {
                // 显示准备复制的文件名称
                var config = GetCurrentConfiguration();
                if (config.TargetPaths.Count > 1)
                {
                    // 如果有多个目标，显示目标数量
                    lblProgress.Text = $"准备复制到 {config.TargetPaths.Count} 个目标: {fileName}";
                }
                else
                {
                    // 单个目标时的简洁显示
                    lblProgress.Text = $"正在复制: {fileName}";
                }
            }
        }

        private string FormatBytes(long bytes)
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

        private void SetUIState(bool enabled)
        {
            btnStartCopy.Enabled = enabled;
            btnCancel.Enabled = !enabled;
            btnBrowseSource.Enabled = enabled;
            txtSourcePath.Enabled = enabled;
            txtTargetPaths.Enabled = enabled;
            txtBlacklist.Enabled = enabled;
            btnSaveConfig.Enabled = enabled;
            btnNewConfig.Enabled = enabled;
            btnDeleteConfig.Enabled = enabled;
            cmbConfigurations.Enabled = enabled;
            
            // 当操作完成恢复UI状态时，重置进度条和状态提示
            if (enabled && !_isCopying)
            {
                progressBarCopy.Value = 0;
                // 注意：不要在这里直接重置lblProgress，因为不同的操作结果有不同的提示文本
            }
        }

        private List<string> GetTargetPaths()
        {
            return txtTargetPaths.Text
                .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(path => path.Trim())
                .Where(path => !string.IsNullOrWhiteSpace(path))
                .ToList();
        }

        private List<string> GetBlackList()
        {
            return txtBlacklist.Text
                .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(pattern => pattern.Trim())
                .Where(pattern => !string.IsNullOrWhiteSpace(pattern))
                .ToList();
        }

        private CopyConfiguration GetCurrentConfiguration()
        {
            return new CopyConfiguration
            {
                SourcePath = txtSourcePath.Text.Trim(),
                TargetPaths = GetTargetPaths(),
                BlackList = GetBlackList()
            };
        }

        private bool IsValidPath(string path)
        {
            try
            {
                return !string.IsNullOrWhiteSpace(path) &&
                       Path.IsPathRooted(path) &&
                       !path.Any(c => Path.GetInvalidPathChars().Contains(c));
            }
            catch
            {
                return false;
            }
        }

        private void TxtTargetPaths_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void TxtTargetPaths_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data != null)
            {
                var files = e.Data.GetData(DataFormats.FileDrop) as string[];
                if (files != null && files.Length > 0)
                {
                    // 获取当前已有的目标路径
                    var existingPaths = new HashSet<string>(GetTargetPaths(), StringComparer.OrdinalIgnoreCase);
                    var newPaths = new List<string>();

                    // 检查拖拽的所有路径
                    foreach (var path in files)
                    {
                        if (Directory.Exists(path) && !existingPaths.Contains(path))
                        {
                            newPaths.Add(path);
                            existingPaths.Add(path);
                        }
                        else if (File.Exists(path))
                        {
                            var dirPath = Path.GetDirectoryName(path);
                            if (!string.IsNullOrEmpty(dirPath) && !existingPaths.Contains(dirPath))
                            {
                                newPaths.Add(dirPath);
                                existingPaths.Add(dirPath);
                            }
                        }
                    }

                    // 如果有新路径，添加到文本框中
                    if (newPaths.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(txtTargetPaths.Text))
                        {
                            txtTargetPaths.AppendText(Environment.NewLine);
                        }
                        txtTargetPaths.AppendText(string.Join(Environment.NewLine, newPaths));
                    }
                }
            }
        }

        private void TxtBlacklist_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void TxtBlacklist_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data != null)
            {
                var items = e.Data.GetData(DataFormats.FileDrop) as string[];
                if (items != null && items.Length > 0)
                {
                    // 获取当前已有的黑名单项目
                    var existingItems = new HashSet<string>(GetBlackList(), StringComparer.OrdinalIgnoreCase);
                    var newItems = new List<string>();

                    // 检查拖拽的所有项目
                    foreach (var item in items)
                    {
                        // 对于文件和文件夹，只提取名称作为黑名单项目
                        string name;
                        if (Directory.Exists(item))
                        {
                            name = Path.GetFileName(item);
                        }
                        else if (File.Exists(item))
                        {
                            name = Path.GetFileName(item);
                        }
                        else
                        {
                            continue; // 忽略无效的项目
                        }

                        // 只添加新的、非空的项目
                        if (!string.IsNullOrEmpty(name) && !existingItems.Contains(name))
                        {
                            newItems.Add(name);
                            existingItems.Add(name);
                        }
                    }

                    // 如果有新项目，添加到文本框中
                    if (newItems.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(txtBlacklist.Text))
                        {
                            txtBlacklist.AppendText(Environment.NewLine);
                        }
                        txtBlacklist.AppendText(string.Join(Environment.NewLine, newItems));
                    }
                }
            }
        }

        private void BtnSaveConfig_Click(object sender, EventArgs e)
        {
            var config = GetCurrentConfiguration();
            if (string.IsNullOrWhiteSpace(config.SourcePath))
            {
                MessageBox.Show("请先设置源文件夹", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 检查是否有选中的配置方案
            string configName = null;
            string originalConfigName = null;
            if (cmbConfigurations.SelectedItem is string selectedConfigName)
            {
                originalConfigName = selectedConfigName;
                // 使用选中的配置名称作为默认值打开保存对话框
                using var dialog = new SaveConfigDialog(selectedConfigName);
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    configName = dialog.ConfigName;
                }
                else
                {
                    return; // 用户取消操作
                }
            }
            else
            {
                // 如果没有选中的配置，显示对话框让用户输入新名称
                using var dialog = new SaveConfigDialog();
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    configName = dialog.ConfigName;
                }
                else
                {
                    return; // 用户取消操作
                }
            }

            // 检查是否需要覆盖现有配置
            if (!string.IsNullOrWhiteSpace(configName))
            {
                bool saveSuccess = false;
                
                // 如果当前有选中的配置，并且用户更改了配置名称
                if (!string.IsNullOrEmpty(originalConfigName) && originalConfigName != configName)
                {
                    // 直接更新当前解决方案，而不是创建新的
                    config.Name = configName;
                    saveSuccess = _configService.RenameConfiguration(originalConfigName, configName);
                }
                else
                {
                    // 正常保存配置
                    config.Name = configName;
                    saveSuccess = _configService.SaveConfiguration(config);
                }

                if (saveSuccess)
                {
                    MessageBox.Show("配置保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadConfigurations();
                    // 重新选择保存后的配置
                    cmbConfigurations.SelectedItem = configName;
                }
                else
                {
                    MessageBox.Show("配置保存失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnDeleteConfig_Click(object sender, EventArgs e)
        {
            // 检查是否正在复制过程中
            if (_isCopying)
            {
                MessageBox.Show("正在执行复制操作，请等待操作完成后再删除配置方案", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (cmbConfigurations.SelectedItem is string configName)
            {
                // 显示确认对话框
                DialogResult result = MessageBox.Show(
                    $"确定要删除配置方案 '{configName}' 吗？\n此操作不可撤销。", 
                    "确认删除", 
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    // 临时禁用删除按钮，防止重复点击
                    btnDeleteConfig.Enabled = false;
                    try
                    {
                        if (_configService.DeleteConfiguration(configName))
                        {
                            MessageBox.Show("配置方案删除成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadConfigurations();
                            
                            // 如果还有其他配置方案，自动加载第一个方案
                            if (cmbConfigurations.Items.Count > 0)
                            {
                                cmbConfigurations.SelectedIndex = 0; // 选择第一个配置
                                if (cmbConfigurations.SelectedItem is string firstConfigName)
                                {
                                    var firstConfig = _configService.LoadConfiguration(firstConfigName);
                                    if (firstConfig != null)
                                    {
                                        LoadConfiguration(firstConfig);
                                    }
                                }
                            }
                            // 如果没有其他配置方案，清空配置内容
                            else if (cmbConfigurations.SelectedItem == null)
                            {
                                txtSourcePath.Text = "";
                                txtTargetPaths.Text = "";
                                txtBlacklist.Text = "ArkApi\r\nlogs\r\nShooterGame";
                            }
                        }
                        else
                        {
                            MessageBox.Show("配置方案删除失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    finally
                    {
                        // 确保按钮状态恢复
                        btnDeleteConfig.Enabled = true;
                    }
                }
            }
            else
            {
                MessageBox.Show("请选择要删除的配置方案", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void cmbConfigurations_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbConfigurations.SelectedItem is string configName)
            {
                var config = _configService.LoadConfiguration(configName);
                if (config != null)
                {
                    LoadConfiguration(config);
                    // 不显示提示消息，以简化用户体验
                }
                else
                {
                    MessageBox.Show("配置加载失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // 为了向后兼容保留此方法，但不再从UI中显示
        private void BtnLoadConfig_Click(object sender, EventArgs e)
        {
            cmbConfigurations_SelectedIndexChanged(sender, e);
        }

        private void BtnNewConfig_Click(object sender, EventArgs e)
        {
            // 检查是否正在复制过程中
            if (_isCopying)
            {
                MessageBox.Show("正在执行复制操作，请等待操作完成后再创建新方案", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var config = GetCurrentConfiguration();
            if (string.IsNullOrWhiteSpace(config.SourcePath) && string.IsNullOrWhiteSpace(txtTargetPaths.Text))
            {
                MessageBox.Show("当前没有可保存的配置", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 临时禁用新增按钮，防止重复点击
            btnNewConfig.Enabled = false;
            try
            {
                // 显示对话框让用户输入新方案名称
                using var dialog = new SaveConfigDialog();
                DialogResult result = dialog.ShowDialog();
                
                if (result == DialogResult.OK)
                {
                    var configName = dialog.ConfigName;
                    if (!string.IsNullOrWhiteSpace(configName))
                    {
                        config.Name = configName;
                        if (_configService.SaveConfiguration(config))
                        {
                            MessageBox.Show("新方案创建成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadConfigurations();
                            // 重新选择创建的新方案
                            cmbConfigurations.SelectedItem = configName;
                        }
                        else
                        {
                            MessageBox.Show("新方案创建失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            finally
            {
                // 确保按钮状态恢复
                btnNewConfig.Enabled = true;
            }
        }

        private void LoadConfiguration(CopyConfiguration config)
        {
            txtSourcePath.Text = config.SourcePath;
            txtTargetPaths.Text = string.Join("\r\n", config.TargetPaths);
            txtBlacklist.Text = string.Join("\r\n", config.BlackList);
        }

        private void LoadConfigurations()
        {
            var configs = _configService.GetConfigurationNames();
            cmbConfigurations.Items.Clear();
            cmbConfigurations.Items.AddRange(configs.ToArray());
        }



        private void groupBoxTargets_Enter(object sender, EventArgs e)
        {

        }

        private void btnBrowseSource_Click_1(object sender, EventArgs e)
        {

        }
    }
}
