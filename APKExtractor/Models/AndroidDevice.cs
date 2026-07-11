namespace APKExtractor.Models
{
    /// <summary>
    /// 安卓设备信息
    /// </summary>
    public class AndroidDevice
    {
        public string SerialNumber { get; set; }
        public string Model { get; set; }
        public string State { get; set; }
        public string AndroidVersion { get; set; }
        public string Manufacturer { get; set; }

        public override string ToString()
        {
            return $"{Model} ({SerialNumber})";
        }
    }
}
