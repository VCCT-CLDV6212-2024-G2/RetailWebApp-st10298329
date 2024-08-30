using Azure;
using Azure.Data.Tables;

namespace RetailWebApp_st10298329.Models
{
    public class CustomerProfile : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        // Your existing properties
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        // Default constructor is needed for TableEntity deserialization
        public CustomerProfile() { }

        public CustomerProfile(string partitionKey, string rowKey)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
        }
    }
}
