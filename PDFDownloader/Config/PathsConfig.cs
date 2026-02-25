namespace PDFDownloader.Config
{
    public class PathsConfig
    {
        // The Property names must match the properties for the inner Paths key
        public string ExcelFile { get; set; } = string.Empty;
        public string ReportOutputFolder { get; set; } = string.Empty;
        public string ResultFile { get; set;  } = string.Empty;
    }
}