using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace FileHelper.Models
{
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
        Junction,    // 目录联接
        Symbolic,    // 符号链接
        HardLink     // 硬链接
    }

    public class LinkResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public int TotalLinks { get; set; }
        /// <summary>
        /// 成功处理的链接数量（包括创建、验证成功、删除成功等操作）
        /// </summary>
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
}