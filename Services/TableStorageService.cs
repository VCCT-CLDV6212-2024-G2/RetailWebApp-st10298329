using Azure.Data.Tables;
using RetailWebApp_st10298329.Models;

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

        public async Task AddCustomerAsync(CustomerProfile customer)
        {
            await _customerTableClient.AddEntityAsync(customer); 
        }

        public async Task<CustomerProfile> GetCustomerAsync(string partitionKey, string rowKey)
        {
            return await _customerTableClient.GetEntityAsync<CustomerProfile>(partitionKey, rowKey);
        }

        public async Task AddProductAsync(ProductClass product)
        {
            await _productTableClient.AddEntityAsync(product);
        }

        public async Task<ProductClass> GetProductAsync(string partitionKey, string rowKey)
        {
            return await _productTableClient.GetEntityAsync<ProductClass>(partitionKey, rowKey);
        }
    }
}
