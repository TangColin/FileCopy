using FileCopyHelper.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileCopyHelper.Services
{
    public class FileCopyService
    {
        private const int BUFFER_SIZE = 1024 * 1024; // 1MB buffer for better performance
        
        // 用于跟踪文件冲突处理状态
        private static bool _hasShownConflictDialog = false;
        private static ConflictAction _globalConflictAction = ConflictAction.Cancel;

        public async Task<CopyResult> CopyFilesAsync(
            string sourcePath,
            List<string> targetPaths,
            List<string> blackList,
            IProgress<(int current, int total, string currentFile, string targetFile, long currentBytes, long totalBytes)> progress,
            CancellationToken cancellationToken)
        {
            var result = new CopyResult();

            try
            {
                // 获取所有要复制的文件
                var allFiles = GetFilesToCopy(sourcePath, blackList);
                // 统计所有目录路径的文件数量（源文件数量乘以目标路径数量）
                result.TotalFiles = allFiles.Count * targetPaths.Count;
                result.TotalBytes = allFiles.Sum(f => new FileInfo(f).Length) * targetPaths.Count;

                int currentIndex = 0;
                long copiedBytes = 0;

                foreach (string sourceFile in allFiles)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    string relativePath = GetRelativePath(sourcePath, sourceFile);
                    long fileSize = new FileInfo(sourceFile).Length;

                    // 报告进度（显示当前文件，在进入目标循环前）
                    progress?.Report((currentIndex, result.TotalFiles, sourceFile, "", copiedBytes, result.TotalBytes));

                    int targetsCopied = 0; // 记录成功复制到的目标数量

                    foreach (string targetPath in targetPaths)
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        // 报告正在复制到哪个目标的进度
                        progress?.Report((currentIndex, result.TotalFiles, sourceFile, targetPath, copiedBytes, result.TotalBytes));

                        string targetFile = Path.Combine(targetPath, relativePath);
                        string targetDir = Path.GetDirectoryName(targetFile);

                        try
                        {
                            // 确保目标目录存在
                            if (!string.IsNullOrEmpty(targetDir) && !Directory.Exists(targetDir))
                            {
                                Directory.CreateDirectory(targetDir);
                            }

                            // 检查文件冲突
                            if (File.Exists(targetFile))
                            {
                                var conflictAction = await HandleFileConflict(sourceFile, targetFile);
                                  
                                switch (conflictAction)
                                {
                                    case ConflictAction.Skip:
                                        // 注意：这里不增加SkippedFiles计数，因为文件可能会被复制到其他目标路径
                                        continue;
                                    case ConflictAction.Cancel:
                                        result.ErrorMessage = "用户取消操作";
                                        return result;
                                    case ConflictAction.Replace:
                                        break; // 继续复制，覆盖文件
                                }
                            }

                            // 复制文件
                        await CopyFileAsync(sourceFile, targetFile, cancellationToken);
                        targetsCopied++;
                        copiedBytes += fileSize;
                        result.CopiedBytes = copiedBytes;
                        currentIndex++;
                        }
                        catch (Exception ex)
                        {
                            result.ErrorMessage = $"复制文件 {sourceFile} 到 {targetFile} 时出错: {ex.Message}";
                            return result;
                        }
                    }

                    // 增加成功复制到的目标数量到CopiedFiles
                    result.CopiedFiles += targetsCopied;
                    
                    // 增加被跳过的目标数量到SkippedFiles
                    result.SkippedFiles += targetPaths.Count - targetsCopied;

                    // 对于被跳过的目标，我们也需要更新currentIndex以保持进度条的连续性
                    currentIndex += (targetPaths.Count - targetsCopied);
                }

                result.Success = true;
                // 复制完成时的进度报告（使用空字符串作为目标路径）
                progress?.Report((result.TotalFiles, result.TotalFiles, "", "", result.TotalBytes, result.TotalBytes));
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

        private List<string> GetFilesToCopy(string sourcePath, List<string> blackList)
        {
            var files = new List<string>();
            var patterns = blackList.Where(p => !string.IsNullOrWhiteSpace(p)).ToList();

            try
            {
                GetFilesRecursively(sourcePath, files, patterns);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"获取文件列表时出错: {ex.Message}", ex);
            }

            return files;
        }

        private void GetFilesRecursively(string directory, List<string> files, List<string> blackListPatterns)
        {
            try
            {
                // 检查目录是否在黑名单中
                string dirName = Path.GetFileName(directory);
                if (IsInBlackList(dirName, blackListPatterns) || IsInBlackList(directory, blackListPatterns))
                {
                    return;
                }

                // 获取所有文件
                var allFiles = Directory.GetFiles(directory);
                foreach (string file in allFiles)
                {
                    string fileName = Path.GetFileName(file);
                    if (!IsInBlackList(fileName, blackListPatterns) && !IsInBlackList(file, blackListPatterns))
                    {
                        files.Add(file);
                    }
                }

                // 递归处理子目录
                var subdirectories = Directory.GetDirectories(directory);
                foreach (string subdirectory in subdirectories)
                {
                    GetFilesRecursively(subdirectory, files, blackListPatterns);
                }
            }
            catch (UnauthorizedAccessException)
            {
                // 跳过无权限访问的目录
            }
            catch (DirectoryNotFoundException)
            {
                // 跳过不存在的目录
            }
        }

        private bool IsInBlackList(string itemName, List<string> blackListPatterns)
        {
            foreach (string pattern in blackListPatterns)
            {
                if (string.IsNullOrWhiteSpace(pattern))
                    continue;

                try
                {
                    // 支持通配符匹配
                    if (pattern.Contains('*') || pattern.Contains('?'))
                    {
                        if (MatchesWildcard(itemName, pattern))
                            return true;
                    }
                    else
                    {
                        // 精确匹配
                        if (itemName.Equals(pattern, StringComparison.OrdinalIgnoreCase))
                            return true;
                    }
                }
                catch
                {
                    // 忽略无效的模式
                }
            }

            return false;
        }

        private bool MatchesWildcard(string text, string pattern)
        {
            // 简单的通配符匹配实现
            pattern = pattern.Replace("*", ".*").Replace("?", ".");
            return System.Text.RegularExpressions.Regex.IsMatch(text, "^" + pattern + "$", 
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        }

        private async Task<ConflictAction> HandleFileConflict(string sourceFile, string targetFile)
        {
            // 如果已经显示过冲突对话框，直接返回之前的选择
            if (_hasShownConflictDialog)
            {
                return _globalConflictAction;
            }

            // 在UI线程上显示冲突对话框
            ConflictAction action = ConflictAction.Cancel;
            
            if (Application.OpenForms.Count > 0)
            {
                var mainForm = Application.OpenForms[0];
                if (mainForm != null && mainForm.InvokeRequired)
                {
                    await Task.Run(() =>
                    {
                        mainForm.Invoke(new Action(() =>
                        {
                            using (var dialog = new ConflictDialog(sourceFile, targetFile))
                            {
                                dialog.ShowDialog(mainForm);
                                action = dialog.SelectedAction;
                                
                                // 记住用户的选择
                                _globalConflictAction = action;
                                _hasShownConflictDialog = true;
                            }
                        }));
                    });
                }
                else if (mainForm != null)
                {
                    using (var dialog = new ConflictDialog(sourceFile, targetFile))
                    {
                        dialog.ShowDialog(mainForm);
                        action = dialog.SelectedAction;
                        
                        // 记住用户的选择
                        _globalConflictAction = action;
                        _hasShownConflictDialog = true;
                    }
                }
            }

            return action;
        }
        
        // 重置冲突对话框状态（供外部调用，例如每次复制操作开始时）
        public static void ResetConflictDialogState()
        {
            _hasShownConflictDialog = false;
            _globalConflictAction = ConflictAction.Cancel;
        }

        private async Task CopyFileAsync(string sourceFile, string targetFile, CancellationToken cancellationToken)
        {
            using var sourceStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read, FileShare.Read);
            using var targetStream = new FileStream(targetFile, FileMode.Create, FileAccess.Write, FileShare.None);

            var buffer = new byte[BUFFER_SIZE];
            int bytesRead;

            while ((bytesRead = await sourceStream.ReadAsync(buffer, 0, buffer.Length, cancellationToken)) > 0)
            {
                await targetStream.WriteAsync(buffer, 0, bytesRead, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
            }

            // 保持文件的时间戳
            try
            {
                var sourceInfo = new FileInfo(sourceFile);
                var targetInfo = new FileInfo(targetFile);
                targetInfo.CreationTime = sourceInfo.CreationTime;
                targetInfo.LastWriteTime = sourceInfo.LastWriteTime;
                targetInfo.LastAccessTime = sourceInfo.LastAccessTime;
            }
            catch
            {
                // 忽略时间戳设置错误
            }
        }

        private string GetRelativePath(string fromPath, string toPath)
        {
            Uri fromUri = new Uri(AppendDirectorySeparatorChar(fromPath));
            Uri toUri = new Uri(toPath);

            if (fromUri.Scheme != toUri.Scheme)
            {
                return toPath;
            }

            Uri relativeUri = fromUri.MakeRelativeUri(toUri);
            string relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            return relativePath.Replace('/', Path.DirectorySeparatorChar);
        }

        private string AppendDirectorySeparatorChar(string path)
        {
            if (!Path.HasExtension(path) &&
                !path.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                return path + Path.DirectorySeparatorChar;
            }

            return path;
        }
    }
}