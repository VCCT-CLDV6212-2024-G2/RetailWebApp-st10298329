using Azure.Storage.Queues;

namespace RetailWebApp_st10298329.Services
{
    public class QueueStorageService
    {
        private readonly QueueClient _queueClient;

        public QueueStorageService(string storageConnectionString, string queueName)
        {
            var serviceClient = new QueueServiceClient(storageConnectionString);
            _queueClient = serviceClient.GetQueueClient(queueName);
            _queueClient.CreateIfNotExists();
        }

        public async Task SendMessageAsync(string message)
        {
            await _queueClient.SendMessageAsync(message);
        }

        public async Task<string> ReceiveMessageAsync()
        {
            var response = await _queueClient.ReceiveMessageAsync();
            return response?.Value?.MessageText;
        }
    }
}
