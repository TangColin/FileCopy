# FileCopyHelper 项目分析与文件映射模块设计

## 1. 项目概述

FileCopyHelper 是一个基于 Windows Forms 的文件批量复制工具，使用 C# 和 .NET Framework 4.7.2 开发。该工具旨在简化将单个源文件夹内容同时复制到多个目标路径的操作流程。

### 1.1 核心功能
- 多目标同步复制
- 智能黑名单过滤（支持通配符匹配）
- 配置管理（保存和加载配置方案）
- 实时进度显示
- 文件冲突处理
- 拖拽操作支持
- 自动恢复上次配置

### 1.2 技术架构
- **前端**: Windows Forms
- **后端**: C#
- **框架**: .NET Framework 4.7.2
- **数据格式**: JSON (配置存储)
- **第三方库**: Newtonsoft.Json (JSON序列化), Costura.Fody (程序集嵌入)

## 2. 现有架构分析

### 2.1 分层架构设计
项目采用分层设计模式，分为UI层、服务层和数据层三层架构：
- **UI层**: MainForm 及相关对话框 (ConflictDialog, SaveConfigDialog)
- **服务层**: FileCopyService (文件复制逻辑), ConfigurationService (配置管理)
- **数据层**: CopyConfiguration (配置数据模型), CopyResult (复制结果模型)

### 2.2 核心组件交互关系
```
MainForm (UI层)
├── FileCopyService (服务层)
│   ├── CopyConfiguration (数据模型)
│   └── CopyResult (结果模型)
├── ConfigurationService (服务层)
│   └── CopyConfiguration (数据模型)
└── Dialogs (UI组件)
    ├── ConflictDialog
    └── SaveConfigDialog
```

### 2.3 数据流分析
1. 用户在 MainForm 中设置源路径、目标路径和黑名单
2. MainForm 调用 ConfigurationService 保存/加载配置
3. 用户点击"开始复制"时，MainForm 调用 FileCopyService 执行复制操作
4. FileCopyService 返回 CopyResult 对象给 MainForm 显示结果

## 3. 新增功能需求分析

### 3.1 功能描述
新增文件映射模块，使用 Windows 系统自带的 MKLINK 命令创建符号链接、硬链接和目录联接。

### 3.2 功能要求
1. 界面采用分页布局，左侧为文件复制，右侧为文件链接
2. 链接类型选择（符号链接、硬链接、目录联接）
3. 源路径和目标路径输入方式与文件复制模块保持一致（支持拖拽、浏览、手动输入）
4. 支持配置方案管理（与文件复制模块相同）
5. 界面布局分为四个板块：
   - 链接类型选择（默认目录链接）
   - 源目标路径（支持拖拽、浏览、手动输入）
   - 目标路径（多行文本框，支持拖拽或手动输入）
   - 操作面板（创建链接、验证链接、删除链接、使用帮助、配置管理、进度条、状态提示）

## 4. 新增模块设计

### 4.1 界面设计
```
+-------------------------------------------------------------+
| 文件复制助手 v2.0                                           |
+----------------------+--------------------------------------+
| 文件复制             | 文件链接                             |
| [Tab1]               | [Tab2]                               |
+----------------------+--------------------------------------+
|                                                              |
| [文件复制模块]           [文件链接模块]                        |
|                      +------------------------------------+ |
|                      | 链接类型: (o) 目录链接 ( ) 文件链接  | |
|                      |           ( ) 硬链接               | |
|                      +------------------------------------+ |
|                      | 源文件夹（拖拽或浏览设置目录）       | |
|                      | [路径输入框] [浏览...]              | |
|                      +------------------------------------+ |
|                      | 目标路径（拖拽或输入路径，每行一个）  | |
|                      | [多行文本框]                        | |
|                      +------------------------------------+ |
|                      | 操作面板                            | |
|                      | [创建链接] [验证链接] [删除链接]      | |
|                      | [使用帮助]                          | |
|                      | 配置管理: [配置选择] [保存] [新增] [删除]| |
|                      | [进度条]                            | |
|                      | [状态提示]                          | |
|                      +------------------------------------+ |
+-------------------------------------------------------------+
| [状态栏]                                                    |
+-------------------------------------------------------------+
```

### 4.2 类结构设计

