using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace FileHelper.Models
{
    public class CopyConfiguration
    {
        public string Name { get; set; } = string.Empty;
        public string SourcePath { get; set; } = string.Empty;
        public List<string> TargetPaths { get; set; } = new List<string>();
        public List<string> BlackList { get; set; } = new List<string>();
        public DateTime LastModified { get; set; } = DateTime.Now;

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public static CopyConfiguration FromJson(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<CopyConfiguration>(json);
            }
            catch
            {
                return null;
            }
        }
    }

    public enum ConflictAction
    {
        Replace,
        Skip,
        Cancel
    }

    public class CopyResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public int TotalFiles { get; set; }
        public int CopiedFiles { get; set; }
        public int SkippedFiles { get; set; }
        public long TotalBytes { get; set; }
        public long CopiedBytes { get; set; }
    }
}