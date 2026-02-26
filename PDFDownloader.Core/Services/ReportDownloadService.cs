using PDFDownloader.Core.Interfaces;
using PDFDownloader.Core.Models;
using System.Diagnostics;

namespace PDFDownloader.Core.Services
{
    public class ReportDownloadService
    {
        private readonly IMetadataReader _metadataReader;
        private readonly IReportDownloader _reportDownloader;
        private readonly IResultWriter _resultWriter;
        private readonly string _reportOutputFolder;
        private readonly int _maxConcurrency;

        private readonly object _resultsLock = new object();
        private readonly object _progressLock = new object();

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

            // Concurrency Controller - max of _maxConcurrency at a time
            using SemaphoreSlim semaphore = new SemaphoreSlim(_maxConcurrency);

            // Storage for all running tasks
            List<Task> tasks = new List<Task>();

            int completedCount = 0;
            int totalCount = reports.Count;

            Console.WriteLine("Starting Downloading...");

            foreach (ReportMetadata report in reports)
            {
                // Wait for available slot
                await semaphore.WaitAsync();

                // Creates task
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

                        // Add result - lock ensures only one thread may enter this block at a time
                        lock (_resultsLock)
                        {
                            results.Add(new DownloadResult
                            {
                                BRNummer = report.BRNummer,
                                IsDownloaded = isDownloaded
                            });
                        }

                        // Update progress bar - lock ensures only one thread may enter this block at a time
                        lock (_progressLock)
                        { 
                            completedCount++;
                            int percent = (int)((double)completedCount / reports.Count * 100);

                            Console.CursorLeft = 0;
                            Console.Write($"Downloading PDF Process: {percent}% ");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(new string('|', percent / 2));
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write(new string('|', 50 - percent / 2));
                        }
                    }
                    finally
                    {
                        // Release task to make room for new one
                        semaphore.Release();
                    }
                });

                tasks.Add(task);
            }

            // Waits until every download is finished
            await Task.WhenAll(tasks);
            Console.WriteLine();

            // Create JSON file with results
            await _resultWriter.WriteAsync(results);

            Console.WriteLine("Succesfully writes results to JSON.");
        }
    }
}
