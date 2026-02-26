using Microsoft.Extensions.Configuration;
using PDFDownloader.Config;
using PDFDownloader.Core.Interfaces;
using PDFDownloader.Infrastructure.Excel;
using PDFDownloader.Infrastructure.Download;
using PDFDownloader.Infrastructure.Storage;
using PDFDownloader.Core.Services;

// Builds the configuration object
IConfigurationRoot configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

// Creates an Appsettings object
// It matches the json keys to the property names in AppSettings.cs
// It matches the inner json keys to the property names inside each class in the AppSettings.cs
AppSettings? settings = configuration.Get<AppSettings>();

if (settings == null)
    throw new Exception("Failed to load configuration.");

// Fetching configurations from appsettings.json
string excelPath = Path.Combine(Directory.GetCurrentDirectory(), settings.Paths.ExcelFile);
string reportFolder = Path.Combine(Directory.GetCurrentDirectory(), settings.Paths.ReportOutputFolder);
int maxConcurrency = settings.Download.MaxConcurrency;

// Creating Infrastrucure
IMetadataReader metadataReader = new ExcelMetadataReader(excelPath, settings.Excel);
IReportDownloader reportDownloader = new HttpReportDownloader();
IResultWriter resultWriter = new JsonResultWriter();

// Injecting Service with the created infrastructure
ReportDownloadService reportDownloadService = new ReportDownloadService(metadataReader, reportDownloader, resultWriter, reportFolder, maxConcurrency);

await reportDownloadService.ExecuteAsync();