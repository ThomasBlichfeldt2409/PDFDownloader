using PDFDownloader.Core.Config;

namespace PDFDownloader.Config
{
    public class AppSettings
    {
        // The property names Excel, Paths, Download must match the keys in the appsettings.json
        public ExcelConfig Excel { get; set; } = new ExcelConfig();
        public PathsConfig Paths { get; set; } = new PathsConfig();
        public DownloadConfig Download { get; set; } = new DownloadConfig();
    }
}
