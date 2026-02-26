using ClosedXML.Excel;
using PDFDownloader.Core.Config;
using PDFDownloader.Core.Interfaces;
using PDFDownloader.Core.Models;

namespace PDFDownloader.Infrastructure.Excel
{
    public class ExcelMetadataReader : IMetadataReader
    {
        private readonly string _filePath;
        private readonly ExcelConfig _excelConfig;

        public ExcelMetadataReader(string filePath, ExcelConfig excelConfig)
        {
            _filePath = filePath;
            _excelConfig = excelConfig;
        }

        public Task<List<ReportMetadata>> ReadAsync()
        {
            List<ReportMetadata> reports = new List<ReportMetadata>();

            // Opens the Excel file, using ensure to dispose after finishing 
            using XLWorkbook workbook = new XLWorkbook(_filePath);

            // Gets the first sheet in the workbook
            IXLWorksheet worksheet = workbook.Worksheet(1);

            // RangeUsed() -> Returns the area that contains data
            // RowUsed() -> Returns only rows that contains data
            // Skip(1) -> Skips the first row (header row)
            IEnumerable<IXLRangeRow> rows = worksheet.RangeUsed().RowsUsed().Skip(_excelConfig.StartRow - 1);

            Console.WriteLine("Starting fething URL's...");
            int completedRows = 0;
            int totalRows = rows.Count();

            foreach (IXLRangeRow row in rows)
            {
                string brNummer = row.Cell(_excelConfig.BRNummerColumn).GetString().Trim();
                string primaryUrl = row.Cell(_excelConfig.PrimaryUrlColumn).GetString().Trim();
                string secondaryUrl = row.Cell(_excelConfig.SecondaryUrlColumn).GetString().Trim();

                if (string.IsNullOrWhiteSpace(brNummer))
                    continue;

                // If there is no secondaryUrl in the Excel worksheet
                // SecondaryUrl will be null, else it will be secondaryUrl
                reports.Add(new ReportMetadata
                {
                    BRNummer = brNummer,
                    PrimaryUrl = primaryUrl,
                    SecondaryUrl = string.IsNullOrWhiteSpace(secondaryUrl) ? null : secondaryUrl
                });

                Console.CursorLeft = 0;
                completedRows++;
                int percent = (int)((double)completedRows / totalRows * 100);

                Console.CursorLeft = 0;
                Console.Write($"Downloading PDF Process: {percent}% ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(new string('|', percent / 2));
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(new string('|', 50 - percent / 2));
            }

            return Task.FromResult(reports);
        }
    }
}
