using FileCopyHelper.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileCopyHelper.Services
{
    public class ConfigurationService
    {
        private readonly string _configDirectory;
        private readonly string _configExtension = ".json";

        public ConfigurationService()
        {
            // 使用程序当前目录作为配置文件存储位置
            _configDirectory = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Configurations");

            // 确保配置目录存在
            if (!Directory.Exists(_configDirectory))
            {
                Directory.CreateDirectory(_configDirectory);
            }
        }

        public bool SaveConfiguration(CopyConfiguration config)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(config.Name))
                {
                    return false;
                }

                // 清理文件名中的无效字符
                string safeFileName = GetSafeFileName(config.Name);
                string filePath = Path.Combine(_configDirectory, safeFileName + _configExtension);

                config.LastModified = DateTime.Now;
                string json = config.ToJson();

                File.WriteAllText(filePath, json);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"保存配置失败: {ex.Message}");
                return false;
            }
        }

        public CopyConfiguration LoadConfiguration(string configName)
        {
            try
            {
                string safeFileName = GetSafeFileName(configName);
                string filePath = Path.Combine(_configDirectory, safeFileName + _configExtension);

                if (!File.Exists(filePath))
                {
                    return null;
                }

                string json = File.ReadAllText(filePath);
                return CopyConfiguration.FromJson(json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"加载配置失败: {ex.Message}");
                return null;
            }
        }

        public List<string> GetConfigurationNames()
        {
            try
            {
                if (!Directory.Exists(_configDirectory))
                {
                    return new List<string>();
                }

                return Directory.GetFiles(_configDirectory, "*" + _configExtension)
                    .Select(file => Path.GetFileNameWithoutExtension(file))
                    .OrderBy(name => name)
                    .ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"获取配置列表失败: {ex.Message}");
                return new List<string>();
            }
        }

        public bool DeleteConfiguration(string configName)
        {
            try
            {
                string safeFileName = GetSafeFileName(configName);
                string filePath = Path.Combine(_configDirectory, safeFileName + _configExtension);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"删除配置失败: {ex.Message}");
                return false;
            }
        }

        public List<CopyConfiguration> GetAllConfigurations()
        {
            var configurations = new List<CopyConfiguration>();

            try
            {
                var configNames = GetConfigurationNames();
                foreach (string configName in configNames)
                {
                    var config = LoadConfiguration(configName);
                    if (config != null)
                    {
                        configurations.Add(config);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"获取所有配置失败: {ex.Message}");
            }

            return configurations.OrderByDescending(c => c.LastModified).ToList();
        }

        public bool RenameConfiguration(string oldName, string newName)
        {
            try
            {
                var config = LoadConfiguration(oldName);
                if (config == null)
                {
                    return false;
                }

                config.Name = newName;
                if (SaveConfiguration(config))
                {
                    DeleteConfiguration(oldName);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"重命名配置失败: {ex.Message}");
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

        public string GetConfigurationPath(string configName)
        {
            string safeFileName = GetSafeFileName(configName);
            return Path.Combine(_configDirectory, safeFileName + _configExtension);
        }

        public bool ConfigurationExists(string configName)
        {
            string filePath = GetConfigurationPath(configName);
            return File.Exists(filePath);
        }
    }
}