using FileHelper.Dialogs;
using FileHelper.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileHelper.Services
{
    public class LinkService
    {
        // 定义委托用于处理冲突
        public delegate LinkConflictDialog.ConflictAction ConflictHandler(string targetPath);
        
        // 事件用于处理冲突
        public event ConflictHandler OnConflict;
        
        // Windows API用于获取文件信息
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool GetFileInformationByHandle(IntPtr hFile, out BY_HANDLE_FILE_INFORMATION lpFileInformation);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr CreateFile(
            string lpFileName,
            uint dwDesiredAccess,
            uint dwShareMode,
            IntPtr lpSecurityAttributes,
            uint dwCreationDisposition,
            uint dwFlagsAndAttributes,
            IntPtr hTemplateFile);

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct BY_HANDLE_FILE_INFORMATION
        {
            public uint dwFileAttributes;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftCreationTime;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftLastAccessTime;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftLastWriteTime;
            public uint dwVolumeSerialNumber;
            public uint nFileSizeHigh;
            public uint nFileSizeLow;
            public uint dwNumberOfLinks;  // 链接数
            public uint nFileIndexHigh;
            public uint nFileIndexLow;
        }

        private const uint GENERIC_READ = 0x80000000;
        private const uint FILE_SHARE_READ = 0x00000001;
        private const uint FILE_SHARE_WRITE = 0x00000002;
        private const uint OPEN_EXISTING = 3;
        private const uint FILE_ATTRIBUTE_NORMAL = 0x80;
        
        public async Task<LinkResult> CreateLinksAsync(
            string sourcePath,
            List<string> targetPaths,
            LinkType linkType,
            IProgress<(int current, int total, string targetPath, LinkOperationStatus status)> progress)
        {
            var result = new LinkResult
            {
                TotalLinks = targetPaths.Count
            };

            try
            {
                int currentIndex = 0;

                foreach (string targetPath in targetPaths)
                {
                    currentIndex++;
                    progress?.Report((currentIndex, targetPaths.Count, targetPath, LinkOperationStatus.Creating));

                    try
                    {
                        // 检查目标路径是否已存在
                        if (Directory.Exists(targetPath) || File.Exists(targetPath))
                        {
                            // 触发冲突事件，获取用户选择
                            LinkConflictDialog.ConflictAction action = LinkConflictDialog.ConflictAction.Skip;
                            if (OnConflict != null)
                            {
                                action = OnConflict(targetPath);
                            }
                            else
                            {
                                // 如果没有注册事件处理程序，默认跳过
                                action = LinkConflictDialog.ConflictAction.Skip;
                            }

                            switch (action)
                            {
                                case LinkConflictDialog.ConflictAction.Replace:
                                    // 对于硬链接，需要特殊处理以避免文件丢失
                                    if (linkType == LinkType.HardLink && File.Exists(targetPath))
                                    {
                                        // 检查现有目标是否已经是硬链接
                                        if (IsHardLink(targetPath))
                                        {
                                            // 如果是硬链接，直接删除它
                                            progress?.Report((currentIndex, targetPaths.Count, targetPath, LinkOperationStatus.Deleting));
                                            await DeletePathAsync(targetPath);
                                        }
                                        else
                                        {
                                            // 如果不是硬链接，警告用户可能会删除原始文件
                                            var dialogResult = MessageBox.Show(
                                                $"目标文件 '{targetPath}' 不是硬链接，删除它可能会导致原始文件数据丢失。\n\n是否继续替换操作？",
                                                "硬链接替换警告",
                                                MessageBoxButtons.YesNo,
                                                MessageBoxIcon.Warning);
                                            
                                            if (dialogResult == DialogResult.Yes)
                                            {
                                                progress?.Report((currentIndex, targetPaths.Count, targetPath, LinkOperationStatus.Deleting));
                                                await DeletePathAsync(targetPath);
                                            }
                                            else
                                            {
                                                result.SkippedLinks++;
                                                progress?.Report((currentIndex, targetPaths.Count, targetPath, LinkOperationStatus.Skipped));
                                                continue; // 跳过当前链接的创建
                                            }
                                        }
                                    }
                                    else
                                    {
                                        // 对于其他类型的链接或目录，正常删除
                                        progress?.Report((currentIndex, targetPaths.Count, targetPath, LinkOperationStatus.Deleting));
                                        await DeletePathAsync(targetPath);
                                    }
                                    // 注意：这里不应该增加CreatedLinks计数器，因为删除操作不算是"创建"
                                    progress?.Report((currentIndex, targetPaths.Count, targetPath, LinkOperationStatus.Success));
                                    break;
                                case LinkConflictDialog.ConflictAction.Skip:
                                    result.SkippedLinks++;
                                    progress?.Report((currentIndex, targetPaths.Count, targetPath, LinkOperationStatus.Skipped));
                                    continue; // 跳过当前链接的创建
                                case LinkConflictDialog.ConflictAction.Cancel:
                                    result.FailedLinks++;
                                    progress?.Report((currentIndex, targetPaths.Count, targetPath, LinkOperationStatus.Failed));
                                    // 取消整个操作
                                    result.Success = false;
                                    result.ErrorMessage = "用户取消操作";
                                    return result;
                            }
                        }

                        // 确保目标目录存在
                        string targetDir = Path.GetDirectoryName(targetPath);
                        if (!string.IsNullOrEmpty(targetDir) && !Directory.Exists(targetDir))
                        {
                            Directory.CreateDirectory(targetDir);
                        }

                        // 创建链接
                        bool success = await CreateLinkAsync(sourcePath, targetPath, linkType);
                        if (success)
                        {
                            result.CreatedLinks++;
                            progress?.Report((currentIndex, targetPaths.Count, targetPath, LinkOperationStatus.Success));
                        }
                        else
                        {
                            result.FailedLinks++;
                            progress?.Report((currentIndex, targetPaths.Count, targetPath, LinkOperationStatus.Failed));
                        }
                    }
                    catch (Exception ex)
                    {
                        result.FailedLinks++;
                        progress?.Report((currentIndex, targetPaths.Count, targetPath, LinkOperationStatus.Failed));
                        // 记录错误但继续处理其他链接
                        System.Diagnostics.Debug.WriteLine($"创建链接失败 {targetPath}: {ex.Message}");
                    }
                }

                result.Success = result.FailedLinks == 0;
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
                return result;
            }
        }

        public async Task<LinkResult> ValidateLinksAsync(
            string sourcePath,
            List<string> targetPaths,
            LinkType linkType,
            IProgress<(int current, int total, string targetPath, LinkOperationStatus status)> progress)
        {
            var result = new LinkResult
            {
                TotalLinks = targetPaths.Count
            };

            try
            {
                int currentIndex = 0;

                foreach (string targetPath in targetPaths)
                {
                    currentIndex++;
                    progress?.Report((currentIndex, targetPaths.Count, targetPath, LinkOperationStatus.Validating));

                    try
                    {
                        bool isValid = await ValidateLinkAsync(sourcePath, targetPath, linkType);
                        if (isValid)
                        {
                            result.CreatedLinks++;
                            progress?.Report((currentIndex, targetPaths.Count, targetPath, LinkOperationStatus.Success));
                        }
                        else
                        {
                            result.FailedLinks++;
                            progress?.Report((currentIndex, targetPaths.Count, targetPath, LinkOperationStatus.Failed));
                        }
                    }
                    catch (Exception ex)
                    {
                        result.FailedLinks++;
                        progress?.Report((currentIndex, targetPaths.Count, targetPath, LinkOperationStatus.Failed));
                        // 提供更详细的错误信息
                        System.Diagnostics.Debug.WriteLine($"验证链接失败 {targetPath}: {ex.Message}\n详细信息: {ex}");
                    }
                }

                result.Success = result.FailedLinks == 0;
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = $"验证链接时发生错误: {ex.Message}\n详细信息: {ex}";
                return result;
            }
        }

        public async Task<LinkResult> DeleteLinksAsync(
            List<string> targetPaths,
            IProgress<(int current, int total, string targetPath, LinkOperationStatus status)> progress)
        {
            var result = new LinkResult
            {
                TotalLinks = targetPaths.Count
            };

            try
            {
                int currentIndex = 0;

                foreach (string targetPath in targetPaths)
                {
                    currentIndex++;
                    progress?.Report((currentIndex, targetPaths.Count, targetPath, LinkOperationStatus.Deleting));

                    try
                    {
                        // 检查路径是否存在
                        if (Directory.Exists(targetPath) || File.Exists(targetPath))
                        {
                            // 删除链接
                            await DeletePathAsync(targetPath);

                            // 删除操作应该增加DeletedLinks计数器，但由于LinkResult中没有这个字段，
                            // 我们使用CreatedLinks来表示成功处理的链接数量
                            result.CreatedLinks++;
                            progress?.Report((currentIndex, targetPaths.Count, targetPath, LinkOperationStatus.Success));
                        }
                        else
                        {
                            result.SkippedLinks++;
                            progress?.Report((currentIndex, targetPaths.Count, targetPath, LinkOperationStatus.Skipped));
                        }
                    }
                    catch (Exception ex)
                    {
                        result.FailedLinks++;
                        progress?.Report((currentIndex, targetPaths.Count, targetPath, LinkOperationStatus.Failed));
                        // 提供更详细的错误信息
                        System.Diagnostics.Debug.WriteLine($"删除链接失败 {targetPath}: {ex.Message}\n详细信息: {ex}");
                    }
                }

                result.Success = result.FailedLinks == 0;
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = $"删除链接时发生错误: {ex.Message}\n详细信息: {ex}";
                return result;
            }
        }

        private async Task DeletePathAsync(string path)
        {
            await Task.Run(() =>
            {
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }
                else if (File.Exists(path))
                {
                    File.Delete(path);
                }
            });
        }

        private async Task<bool> CreateLinkAsync(string sourcePath, string targetPath, LinkType linkType)
        {
            return await Task.Run(() =>
            {
                try
                {
                    // 检查源路径是否存在
                    if (!Directory.Exists(sourcePath) && !File.Exists(sourcePath))
                    {
                        throw new InvalidOperationException($"源路径不存在: {sourcePath}");
                    }

                    // 构建MKLINK命令
                    string arguments = GetMklinkArguments(sourcePath, targetPath, linkType);
                    string command = $"mklink {arguments}";

                    // 执行命令
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = $"/C {command}",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden
                    };

                    using (Process process = Process.Start(startInfo))
                    {
                        string output = process.StandardOutput.ReadToEnd();
                        string error = process.StandardError.ReadToEnd();
                        process.WaitForExit();

                        // 检查执行结果
                        if (process.ExitCode != 0)
                        {
                            // 提供更详细的错误信息
                            string detailedError = $"MKLINK命令执行失败\n命令: {command}\n退出代码: {process.ExitCode}\n输出: {output}\n错误: {error}";
                            throw new InvalidOperationException(detailedError);
                        }

                        return true;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"创建链接时出错: {ex.Message}");
                    // 记录更详细的异常信息
                    System.Diagnostics.Debug.WriteLine($"创建链接时详细错误: {ex}");
                    return false;
                }
            });
        }

        private async Task<bool> ValidateLinkAsync(string sourcePath, string targetPath, LinkType linkType)
        {
            return await Task.Run(() =>
            {
                try
                {
                    // 检查目标路径是否存在
                    bool exists = Directory.Exists(targetPath) || File.Exists(targetPath);
                    if (!exists)
                    {
                        return false;
                    }

                    // 对于符号链接和目录联接，可以检查链接目标
                    if (linkType == LinkType.Symbolic || linkType == LinkType.Junction)
                    {
                        // 这里可以添加更详细的验证逻辑
                        // 例如检查链接是否指向正确的源路径
                        return true;
                    }
                    else if (linkType == LinkType.HardLink)
                    {
                        // 硬链接验证比较复杂，简单检查文件是否存在
                        return File.Exists(targetPath);
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"验证链接时出错: {ex.Message}\n目标路径: {targetPath}\n源路径: {sourcePath}\n链接类型: {linkType}\n详细信息: {ex}");
                    return false;
                }
            });
        }

        private string GetMklinkArguments(string sourcePath, string targetPath, LinkType linkType)
        {
            switch (linkType)
            {
                case LinkType.Junction:
                    // 目录联接: mklink /J <链接> <目标>
                    return $"/J \"{targetPath}\" \"{sourcePath}\"";
                case LinkType.Symbolic:
                    // 符号链接: mklink /D <链接> <目标> (目录) 或 mklink <链接> <目标> (文件)
                    if (Directory.Exists(sourcePath))
                    {
                        return $"/D \"{targetPath}\" \"{sourcePath}\"";
                    }
                    else
                    {
                        return $"\"{targetPath}\" \"{sourcePath}\"";
                    }
                case LinkType.HardLink:
                    // 硬链接: mklink /H <链接> <目标> (仅文件)
                    return $"/H \"{targetPath}\" \"{sourcePath}\"";
                default:
                    throw new ArgumentException("不支持的链接类型", nameof(linkType));
            }
        }
        
        /// <summary>
        /// 检查文件是否为硬链接
        /// </summary>
        /// <param name="filePath">要检查的文件路径</param>
        /// <returns>如果是硬链接返回true，否则返回false</returns>
        private bool IsHardLink(string filePath)
        {
            IntPtr handle = IntPtr.Zero;
            try
            {
                // 使用CreateFile打开文件
                handle = CreateFile(
                    filePath,
                    GENERIC_READ,
                    FILE_SHARE_READ | FILE_SHARE_WRITE,
                    IntPtr.Zero,
                    OPEN_EXISTING,
                    FILE_ATTRIBUTE_NORMAL,
                    IntPtr.Zero);

                if (handle == IntPtr.Zero || handle == new IntPtr(-1))
                {
                    return false;
                }

                // 获取文件信息
                BY_HANDLE_FILE_INFORMATION fileInfo;
                if (GetFileInformationByHandle(handle, out fileInfo))
                {
                    // 如果链接数大于1，则为硬链接
                    return fileInfo.dwNumberOfLinks > 1;
                }
            }
            catch
            {
                // 如果出现任何异常，假设不是硬链接
            }
            finally
            {
                // 关闭文件句柄
                if (handle != IntPtr.Zero && handle != new IntPtr(-1))
                {
                    CloseHandle(handle);
                }
            }
            
            return false;
        }
        
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr hObject);
    }
}