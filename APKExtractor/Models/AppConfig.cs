using System;
using System.IO;
using System.Text;

namespace APKExtractor.Models
{
    /// <summary>
    /// 应用配置（工具路径等），持久化到config.json
    /// </summary>
    public class AppConfig
    {
        /// <summary>ADB可执行文件路径（空则自动查找）</summary>
        public string AdbPath { get; set; }

        /// <summary>aapt可执行文件路径（空则自动查找）</summary>
        public string AaptPath { get; set; }

        /// <summary>默认导出目录（空则使用程序目录下APK_Export）</summary>
        public string ExportPath { get; set; }

        /// <summary>界面语言（zh=中文，en=英文）</summary>
        public string Language { get; set; } = "zh";

        private static readonly string ConfigFile = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "config.json");

        /// <summary>
        /// 加载配置文件，如果不存在则返回空配置
        /// </summary>
        public static AppConfig Load()
        {
            try
            {
                if (File.Exists(ConfigFile))
                {
                    string json = File.ReadAllText(ConfigFile, Encoding.UTF8);
                    return SimpleDeserialize(json);
                }
            }
            catch { }

            return new AppConfig();
        }

        /// <summary>
        /// 保存配置到文件
        /// </summary>
        public void Save()
        {
            try
            {
                string json = SimpleSerialize();
                File.WriteAllText(ConfigFile, json, Encoding.UTF8);
            }
            catch { }
        }

        // 简易JSON序列化（避免引入额外依赖）
        private string SimpleSerialize()
        {
            var sb = new StringBuilder();
            sb.AppendLine("{");
            sb.AppendLine($"  \"AdbPath\": {EscapeJson(AdbPath)},");
            sb.AppendLine($"  \"AaptPath\": {EscapeJson(AaptPath)},");
            sb.AppendLine($"  \"ExportPath\": {EscapeJson(ExportPath)},");
            sb.AppendLine($"  \"Language\": {EscapeJson(Language)}");
            sb.AppendLine("}");
            return sb.ToString();
        }

        private static string EscapeJson(string s)
        {
            if (string.IsNullOrEmpty(s))
                return "\"\"";
            // 转义反斜杠和引号
            return "\"" + s.Replace("\\", "\\\\").Replace("\"", "\\\"") + "\"";
        }

        private static AppConfig SimpleDeserialize(string json)
        {
            var config = new AppConfig();
            config.AdbPath = ExtractValue(json, "AdbPath");
            config.AaptPath = ExtractValue(json, "AaptPath");
            config.ExportPath = ExtractValue(json, "ExportPath");
            config.Language = ExtractValue(json, "Language");
            
            // Default to Chinese if language is not set
            if (string.IsNullOrEmpty(config.Language))
            {
                config.Language = "zh";
            }
            
            return config;
        }

        private static string ExtractValue(string json, string key)
        {
            string pattern = "\"" + key + "\"";
            int idx = json.IndexOf(pattern, StringComparison.OrdinalIgnoreCase);
            if (idx < 0) return null;

            // 找到冒号后的值
            int colonIdx = json.IndexOf(':', idx + pattern.Length);
            if (colonIdx < 0) return null;

            // 找到第一个引号
            int start = json.IndexOf('"', colonIdx + 1);
            if (start < 0) return null;

            // 找到结束引号（处理转义）
            var sb = new StringBuilder();
            int i = start + 1;
            while (i < json.Length)
            {
                char c = json[i];
                if (c == '\\' && i + 1 < json.Length)
                {
                    char next = json[i + 1];
                    if (next == '"') { sb.Append('"'); i += 2; continue; }
                    if (next == '\\') { sb.Append('\\'); i += 2; continue; }
                }
                if (c == '"') break;
                sb.Append(c);
                i++;
            }
            return sb.ToString();
        }
    }
}
