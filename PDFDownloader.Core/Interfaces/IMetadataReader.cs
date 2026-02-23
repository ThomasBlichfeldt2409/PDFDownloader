using PDFDownloader.Core.Models;

namespace PDFDownloader.Core.Interfaces
{
    public interface IMetadataReader
    {
        Task<List<ReportMetadata>> ReadAsync();
    }
}
