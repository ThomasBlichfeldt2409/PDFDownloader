using PDFDownloader.Core.Interfaces;

namespace PDFDownloader.Infrastructure.Download
{
    public class HttpReportDownloader : IReportDownloader
    {
        private readonly HttpClient _httpClient;

        public HttpReportDownloader()
        {
            _httpClient = new HttpClient();
        }

        public async Task<bool> DownloadAsync(string url, string filePath)
        {
            try
            {
                using HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    return false;

                string? contentType = response.Content.Headers.ContentType?.MediaType;

                if (contentType == null || contentType != "application/pdf")
                    return false;

                byte[] fileBytes = await response.Content.ReadAsByteArrayAsync();

                await File.WriteAllBytesAsync(filePath, fileBytes);

                return true;
            }
            catch
            {
                return false;
            }

        }
    }
}
