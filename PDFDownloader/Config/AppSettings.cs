namespace PDFDownloader.Config
{
    public class AppSettings
    {
        public ExcelConfig Excel { get; set; } = new ExcelConfig();
        public PathsConfig Paths { get; set; } = new PathsConfig();
        public DownloadConfig Download { get; set; } = new DownloadConfig();
    }
}
