using PDFDownloader.Core.Models;

namespace PDFDownloader.Core.Interfaces
{
    public interface IMetadataReader
    {
        // Uses Task to make ReadAsync future-proof, async await can be used for this method
        Task<List<ReportMetadata>> ReadAsync();
    }
}
