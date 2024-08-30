using Azure;
using Azure.Data.Tables;

namespace RetailWebApp_st10298329.Models
{
    public class ProductClass : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        // Product-specific properties
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }

        // Default constructor for TableEntity deserialization
        public ProductClass() { }

        public ProductClass(string partitionKey, string rowKey)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
        }
    }
}
