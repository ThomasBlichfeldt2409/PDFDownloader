using DocumentFormat.OpenXml.Spreadsheet;
using PDFDownloader.Core.Interfaces;
using PDFDownloader.Core.Models;
using System.Text.Json;

namespace PDFDownloader.Infrastructure.Storage
{
    public class JsonResultWriter : IResultWriter
    {
        private readonly string _resultFilePath;

        public JsonResultWriter(string resultFilePath)
        {
            _resultFilePath = resultFilePath;
        }

        public async Task WriteAsync(List<DownloadResult> results)
        {
            Console.WriteLine("Writes results to JSON...");

            // Ensure directory exists
            string? directory = Path.GetDirectoryName(_resultFilePath);

            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Configure JSON formatting
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            // Serialize and write to file
            await using FileStream stream = File.Create(_resultFilePath);
            await JsonSerializer.SerializeAsync(stream, results, options);
        }
    }
}
