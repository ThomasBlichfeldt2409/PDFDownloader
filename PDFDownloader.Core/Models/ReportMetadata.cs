namespace PDFDownloader.Core.Models
{
    public class ReportMetadata
    {
        public string BRNummer { get; set; } = string.Empty;
        public string PrimaryUrl { get; set; } = string.Empty;
        public string? SecondaryUrl { get; set; }
    }
}
