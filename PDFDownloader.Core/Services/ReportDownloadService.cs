using PDFDownloader.Core.Interfaces;
using PDFDownloader.Core.Models;

namespace PDFDownloader.Core.Services
{
    public class ReportDownloadService
    {
        private readonly IMetadataReader _metadataReader;
        private readonly IReportDownloader _reportDownloader;
        private readonly IResultWriter _resultWriter;

        public ReportDownloadService(IMetadataReader metadataReader, IReportDownloader reportDownloader, IResultWriter resultWriter)
        {
            _metadataReader = metadataReader;
            _reportDownloader = reportDownloader;
            _resultWriter = resultWriter;
        }

        public async Task ExecuteAsync()
        {
            // Read metadata

            // Loop through each report
            
            // Write results
        }
    }
}
