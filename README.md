# PDFDownloader
A .NET console application for downloading PDF reports in parallel from URLs stored in an Excel file, 
with configurable concurrency and JSON result logging.

# Description
PDFDownloader is a .NET console application designed to automate the download of PDF reports based on 
metadata stored in an Excel file.

The application:
•	Reads report metadata (BR number, Primary URL, secondary URL) from Excel
•	Attempts to download each report as a PDF
•	Falls back to a secondary URL if the primary URL fails
•	Downloads files in parallel
•	Displays real-time progress in the console
•	Writes a JSON summary of the download results

The solution follows this architecture
•	Core – Interfaces, Models, and logic
•	Infrastructure – Excel reader, HTTP downloader, JSON writer
•	Console App – Entry point and configurations

# Dependencies
Before running the application, ensure you have:
•	.NET SDK 10.0 or newer
•	Windows 10 / Windows 11
•	Visual Studio 2022

NuGet packages used:
•	Console application:
	•	Microsoft.Extensions.Configuration 10.0.3
	•	Microsoft.Extensions.Configuration.Binder 10.0.3
	•	Microsoft.Extensions.Configuration.Json 10.0.3

•	Infrastructure - Class library:
	•	ClosedXML 0.105.0

# Installing
1. Clone the repository:
	git clone https://github.com/your-username/PDFDownloader.git

2. Open the solution in Visual Studio:
	PDFDownloader.sln

3. Restore NuGet packages:
	dotnet restore

4. Ensure configuration are setup correctly:
	Open and modify if needed appsettings.json
	Make sure Excel file exists in the specified location


# Executing the program
Set PDFDownloader as Startup Project in Visual Studio
Press F5 or click Run

# Help
MaxConcurrency from appsettings.json is recommended to be in the range 5-20

# Author
Thomas
GitHub: ThomasBlichfeldt2409
