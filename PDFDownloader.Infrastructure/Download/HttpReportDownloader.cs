using PDFDownloader.Core.Interfaces;

namespace PDFDownloader.Infrastructure.Download
{
    public class HttpReportDownloader : IReportDownloader
    {
        public Task<bool> DownloadAsync(string url, string filePath)
        {
            return Task.FromResult(true);
        }
    }
}
