using PDFDownloader.Core.Models;

namespace PDFDownloader.Core.Interfaces
{
    public interface IResultWriter
    {
        Task WriteAsync(List<DownloadResult> results);   
    }
}
