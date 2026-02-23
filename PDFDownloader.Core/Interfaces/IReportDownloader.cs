namespace PDFDownloader.Core.Interfaces
{
    public interface IReportDownloader
    {
        Task<bool> DownloadAsync(string url, string filePath);
    }
}
