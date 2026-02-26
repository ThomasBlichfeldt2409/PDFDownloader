using PDFDownloader.Core.Interfaces;
using PDFDownloader.Core.Models;

namespace PDFDownloader.Core.Services
{
    public class ReportDownloadService
    {
        private readonly IMetadataReader _metadataReader;
        private readonly IReportDownloader _reportDownloader;
        private readonly IResultWriter _resultWriter;
        private readonly string _reportOutputFolder;
        private readonly int _maxConcurrency;

        public ReportDownloadService(
            IMetadataReader metadataReader, 
            IReportDownloader reportDownloader, 
            IResultWriter resultWriter, 
            string reportOutputFolder, 
            int maxConcurrency)
        {
            _metadataReader = metadataReader;
            _reportDownloader = reportDownloader;
            _resultWriter = resultWriter;
            _reportOutputFolder = reportOutputFolder;
            _maxConcurrency = maxConcurrency;
        }

        public async Task ExecuteAsync()
        {
            // Ensure output folder exists
            Directory.CreateDirectory(_reportOutputFolder);

            // Read URL's from the data
            List<ReportMetadata> reports = await _metadataReader.ReadAsync();
            List<DownloadResult> results = new List<DownloadResult>();

            using SemaphoreSlim semaphore = new SemaphoreSlim(_maxConcurrency);

            List<Task> tasks = new List<Task>();

            foreach (ReportMetadata report in reports)
            {
                await semaphore.WaitAsync();

                Task task = Task.Run(async () =>
                {
                    try
                    {
                        bool isDownloaded = false;

                        string filePath = Path.Combine(_reportOutputFolder, $"{report.BRNummer}.pdf");

                        // Try Primary URL
                        if (!string.IsNullOrWhiteSpace(report.PrimaryUrl))
                        {
                            isDownloaded = await _reportDownloader.DownloadAsync(report.PrimaryUrl, filePath);
                        }

                        // if Primary URL failed, try Secondary URL
                        if (!string.IsNullOrWhiteSpace(report.SecondaryUrl) && !isDownloaded)
                        {
                            isDownloaded = await _reportDownloader.DownloadAsync(report.SecondaryUrl, filePath);
                        }

                        lock (results)
                        {
                            results.Add(new DownloadResult
                            {
                                BRNummer = report.BRNummer,
                                IsDownloaded = isDownloaded
                            });
                        }
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                });

                tasks.Add(task);
            }

            await Task.WhenAll(tasks);
        }
    }
}
