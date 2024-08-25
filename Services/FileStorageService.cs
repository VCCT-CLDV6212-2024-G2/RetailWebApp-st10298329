namespace RetailWebApp_st10298329.Services
{
    public class FileStorageService
    {
        private readonly ShareClient _shareClient;

        public FileStorageService(string storageConnectionString, string shareName)
        {
            var serviceClient = new ShareServiceClient(storageConnectionString);
            _shareClient = serviceClient.GetShareClient(shareName);
            _shareClient.CreateIfNotExists();
        }

        public async Task UploadFileAsync(string fileName, Stream content)
        {
            var directoryClient = _shareClient.GetRootDirectoryClient();
            var fileClient = directoryClient.GetFileClient(fileName);
            await fileClient.CreateAsync(content.Length);
            await fileClient.UploadAsync(content);
        }

        public async Task<Stream> DownloadFileAsync(string fileName)
        {
            var fileClient = _shareClient.GetRootDirectoryClient().GetFileClient(fileName);
            var downloadInfo = await fileClient.DownloadAsync();
            return downloadInfo.Value.Content;
        }
    }
}