#### 4.2.1 新增数据模型
```csharp
// LinkConfiguration.cs
public class LinkConfiguration
{
    public string Name { get; set; } = string.Empty;
    public LinkType LinkType { get; set; } = LinkType.Junction;
    public string SourcePath { get; set; } = string.Empty;
    public List<string> TargetPaths { get; set; } = new List<string>();
    public DateTime LastModified { get; set; } = DateTime.Now;

    public string ToJson()
    {
        return JsonConvert.SerializeObject(this, Formatting.Indented);
    }

    public static LinkConfiguration FromJson(string json)
    {
        try
        {
            return JsonConvert.DeserializeObject<LinkConfiguration>(json);
        }
        catch
        {
            return null;
        }
    }
}

public enum LinkType
{
    Unknown,     // 未知类型
    Junction,    // 目录联接
    Symbolic,    // 符号链接
    HardLink     // 硬链接
}

// LinkResult.cs
public class LinkResult
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public int TotalLinks { get; set; }
    public int CreatedLinks { get; set; }
    public int SkippedLinks { get; set; }
    public int FailedLinks { get; set; }
}

public enum LinkOperationStatus
{
    Creating,
    Validating,
    Deleting,
    Success,
    Skipped,
    Failed
}
```

#### 4.2.2 新增服务类
```csharp
// LinkService.cs
public class LinkService
{
    private const int BUFFER_SIZE = 1024 * 1024; // 1MB buffer
    
    public async Task<LinkResult> CreateLinksAsync(
        LinkType linkType,
        string sourcePath,
        List<string> targetPaths,
        IProgress<(int current, int total, string currentTarget, LinkOperationStatus status)> progress,
        CancellationToken cancellationToken)
    {
        var result = new LinkResult();
        
        try
        {
            result.TotalLinks = targetPaths.Count;
            int currentIndex = 0;
            
            foreach (string targetPath in targetPaths)
            {
                cancellationToken.ThrowIfCancellationRequested();
                
                // 报告进度
                progress?.Report((currentIndex, result.TotalLinks, targetPath, LinkOperationStatus.Creating));
                
                try
                {
                    string command = GetLinkCommand(linkType, sourcePath, targetPath);
                    bool success = ExecuteLinkCommand(command);
                    
                    if (success)
                    {
                        result.CreatedLinks++;
                        progress?.Report((currentIndex, result.TotalLinks, targetPath, LinkOperationStatus.Success));
                    }
                    else
                    {
                        result.FailedLinks++;
                        progress?.Report((currentIndex, result.TotalLinks, targetPath, LinkOperationStatus.Failed));
                    }
                }
                catch (Exception ex)
                {
                    result.FailedLinks++;
                    progress?.Report((currentIndex, result.TotalLinks, targetPath, LinkOperationStatus.Failed));
                }
                
                currentIndex++;
            }
            
            result.Success = (result.FailedLinks == 0);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.Message;
        }
        
        return result;
    }
        
    public LinkValidationResult ValidateLink(string targetPath)
    {
        try
        {
            if (!Directory.Exists(targetPath) && !File.Exists(targetPath))
            {
                return new LinkValidationResult { IsValid = false, Message = "路径不存在" };
            }
            
            // 检查是否为链接
            LinkType linkType = DetectLinkType(targetPath);
            if (linkType == LinkType.Unknown)
            {
                return new LinkValidationResult { IsValid = false, Message = "不是有效的链接" };
            }
            
            // 获取目标路径
            string target = GetLinkTarget(targetPath);
            if (string.IsNullOrEmpty(target))
            {
                return new LinkValidationResult { IsValid = false, Message = "无法获取链接目标" };
            }
            
            return new LinkValidationResult 
            { 
                IsValid = true, 
                LinkType = linkType, 
                Target = target,
                Message = "链接有效"
            };
        }
        catch (Exception ex)
        {
            return new LinkValidationResult { IsValid = false, Message = $"验证失败: {ex.Message}" };
        }
    }
    
    public bool DeleteLink(string targetPath)
    {
        try
        {
            if (Directory.Exists(targetPath))
            {
                Directory.Delete(targetPath);
                return true;
            }
            else if (File.Exists(targetPath))
            {
                File.Delete(targetPath);
                return true;
            }
            
            return false;
        }
        catch
        {
            return false;
        }
    }
    
    private string GetLinkCommand(LinkType linkType, string sourcePath, string targetPath)
    {
        string targetDir = Path.GetDirectoryName(targetPath);
        if (!string.IsNullOrEmpty(targetDir) && !Directory.Exists(targetDir))
        {
            Directory.CreateDirectory(targetDir);
        }
        
        switch (linkType)
        {
            case LinkType.Junction:
                return $"/c mklink /J "{targetPath}" "{sourcePath}"";
            case LinkType.Symbolic:
                if (Directory.Exists(sourcePath))
                {
                    return $"/c mklink /D "{targetPath}" "{sourcePath}"";
                }
                else
                {
                    return $"/c mklink "{targetPath}" "{sourcePath}"";
                }
            case LinkType.HardLink:
                return $"/c mklink /H "{targetPath}" "{sourcePath}"";
            default:
                throw new ArgumentException("不支持的链接类型");
        }
    }
    
    private bool ExecuteLinkCommand(string command)
    {
        try
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = command,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };
            
            using (Process process = Process.Start(startInfo))
            {
                process.WaitForExit();
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                
                // 检查是否成功创建链接
                return process.ExitCode == 0 && 
                       (output.Contains("创建了符号链接") || 
                        output.Contains("创建了硬链接") || 
                        output.Contains("创建了联接"));
            }
        }
        catch
        {
            return false;
        }
    }
    
    private LinkType DetectLinkType(string targetPath)
    {
        try
        {
            if (Directory.Exists(targetPath))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(targetPath);
                if (dirInfo.Attributes.HasFlag(FileAttributes.ReparsePoint))
                {
                    // 进一步检查是符号链接还是目录联接
                    return CheckReparsePointType(targetPath);
                }
            }
            else if (File.Exists(targetPath))
            {
                FileInfo fileInfo = new FileInfo(targetPath);
                if (fileInfo.Attributes.HasFlag(FileAttributes.ReparsePoint))
                {
                    return LinkType.Symbolic;
                }
                else
                {
                    // 检查是否为硬链接
                    return CheckHardLink(targetPath);
                }
            }
            
            return LinkType.Unknown;
        }
        catch
        {
            return LinkType.Unknown;
        }
    }
    
    private LinkType CheckReparsePointType(string path)
    {
        // 简化实现，实际需要使用 Windows API 检查
        return LinkType.Junction; // 默认返回目录联接
    }
    
    private LinkType CheckHardLink(string filePath)
    {
        try
        {
            FileInfo fileInfo = new FileInfo(filePath);
            // 简化实现，实际需要检查文件链接计数
            return LinkType.HardLink;
        }
        catch
        {
            return LinkType.Unknown;
        }
    }
    
    private string GetLinkTarget(string linkPath)
    {
        try
        {
            if (Directory.Exists(linkPath))
            {
                return new DirectoryInfo(linkPath).LinkTarget;
            }
            else if (File.Exists(linkPath))
            {
                return new FileInfo(linkPath).LinkTarget;
            }
        }
        catch
        {
            // 忽略异常
        }
        
        return null;
    }
}

// LinkValidationResult.cs
public class LinkValidationResult
{
    public bool IsValid { get; set; }
    public LinkType LinkType { get; set; } = LinkType.Unknown;
    public string Target { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}
```

