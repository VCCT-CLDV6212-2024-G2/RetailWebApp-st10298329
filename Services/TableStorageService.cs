namespace RetailWebApp_st10298329.Services
{
    public class TableStorageService
    {
        private readonly TableClient _customerTableClient;
        private readonly TableClient _productTableClient;

        public TableStorageService(string storageConnectionString, string customerTableName, string productTableName)
        {
            var serviceClient = new TableServiceClient(storageConnectionString);
            _customerTableClient = serviceClient.GetTableClient(customerTableName);
            _productTableClient = serviceClient.GetTableClient(productTableName);
            _customerTableClient.CreateIfNotExists();
            _productTableClient.CreateIfNotExists();
        }

        public async Task AddCustomerAsync(CustomerEntity customer)
        {
            await _customerTableClient.AddEntityAsync(customer);
        }

        public async Task<CustomerEntity> GetCustomerAsync(string partitionKey, string rowKey)
        {
            return await _customerTableClient.GetEntityAsync<CustomerEntity>(partitionKey, rowKey);
        }

        public async Task AddProductAsync(ProductEntity product)
        {
            await _productTableClient.AddEntityAsync(product);
        }

        public async Task<ProductEntity> GetProductAsync(string partitionKey, string rowKey)
        {
            return await _productTableClient.GetEntityAsync<ProductEntity>(partitionKey, rowKey);
        }
    }
}
