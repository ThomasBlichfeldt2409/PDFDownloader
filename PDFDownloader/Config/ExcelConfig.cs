namespace PDFDownloader.Config
{
    public class ExcelConfig
    {
        public string BRNummerColumn { get; set; } = string.Empty;
        public string PrimaryUrlColumn { get; set; } = string.Empty;
        public string SecondaryColumn { get; set; } = string.Empty;
        public int StartRow { get; set; }
    }
}