#### 4.2.3 扩展现有服务
```csharp
// ConfigurationService.cs (扩展)
public class ConfigurationService
{
    private readonly string _configDirectory;
    private readonly string _linkConfigDirectory;
    private readonly string _configExtension = ".json";
    
    public ConfigurationService()
    {
        // 使用程序当前目录作为配置文件存储位置
        _configDirectory = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "Configurations", "copy");
            
        _linkConfigDirectory = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "Configurations", "link");

        // 确保配置目录存在
        if (!Directory.Exists(_configDirectory))
        {
            Directory.CreateDirectory(_configDirectory);
        }
        
        if (!Directory.Exists(_linkConfigDirectory))
        {
            Directory.CreateDirectory(_linkConfigDirectory);
        }
    }
    
    // 新增方法
    public bool SaveLinkConfiguration(LinkConfiguration config)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(config.Name))
            {
                return false;
            }

            // 清理文件名中的无效字符
            string safeFileName = GetSafeFileName(config.Name);
            string filePath = Path.Combine(_linkConfigDirectory, safeFileName + _configExtension);

            config.LastModified = DateTime.Now;
            string json = config.ToJson();

            File.WriteAllText(filePath, json);
            return true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"保存链接配置失败: {ex.Message}");
            return false;
        }
    }
    
    public LinkConfiguration LoadLinkConfiguration(string configName)
    {
        try
        {
            string safeFileName = GetSafeFileName(configName);
            string filePath = Path.Combine(_linkConfigDirectory, safeFileName + _configExtension);

            if (!File.Exists(filePath))
            {
                return null;
            }

            string json = File.ReadAllText(filePath);
            return LinkConfiguration.FromJson(json);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"加载链接配置失败: {ex.Message}");
            return null;
        }
    }
    
    public List<string> GetLinkConfigurationNames()
    {
        try
        {
            if (!Directory.Exists(_linkConfigDirectory))
            {
                return new List<string>();
            }

            return Directory.GetFiles(_linkConfigDirectory, "*" + _configExtension)
                .Select(file => Path.GetFileNameWithoutExtension(file))
                .OrderBy(name => name)
                .ToList();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"获取链接配置列表失败: {ex.Message}");
            return new List<string>();
        }
    }
    
    public bool DeleteLinkConfiguration(string configName)
    {
        try
        {
            string safeFileName = GetSafeFileName(configName);
            string filePath = Path.Combine(_linkConfigDirectory, safeFileName + _configExtension);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"删除链接配置失败: {ex.Message}");
            return false;
        }
    }
    
    public List<LinkConfiguration> GetAllLinkConfigurations()
    {
        var configurations = new List<LinkConfiguration>();

        try
        {
            var configNames = GetLinkConfigurationNames();
            foreach (string configName in configNames)
            {
                var config = LoadLinkConfiguration(configName);
                if (config != null)
                {
                    configurations.Add(config);
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"获取所有链接配置失败: {ex.Message}");
        }

        return configurations.OrderByDescending(c => c.LastModified).ToList();
    }
    
    public bool RenameLinkConfiguration(string oldName, string newName)
    {
        try
        {
            var config = LoadLinkConfiguration(oldName);
            if (config == null)
            {
                return false;
            }

            config.Name = newName;
            if (SaveLinkConfiguration(config))
            {
                DeleteLinkConfiguration(oldName);
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"重命名链接配置失败: {ex.Message}");
            return false;
        }
    }
    
    private string GetSafeFileName(string fileName)
    {
        // 移除或替换文件名中的无效字符
        var invalidChars = Path.GetInvalidFileNameChars();
        string safeName = fileName;

        foreach (char invalidChar in invalidChars)
        {
            safeName = safeName.Replace(invalidChar, '_');
        }

        // 限制文件名长度
        if (safeName.Length > 200)
        {
            safeName = safeName.Substring(0, 200);
        }

        return safeName;
    }
}
```

