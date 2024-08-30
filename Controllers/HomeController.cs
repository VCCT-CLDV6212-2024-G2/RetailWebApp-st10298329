using Microsoft.AspNetCore.Mvc;
using RetailWebApp_st10298329.Models;
using System.Diagnostics;
using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using Azure.Storage.Files.Shares;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


namespace RetailWebApp_st10298329.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BlobContainerClient _blobContainerClient;
        private readonly ShareClient _shareClient;
        private readonly TableClient _customerTableClient;
        private readonly TableClient _productTableClient;
        private readonly QueueClient _transactionQueueClient;
        private readonly QueueClient _inventoryQueueClient;

        public HomeController(ILogger<HomeController> logger ,BlobContainerClient blobContainerClient, 
            ShareClient shareClient, TableClient customerTableClient, TableClient productTableClient,
            QueueClient queueClient, QueueClient transactionQueueClient, QueueClient inventoryQueueClient)
        {
            _blobContainerClient = blobContainerClient;
            _shareClient = shareClient;
            _customerTableClient = customerTableClient;
            _logger = logger;
            _productTableClient = productTableClient;
            _transactionQueueClient =   transactionQueueClient;
            _inventoryQueueClient = inventoryQueueClient;
        }

        // Upload File to Azure File Storage
        [HttpPost]
        public async Task<IActionResult> UploadFileToAzure(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var directoryClient = _shareClient.GetRootDirectoryClient();
                var fileClient = directoryClient.GetFileClient(file.FileName);

                using (var stream = file.OpenReadStream())
                {
                    await fileClient.CreateAsync(file.Length);
                    await fileClient.UploadRangeAsync(new Azure.HttpRange(0, file.Length), stream);
                }
            }
            return RedirectToAction("Index");
        }

        // Upload Image to Azure Blob Storage
        [HttpPost]
        public async Task<IActionResult> UploadImageToAzureBlobStorage(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var blobClient = _blobContainerClient.GetBlobClient(file.FileName);
                using (var stream = file.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, true);
                }
            }
            return RedirectToAction("Index");
        }

        // GET: /Home/AddCustomer
        [HttpGet]
        public IActionResult AddCustomer()
        {
            return View();
        }

        // Add Customer Profile
        // POST: /Home/AddCustomerProfile
        [HttpPost]
        public async Task<IActionResult> AddCustomerProfile(string firstName, string lastName, string email, string phoneNumber)
        {
            var customerProfile = new CustomerProfile
            {
                PartitionKey = "CustomerProfile",
                RowKey = Guid.NewGuid().ToString(),
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PhoneNumber = phoneNumber
            };

            await _customerTableClient.AddEntityAsync(customerProfile);

            return RedirectToAction("Index");
        }

        // GET: /Home/AddProduct
        [HttpGet]
        public IActionResult AddProduct()
        {
            return View();
        }

        // Add Product to Azure Table Storage
        // POST: /Home/AddProduct
        [HttpPost]
        public async Task<IActionResult> AddProduct(string name, string description, decimal price, IFormFile productImage)
        {
            string imageUrl = null;

            if (productImage != null && productImage.Length > 0)
            {
                // Upload the product image to Azure Blob Storage
                var blobClient = _blobContainerClient.GetBlobClient(Guid.NewGuid().ToString() + Path.GetExtension(productImage.FileName));
                using (var stream = productImage.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, true);
                }
                imageUrl = blobClient.Uri.ToString();  // Store the URL of the uploaded image
            }

            var product = new ProductClass
            {
                PartitionKey = "Products",
                RowKey = Guid.NewGuid().ToString(),
                Name = name,
                Description = description,
                Price = price,
                ImageUrl = imageUrl
            };

            await _productTableClient.AddEntityAsync(product);

            return RedirectToAction("Index");
        }

        // Method to enqueue a transaction
        public async Task<IActionResult> EnqueueTransaction(string transactionId, decimal amount)
        {
            var transactionMessage = new
            {
                Type = "Transaction",
                TransactionId = transactionId,
                Amount = amount,
                Timestamp = DateTime.UtcNow
            };

            string messageContent = JsonConvert.SerializeObject(transactionMessage);
            await _transactionQueueClient.SendMessageAsync(Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(messageContent)));

            return RedirectToAction("Index");
        }

        // Method to enqueue an inventory process
        public async Task<IActionResult> EnqueueInventory(string productId, int quantity, string action)
        {
            var inventoryMessage = new
            {
                Type = "Inventory",
                ProductId = productId,
                Quantity = quantity,
                Action = action,
                Timestamp = DateTime.UtcNow
            };

            string messageContent = JsonConvert.SerializeObject(inventoryMessage);
            await _inventoryQueueClient.SendMessageAsync(Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(messageContent)));

            return RedirectToAction("Index");
        }

        // Dashboard Index
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
