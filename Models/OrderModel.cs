using Azure;
using Azure.Data.Tables;

namespace RetailWebApp_st10298329.Models
{
    public class OrderModel : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        public string ProductId { get; set; }
        public string CustomerId { get; set; }
        public int Quantity { get; set; }
        public DateTime OrderDate { get; set; }

        public OrderModel() { }

        public OrderModel(string partitionKey, string rowKey)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
        }
    }
}