#### 4.2.4 UI组件设计
```csharp
// MainForm.cs (修改)
public partial class MainForm : Form
{
    // 新增控件和字段
    private TabControl tabControlMain;
    private TabPage tabPageCopy;
    private TabPage tabPageLink;
    
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
    
    // 链接配置管理控件
    private GroupBox groupBoxLinkConfig;
    private ComboBox cmbLinkConfigurations;
    private Button btnSaveLinkConfig;
    private Button btnNewLinkConfig;
    private Button btnDeleteLinkConfig;
    private ProgressBar progressBarLink;
    private Label lblLinkProgress;
    
    // 文件链接服务
    private LinkService _linkService;
    
    // 文件链接配置管理
    private void InitializeLinkTab();
    private void LoadLinkConfiguration(LinkConfiguration config);
    private LinkConfiguration GetCurrentLinkConfiguration();
    private async Task StartLinkOperation();
    private void UpdateLinkProgress(int current, int total, string targetPath, LinkOperationStatus status);
    private void LoadLinkConfigurations();
    private void SaveLinkConfiguration();
    private void DeleteLinkConfiguration();
}

// LinkTypeDialog.cs (新增)
public partial class LinkTypeDialog : Form
{
    public LinkType SelectedLinkType { get; private set; }
    
    // 链接类型选择对话框
}
```

## 5. 实现方案

### 5.1 MKLINK 命令使用
Windows 的 MKLINK 命令语法：
- 目录联接: `mklink /J <链接> <目标>`
- 符号链接: `mklink /D <链接> <目标>` (目录) 或 `mklink <链接> <目标>` (文件)
- 硬链接: `mklink /H <链接> <目标>` (仅文件)

### 5.2 核心实现逻辑

#### 5.2.1 LinkService 实现要点
1. 使用 Process 类执行 MKLINK 命令
2. 异步执行避免UI阻塞
3. 实时进度报告
4. 错误处理和日志记录
5. 链接验证和删除功能
6. 权限检查和提示
7. 路径有效性验证
8. 链接状态监控

#### 5.2.2 权限处理
- 符号链接和目录联接可能需要管理员权限
- 需要检查并提示用户权限不足的情况
- 实现权限检查机制，在执行操作前验证用户权限

#### 5.2.3 路径验证
- 源路径有效性检查
- 目标路径冲突检查
- 路径格式验证

#### 5.3.1 链接配置管理
在现有的 ConfigurationService 基础上扩展链接配置管理功能，保持与文件复制配置相同的存储结构和管理方式。链接配置将存储在独立的目录中，避免与复制配置混淆。

ConfigurationService 构造函数需要增加初始化链接配置目录的代码：

