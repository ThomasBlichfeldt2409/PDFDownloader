using PDFDownloader.Core.Interfaces;
using PDFDownloader.Core.Models;
using System.Runtime.InteropServices;

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
            List<ReportMetadata> reports = await _metadataReader.ReadAsync();

            Console.WriteLine($"Read {reports.Count} reports");
        }
    }
}
