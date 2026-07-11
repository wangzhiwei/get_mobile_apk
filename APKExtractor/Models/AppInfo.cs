namespace APKExtractor.Models
{
    /// <summary>
    /// 应用信息
    /// </summary>
    public class AppInfo
    {
        public string PackageName { get; set; }
        public string AppName { get; set; }
        public string ApkPath { get; set; }
        public string VersionName { get; set; }
        public string VersionCode { get; set; }
        public bool IsSystemApp { get; set; }

        public override string ToString()
        {
            return $"{AppName} ({PackageName})";
        }
    }
}
