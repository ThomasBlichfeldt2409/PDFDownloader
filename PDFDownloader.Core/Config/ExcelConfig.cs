namespace PDFDownloader.Core.Config
{
    public class ExcelConfig
    {
        // The Property names must match the property keys for the inner Excel key
        public string BRNummerColumn { get; set; } = string.Empty;
        public string PrimaryUrlColumn { get; set; } = string.Empty;
        public string SecondaryColumn { get; set; } = string.Empty;
        public int StartRow { get; set; }
    }
}