```csharp
public ConfigurationService()
{
    // 使用程序当前目录作为配置文件存储位置
    _configDirectory = Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory,
        "Configurations", "copy");
        
    _linkConfigDirectory = Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory,
        "Configurations", "link");

    // 确保配置目录存在
    if (!Directory.Exists(_configDirectory))
    {
        Directory.CreateDirectory(_configDirectory);
    }
    
    if (!Directory.Exists(_linkConfigDirectory))
    {
        Directory.CreateDirectory(_linkConfigDirectory);
    }
}
```

### 5.3.2 配置存储结构
```
Configurations/
├── copy/
│   ├── config1.json
│   └── config2.json
└── link/
    ├── link_config1.json
    └── link_config2.json
```

通过将不同类型的配置存储在不同的子目录中，可以更好地组织和管理配置文件。

### 5.4 UI 交互设计
1. 保持与文件复制模块一致的视觉风格和交互方式
2. 实现 Tab 页面切换功能
3. 复用现有的拖拽、浏览、配置管理等功能
4. 添加链接特有的操作按钮（验证、删除）

### 5.5 权限检查机制
在执行链接操作前，需要检查当前用户是否具有足够的权限创建特定类型的链接：

```csharp
private bool CheckLinkPermission(LinkType linkType)
{
    try
    {
        switch (linkType)
        {
            case LinkType.Junction:
                // 目录联接通常不需要特殊权限
                return true;
            case LinkType.Symbolic:
                // 符号链接可能需要管理员权限或特定策略
                return CheckSymbolicLinkPermission();
            case LinkType.HardLink:
                // 硬链接通常不需要特殊权限
                return true;
            default:
                return false;
        }
    }
    catch
    {
        return false;
    }
}

private bool CheckSymbolicLinkPermission()
{
    try
    {
        // 检查是否具有创建符号链接的权限
        // 可以通过尝试创建一个临时符号链接来验证
        string tempDir = Path.GetTempPath();
        string tempSource = Path.Combine(tempDir, "temp_source");
        string tempLink = Path.Combine(tempDir, "temp_link");
        
        // 创建临时文件
        if (!File.Exists(tempSource))
        {
            File.WriteAllText(tempSource, "test");
        }
        
        // 尝试创建符号链接
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = "cmd.exe",
            Arguments = $"/c mklink "{tempLink}" "{tempSource}"",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true,
            WindowStyle = ProcessWindowStyle.Hidden
        };
        
        using (Process process = Process.Start(startInfo))
        {
            process.WaitForExit();
            
            // 清理临时文件
            if (File.Exists(tempLink)) File.Delete(tempLink);
            if (File.Exists(tempSource)) File.Delete(tempSource);
            
            // 检查是否成功创建
            return process.ExitCode == 0;
        }
    }
    catch
    {
        return false;
    }
}
```

## 6. 安全性和权限考虑

### 6.1 权限要求
- 创建符号链接通常需要管理员权限或特定用户权限
- 需要检查并提示用户权限不足的情况

### 6.2 安全检查
- 验证源路径和目标路径的有效性
- 防止路径遍历攻击
- 确保不会创建指向系统关键目录的链接
- 防止创建循环链接
- 验证目标路径不会覆盖重要文件

## 7. 测试策略

### 7.1 单元测试
- LinkService 功能测试
- 配置管理测试
- 路径验证测试
- 权限检查测试
- 链接创建、验证、删除测试

### 7.2 集成测试
- UI 交互测试
- 完整链接创建流程测试
- 配置保存和加载测试
- Tab页面切换测试
- 拖拽功能测试
- 进度显示测试

### 7.3 权限测试
- 不同权限级别下的功能测试
- 错误处理和提示测试
- 权限不足时的用户提示测试
- 管理员权限下的功能测试

## 8. 部署和兼容性

### 8.1 系统要求
- Windows Vista/Server 2008 或更高版本
- .NET Framework 4.7.2 或更高版本
- NTFS文件系统（符号链接和硬链接需要）

### 8.2 兼容性考虑
- 不同 Windows 版本的 MKLINK 命令兼容性
- 不同权限级别下的功能限制
- 不同文件系统（NTFS、FAT32等）对链接的支持差异
- 网络驱动器和映射驱动器的链接支持

## 9. 扩展性考虑

### 9.1 未来功能扩展
- 支持相对路径链接
- 批量链接管理
- 链接状态监控
- 链接批量操作（批量创建、验证、删除）
- 链接类型自动检测
- 链接目标路径批量修改

### 9.2 架构扩展性
- 插件化设计支持更多链接类型
- 命令行接口支持
- 与其他文件操作集成
- 支持网络路径链接
- 支持UNC路径链接
- 链接操作日志记录和审计