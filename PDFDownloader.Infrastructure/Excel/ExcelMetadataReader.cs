using ClosedXML.Excel;
using PDFDownloader.Core.Interfaces;
using PDFDownloader.Core.Models;

namespace PDFDownloader.Infrastructure.Excel
{
    public class ExcelMetadataReader : IMetadataReader
    {
        private readonly string _filePath;

        public ExcelMetadataReader(string filePath)
        {
            _filePath = filePath;
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
            IEnumerable<IXLRangeRow> rows = worksheet.RangeUsed().RowsUsed().Skip(1);

            foreach (IXLRangeRow row in rows)
            {
                string brNummer = row.Cell("A").GetString().Trim();
                string primaryUrl = row.Cell("AL").GetString().Trim();
                string secondaryUrl = row.Cell("AM").GetString().Trim();

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
            }

            return Task.FromResult(reports);
        }
    }
}
