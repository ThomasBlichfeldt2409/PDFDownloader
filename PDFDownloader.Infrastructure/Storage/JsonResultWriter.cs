using PDFDownloader.Core.Interfaces;
using PDFDownloader.Core.Models;

namespace PDFDownloader.Infrastructure.Storage
{
    public class JsonResultWriter : IResultWriter
    {
        public Task WriteAsync(List<DownloadResult> results)
        {
            return Task.FromResult(new List<DownloadResult>()); 
        }
    }
}
