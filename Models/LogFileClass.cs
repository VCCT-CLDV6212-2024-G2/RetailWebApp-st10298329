using Azure.Storage.Files.Shares;
using Azure;
using System;
using System.IO;
using Azure.Storage.Files.Shares.Models;

namespace RetailWebApp_st10298329.Models
{
    public class LogFileClass
    {
        public string FileName { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }

        public LogFileClass() { }

        public LogFileClass(string fileName, string content)
        {
            FileName = fileName;
            Content = content;
            CreatedDate = DateTime.UtcNow;
        }

        public async Task UploadAsync(ShareClient shareClient)
        {
            var directoryClient = shareClient.GetRootDirectoryClient();
            var fileClient = directoryClient.GetFileClient(FileName);
            using (var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(Content)))
            {
                await fileClient.CreateAsync(stream.Length);
                await fileClient.UploadRangeAsync(new HttpRange(0, stream.Length), stream);
            }
        }

        public async Task<string> DownloadAsync(ShareClient shareClient)
        {
            var directoryClient = shareClient.GetRootDirectoryClient();
            var fileClient = directoryClient.GetFileClient(FileName);
            var downloadInfo = await fileClient.DownloadAsync();
            using (var reader = new StreamReader(downloadInfo.Value.Content))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}
