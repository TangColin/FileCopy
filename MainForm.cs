using FileHelper.Dialogs;
using FileHelper.Models;
using FileHelper.Services;
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

namespace FileHelper
{
    public partial class MainForm : Form
    {
        private readonly ConfigurationService _configService;
        private readonly FileCopyService _copyService;
        private readonly LinkService _linkService;
        private CancellationTokenSource _cancellationTokenSource;
        private bool _isCopying = false;
        private bool _isLinking = false;

        public MainForm()
        {
            InitializeComponent();
            // 设置程序图标（从嵌入资源加载）
            this.Icon = new Icon(typeof(MainForm).Assembly.GetManifestResourceStream("FileHelper.FileCopy.ico"));
            _configService = new ConfigurationService();
            _copyService = new FileCopyService();
            _linkService = new LinkService();

            InitializeForm();
            LoadConfigurations();
            
            // 自动加载上次使用的配置方案
            LoadLastUsedConfiguration();
        }
        
        private void LoadLastUsedConfiguration()
        {
            try
            {
                // 只加载文件复制配置到文件复制界面
                var allCopyConfigs = _configService.GetAllConfigurations();
                if (allCopyConfigs != null && allCopyConfigs.Count > 0)
                {
                    // 获取最近修改的配置（GetAllConfigurations已按LastModified降序排列）
                    var lastUsedCopyConfig = allCopyConfigs.FirstOrDefault();
                    if (lastUsedCopyConfig != null)
                    {
                        LoadConfiguration(lastUsedCopyConfig);
                        // 选择对应的配置项
                        cmbConfigurations.SelectedItem = lastUsedCopyConfig.Name;
                    }
                }
                
                // 只加载链接配置到链接界面
                var allLinkConfigs = _configService.GetAllLinkConfigurations();
                if (allLinkConfigs != null && allLinkConfigs.Count > 0)
                {
                    // 获取最近修改的链接配置
                    var lastUsedLinkConfig = allLinkConfigs.FirstOrDefault();
                    if (lastUsedLinkConfig != null)
                    {
                        LoadLinkConfiguration(lastUsedLinkConfig);
                        // 选择对应的配置项
                        cmbLinkConfigurations.SelectedItem = lastUsedLinkConfig.Name;
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
            txtLinkSourcePath.AllowDrop = true;
            txtLinkTargetPaths.AllowDrop = true;

            // 绑定事件
            this.DragEnter += MainForm_DragEnter;
            this.DragDrop += MainForm_DragDrop;
            txtSourcePath.DragEnter += MainForm_DragEnter;
            txtSourcePath.DragDrop += MainForm_DragDrop;
            txtTargetPaths.DragEnter += TxtTargetPaths_DragEnter;
            txtTargetPaths.DragDrop += TxtTargetPaths_DragDrop;
            txtBlacklist.DragEnter += TxtBlacklist_DragEnter;
            txtBlacklist.DragDrop += TxtBlacklist_DragDrop;
            
            // 文件链接拖拽事件
            txtLinkSourcePath.DragEnter += MainForm_DragEnter;
            txtLinkSourcePath.DragDrop += MainForm_DragDrop;
            txtLinkTargetPaths.DragEnter += TxtTargetPaths_DragEnter;
            txtLinkTargetPaths.DragDrop += TxtTargetPaths_DragDrop;

            btnBrowseSource.Click += BtnBrowseSource_Click;
            btnStartCopy.Click += BtnStartCopy_Click;
            btnCancel.Click += BtnCancel_Click;
            btnSaveConfig.Click += BtnSaveConfig_Click;
            // 移除重复的事件绑定，因为在InitializeComponent()中已经绑定
            // btnNewConfig.Click += BtnNewConfig_Click;

            // 文件链接按钮事件
            btnBrowseLinkSource.Click += BtnBrowseLinkSource_Click;
            btnCreateLink.Click += BtnCreateLink_Click;
            btnValidateLink.Click += BtnValidateLink_Click;
            btnDeleteLink.Click += BtnDeleteLink_Click;
            btnLinkHelp.Click += BtnLinkHelp_Click;
            btnLinkSaveConfig.Click += BtnLinkSaveConfig_Click;
            btnLinkNewConfig.Click += BtnLinkNewConfig_Click;
            btnLinkDeleteConfig.Click += BtnLinkDeleteConfig_Click;
            cmbLinkConfigurations.SelectedIndexChanged += CmbLinkConfigurations_SelectedIndexChanged;

            // 设置默认黑名单
            txtBlacklist.Text = "ArkApi\r\nlogs\r\nShooterGame";
            
            // 设置初始状态提示
            lblProgress.Text = "准备就绪";
            progressBarCopy.Value = 0;
            lblLinkProgress.Text = "准备就绪";
            progressBarLink.Value = 0;
            
            // 加载链接配置
            LoadLinkConfigurations();
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
                        // 根据当前活动的选项卡决定将路径设置到哪个文本框
                        if (tabControlMain.SelectedTab == tabPageCopy)
                        {
                            txtSourcePath.Text = path;
                        }
                        else if (tabControlMain.SelectedTab == tabPageLink)
                        {
                            txtLinkSourcePath.Text = path;
                        }
                    }
                    else if (File.Exists(path))
                    {
                        // 根据当前活动的选项卡决定将路径设置到哪个文本框
                        if (tabControlMain.SelectedTab == tabPageCopy)
                        {
                            // 对于文件复制，使用文件所在目录
                            var dirPath = Path.GetDirectoryName(path);
                            txtSourcePath.Text = dirPath ?? "";
                        }
                        else if (tabControlMain.SelectedTab == tabPageLink)
                        {
                            // 对于文件链接，直接使用文件路径
                            txtLinkSourcePath.Text = path;
                        }
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

        private void BtnBrowseLinkSource_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "选择源文件夹";
                
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    txtLinkSourcePath.Text = dialog.SelectedPath;
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

        private async void BtnCreateLink_Click(object sender, EventArgs e)
        {
            if (_isLinking)
                return;

            if (string.IsNullOrWhiteSpace(txtLinkSourcePath.Text))
            {
                MessageBox.Show("请选择源文件夹", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 验证路径格式是否有效
            if (!IsValidPath(txtLinkSourcePath.Text))
            {
                MessageBox.Show("源文件夹路径格式无效", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 验证路径是否存在
            if (!Directory.Exists(txtLinkSourcePath.Text) && !File.Exists(txtLinkSourcePath.Text))
            {
                MessageBox.Show("源文件夹或文件不存在", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var targetPaths = GetLinkTargetPaths();
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

            await StartLinkOperation(LinkOperationType.Create);
        }

        private async void BtnValidateLink_Click(object sender, EventArgs e)
        {
            if (_isLinking)
                return;

            if (string.IsNullOrWhiteSpace(txtLinkSourcePath.Text))
            {
                MessageBox.Show("请选择源文件夹", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var targetPaths = GetLinkTargetPaths();
            if (targetPaths.Count == 0)
            {
                MessageBox.Show("请至少输入一个目标路径", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            await StartLinkOperation(LinkOperationType.Validate);
        }

        private async void BtnDeleteLink_Click(object sender, EventArgs e)
        {
            if (_isLinking)
                return;

            var targetPaths = GetLinkTargetPaths();
            if (targetPaths.Count == 0)
            {
                MessageBox.Show("请至少输入一个目标路径", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show("确定要删除这些链接吗？此操作不可撤销。", "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                await StartLinkOperation(LinkOperationType.Delete);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (_cancellationTokenSource != null)
                _cancellationTokenSource.Cancel();
        }

        private void BtnHelp_Click(object sender, EventArgs e)
        {
            // 创建使用帮助对话框内容
            var helpContent = "文件复制助手 v3.0 作者：唐小布 无极交流群1016741666\n\n" +
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

        private void BtnLinkHelp_Click(object sender, EventArgs e)
        {
            // 创建链接使用帮助对话框内容
            var helpContent = "文件链接助手 v3.0 作者：唐小布 无极交流群1016741666\n\n" +
                              "【工具功能】\n" +
                              "- 创建目录链接、符号链接或硬链接\n" +
                              "- 支持一个源文件夹/文件链接到多个目标路径\n" +
                              "- 支持链接验证和删除\n" +
                              "- 支持配置保存和加载，方便重复使用\n\n" +
                              "【链接类型说明】\n" +
                              "- 目录链接 (Junction): 只能链接目录，适用于本地NTFS卷\n" +
                              "- 符号链接 (Symbolic): 可链接文件或目录，支持跨卷链接\n" +
                              "- 硬链接 (HardLink): 只能链接文件，创建指向同一文件数据的多个入口\n\n" +
                              "【使用方法】\n" +
                              "1. 链接类型选择\n" +
                              "   - 选择要创建的链接类型（默认为目录链接）\n\n" +
                              "2. 源文件夹/文件设置\n" +
                              "   - 在\"源文件夹\"文本框中输入或拖拽文件夹/文件路径\n" +
                              "   - 点击\"浏览...\"按钮选择源文件夹/文件\n\n" +
                              "3. 目标路径设置\n" +
                              "   - 在\"目标路径\"文本框中输入多个目标路径\n" +
                              "   - 每行一个路径，支持多个目标同时创建链接\n\n" +
                              "4. 创建链接\n" +
                              "   - 点击\"创建链接\"按钮启动链接创建操作\n\n" +
                              "5. 验证链接\n" +
                              "   - 点击\"验证链接\"按钮验证链接是否有效\n\n" +
                              "6. 删除链接\n" +
                              "   - 点击\"删除链接\"按钮删除已创建的链接\n\n" +
                              "7. 配置管理\n" +
                              "   - 点击\"保存\"可保存当前设置为配置方案\n" +
                              "   - 点击\"新增\"可创建新的配置方案\n" +
                              "   - 点击\"删除\"可删除选中的配置方案\n\n" +
                              "【注意事项】\n" +
                              "- 创建符号链接可能需要管理员权限\n" +
                              "- 硬链接只能用于文件，不能用于目录\n" +
                              "- 配置方案保存在本地，可随时调用重复使用";

            MessageBox.Show(helpContent, "链接使用帮助", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private async Task StartLinkOperation(LinkOperationType operationType)
        {
            _isLinking = true;
            _cancellationTokenSource = new CancellationTokenSource();

            SetLinkUIState(false);

            try
            {
                var config = GetCurrentLinkConfiguration();
                var progress = new Progress<(int current, int total, string targetPath, LinkOperationStatus status)>(
                    value => UpdateLinkProgress(value.current, value.total, value.targetPath, value.status));

                // 为创建操作注册冲突处理事件
                if (operationType == LinkOperationType.Create)
                {
                    _linkService.OnConflict += HandleLinkConflict;
                }

                LinkResult result = null;

                switch (operationType)
                {
                    case LinkOperationType.Create:
                        result = await _linkService.CreateLinksAsync(
                            config.SourcePath,
                            config.TargetPaths,
                            config.LinkType,
                            progress);
                        break;
                    case LinkOperationType.Validate:
                        result = await _linkService.ValidateLinksAsync(
                            config.SourcePath,
                            config.TargetPaths,
                            config.LinkType,
                            progress);
                        break;
                    case LinkOperationType.Delete:
                        result = await _linkService.DeleteLinksAsync(
                            config.TargetPaths,
                            progress);
                        break;
                }

                // 取消事件注册
                if (operationType == LinkOperationType.Create)
                {
                    _linkService.OnConflict -= HandleLinkConflict;
                }

                if (result.Success)
                {
                    switch (operationType)
                    {
                        case LinkOperationType.Create:
                            lblLinkProgress.Text = "链接创建完成";
                            MessageBox.Show($"链接创建完成!\n\n成功创建: {result.CreatedLinks} 个链接\n跳过: {result.SkippedLinks} 个\n失败: {result.FailedLinks} 个",
                                "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        case LinkOperationType.Validate:
                            lblLinkProgress.Text = "链接验证完成";
                            MessageBox.Show($"链接验证完成!\n\n有效链接: {result.CreatedLinks} 个\n无效链接: {result.FailedLinks} 个",
                                "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        case LinkOperationType.Delete:
                            lblLinkProgress.Text = "链接删除完成";
                            MessageBox.Show($"链接删除完成!\n\n成功删除: {result.CreatedLinks} 个链接\n跳过: {result.SkippedLinks} 个\n失败: {result.FailedLinks} 个",
                                "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                    }
                }
                else
                {
                    lblLinkProgress.Text = "操作失败";
                    // 显示更详细的错误信息
                    string errorMessage = $"操作失败: {result.ErrorMessage}";
                    if (result.FailedLinks > 0)
                    {
                        errorMessage += $"\n\n失败的链接数量: {result.FailedLinks}";
                    }
                    MessageBox.Show(errorMessage, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (OperationCanceledException)
            {
                lblLinkProgress.Text = "操作已取消";
                progressBarLink.Value = 0;
            }
            catch (Exception ex)
            {
                lblLinkProgress.Text = "操作出错";
                // 显示更详细的异常信息
                string errorMessage = $"操作过程中发生错误: {ex.Message}";
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    errorMessage += $"\n\n详细信息: {ex}";
                }
                MessageBox.Show(errorMessage, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isLinking = false;
                SetLinkUIState(true);
                
                // 重置进度条并将状态提示恢复为准备就绪
                progressBarLink.Value = 0;
                lblLinkProgress.Text = "准备就绪";
                
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

        private void UpdateLinkProgress(int current, int total, string targetPath, LinkOperationStatus status)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateLinkProgress(current, total, targetPath, status)));
                return;
            }

            if (total > 0)
            {
                int percentage = (int)((double)current / total * 100);
                progressBarLink.Value = Math.Min(percentage, 100);
            }

            // 更新状态文本，显示完整路径而不是仅文件名
            switch (status)
            {
                case LinkOperationStatus.Creating:
                    lblLinkProgress.Text = $"正在创建链接: {targetPath}";
                    break;
                case LinkOperationStatus.Validating:
                    lblLinkProgress.Text = $"正在验证链接: {targetPath}";
                    break;
                case LinkOperationStatus.Deleting:
                    lblLinkProgress.Text = $"正在删除链接: {targetPath}";
                    break;
                case LinkOperationStatus.Success:
                    lblLinkProgress.Text = $"操作成功: {targetPath}";
                    break;
                case LinkOperationStatus.Skipped:
                    lblLinkProgress.Text = $"已跳过: {targetPath}";
                    break;
                case LinkOperationStatus.Failed:
                    lblLinkProgress.Text = $"操作失败: {targetPath}";
                    break;
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
            // 文件复制UI状态
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

        private void SetLinkUIState(bool enabled)
        {
            // 文件链接UI状态
            btnCreateLink.Enabled = enabled;
            btnValidateLink.Enabled = enabled;
            btnDeleteLink.Enabled = enabled;
            btnBrowseLinkSource.Enabled = enabled;
            txtLinkSourcePath.Enabled = enabled;
            txtLinkTargetPaths.Enabled = enabled;
            btnLinkSaveConfig.Enabled = enabled;
            btnLinkNewConfig.Enabled = enabled;
            btnLinkDeleteConfig.Enabled = enabled;
            cmbLinkConfigurations.Enabled = enabled;
            radioJunction.Enabled = enabled;
            radioSymbolic.Enabled = enabled;
            radioHardLink.Enabled = enabled;
            
            // 当操作完成恢复UI状态时，重置进度条和状态提示
            if (enabled && !_isLinking)
            {
                progressBarLink.Value = 0;
                // 注意：不要在这里直接重置lblLinkProgress，因为不同的操作结果有不同的提示文本
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

        private List<string> GetLinkTargetPaths()
        {
            return txtLinkTargetPaths.Text
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

        private LinkConfiguration GetCurrentLinkConfiguration()
        {
            LinkType linkType = LinkType.Junction;
            if (radioSymbolic.Checked)
                linkType = LinkType.Symbolic;
            else if (radioHardLink.Checked)
                linkType = LinkType.HardLink;

            return new LinkConfiguration
            {
                LinkType = linkType,
                SourcePath = txtLinkSourcePath.Text.Trim(),
                TargetPaths = GetLinkTargetPaths()
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
                    TextBox targetTextBox = sender == txtTargetPaths ? txtTargetPaths : txtLinkTargetPaths;
                    
                    // 获取当前已有的目标路径
                    var existingPaths = new HashSet<string>(
                        targetTextBox.Text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(path => path.Trim())
                            .Where(path => !string.IsNullOrWhiteSpace(path)),
                        StringComparer.OrdinalIgnoreCase);
                    
                    var newPaths = new List<string>();

                    // 检查拖拽的所有路径
                    foreach (var path in files)
                    {
                        if ((Directory.Exists(path) || File.Exists(path)) && !existingPaths.Contains(path))
                        {
                            newPaths.Add(path);
                            existingPaths.Add(path);
                        }
                    }

                    // 如果有新路径，添加到文本框中
                    if (newPaths.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(targetTextBox.Text))
                        {
                            targetTextBox.AppendText(Environment.NewLine);
                        }
                        targetTextBox.AppendText(string.Join(Environment.NewLine, newPaths));
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

        private void BtnLinkSaveConfig_Click(object sender, EventArgs e)
        {
            var config = GetCurrentLinkConfiguration();
            if (string.IsNullOrWhiteSpace(config.SourcePath))
            {
                MessageBox.Show("请先设置源文件夹", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 检查是否有选中的配置方案
            string configName = null;
            string originalConfigName = null;
            if (cmbLinkConfigurations.SelectedItem is string selectedConfigName)
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
                    saveSuccess = _configService.RenameLinkConfiguration(originalConfigName, configName);
                }
                else
                {
                    // 正常保存配置
                    config.Name = configName;
                    saveSuccess = _configService.SaveLinkConfiguration(config);
                }

                if (saveSuccess)
                {
                    MessageBox.Show("链接配置保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadLinkConfigurations();
                    // 重新选择保存后的配置
                    cmbLinkConfigurations.SelectedItem = configName;
                }
                else
                {
                    MessageBox.Show("链接配置保存失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void BtnLinkDeleteConfig_Click(object sender, EventArgs e)
        {
            // 检查是否正在链接过程中
            if (_isLinking)
            {
                MessageBox.Show("正在执行链接操作，请等待操作完成后再删除配置方案", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (cmbLinkConfigurations.SelectedItem is string configName)
            {
                // 显示确认对话框
                DialogResult result = MessageBox.Show(
                    $"确定要删除链接配置方案 '{configName}' 吗？\n此操作不可撤销。", 
                    "确认删除", 
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    // 临时禁用删除按钮，防止重复点击
                    btnLinkDeleteConfig.Enabled = false;
                    try
                    {
                        if (_configService.DeleteLinkConfiguration(configName))
                        {
                            MessageBox.Show("链接配置方案删除成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadLinkConfigurations();
                            
                            // 如果还有其他配置方案，自动加载第一个方案
                            if (cmbLinkConfigurations.Items.Count > 0)
                            {
                                cmbLinkConfigurations.SelectedIndex = 0; // 选择第一个配置
                                if (cmbLinkConfigurations.SelectedItem is string firstConfigName)
                                {
                                    var firstConfig = _configService.LoadLinkConfiguration(firstConfigName);
                                    if (firstConfig != null)
                                    {
                                        LoadLinkConfiguration(firstConfig);
                                    }
                                }
                            }
                            // 如果没有其他配置方案，清空配置内容
                            else if (cmbLinkConfigurations.SelectedItem == null)
                            {
                                txtLinkSourcePath.Text = "";
                                txtLinkTargetPaths.Text = "";
                                radioJunction.Checked = true;
                            }
                        }
                        else
                        {
                            MessageBox.Show("链接配置方案删除失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    finally
                    {
                        // 确保按钮状态恢复
                        btnLinkDeleteConfig.Enabled = true;
                    }
                }
            }
            else
            {
                MessageBox.Show("请选择要删除的链接配置方案", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void CmbLinkConfigurations_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbLinkConfigurations.SelectedItem is string configName)
            {
                var config = _configService.LoadLinkConfiguration(configName);
                if (config != null)
                {
                    LoadLinkConfiguration(config);
                    // 不显示提示消息，以简化用户体验
                }
                else
                {
                    MessageBox.Show("链接配置加载失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void BtnLinkNewConfig_Click(object sender, EventArgs e)
        {
            // 检查是否正在链接过程中
            if (_isLinking)
            {
                MessageBox.Show("正在执行链接操作，请等待操作完成后再创建新方案", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var config = GetCurrentLinkConfiguration();
            if (string.IsNullOrWhiteSpace(config.SourcePath) && string.IsNullOrWhiteSpace(txtLinkTargetPaths.Text))
            {
                MessageBox.Show("当前没有可保存的链接配置", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 临时禁用新增按钮，防止重复点击
            btnLinkNewConfig.Enabled = false;
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
                        if (_configService.SaveLinkConfiguration(config))
                        {
                            MessageBox.Show("新链接方案创建成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadLinkConfigurations();
                            // 重新选择创建的新方案
                            cmbLinkConfigurations.SelectedItem = configName;
                        }
                        else
                        {
                            MessageBox.Show("新链接方案创建失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            finally
            {
                // 确保按钮状态恢复
                btnLinkNewConfig.Enabled = true;
            }
        }

        private void LoadConfiguration(CopyConfiguration config)
        {
            txtSourcePath.Text = config.SourcePath;
            txtTargetPaths.Text = string.Join("\r\n", config.TargetPaths);
            txtBlacklist.Text = string.Join("\r\n", config.BlackList);
        }

        private void LoadLinkConfiguration(LinkConfiguration config)
        {
            // 设置链接类型
            switch (config.LinkType)
            {
                case LinkType.Junction:
                    radioJunction.Checked = true;
                    break;
                case LinkType.Symbolic:
                    radioSymbolic.Checked = true;
                    break;
                case LinkType.HardLink:
                    radioHardLink.Checked = true;
                    break;
            }
            
            txtLinkSourcePath.Text = config.SourcePath;
            txtLinkTargetPaths.Text = string.Join("\r\n", config.TargetPaths);
        }

        private void LoadConfigurations()
        {
            var configs = _configService.GetConfigurationNames();
            cmbConfigurations.Items.Clear();
            cmbConfigurations.Items.AddRange(configs.ToArray());
        }

        private void LoadLinkConfigurations()
        {
            var configs = _configService.GetLinkConfigurationNames();
            cmbLinkConfigurations.Items.Clear();
            cmbLinkConfigurations.Items.AddRange(configs.ToArray());
        }

        private void groupBoxTargets_Enter(object sender, EventArgs e)
        {

        }

        private void btnBrowseSource_Click_1(object sender, EventArgs e)
        {

        }

        private void txtSourcePath_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnValidateLink_Click_1(object sender, EventArgs e)
        {

        }

        private LinkConflictDialog.ConflictAction HandleLinkConflict(string targetPath)
        {
            // 在UI线程上显示冲突对话框
            if (InvokeRequired)
            {
                return (LinkConflictDialog.ConflictAction)Invoke(new Func<string, LinkConflictDialog.ConflictAction>(HandleLinkConflict), targetPath);
            }

            using (var dialog = new LinkConflictDialog(targetPath))
            {
                var result = dialog.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    return dialog.Action;
                }
                else
                {
                    return LinkConflictDialog.ConflictAction.Cancel;
                }
            }
        }
    }

    // 辅助枚举
    public enum LinkOperationType
    {
        Create,
        Validate,
        Delete
    }
}














