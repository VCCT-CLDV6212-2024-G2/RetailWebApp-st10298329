using Microsoft.AspNetCore.Mvc;
using RetailWebApp_st10298329.Models;
using System.Diagnostics;
using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using Azure.Storage.Files.Shares;
using Microsoft.Extensions.Logging;

namespace RetailWebApp_st10298329.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BlobContainerClient _blobContainerClient;
        private readonly TableClient _customerTableClient;
        private readonly TableClient _productTableClient;
        private readonly QueueClient _queueClient;
        private readonly ShareClient _shareClient;

        public HomeController(
            ILogger<HomeController> logger,
             BlobContainerClient blobContainerClient,
             TableClient customerTableClient,
             TableClient productTableClient,
             QueueClient queueClient,
             ShareClient shareClient)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _customerTableClient = customerTableClient ?? throw new ArgumentNullException(nameof(customerTableClient));
            _productTableClient = productTableClient ?? throw new ArgumentNullException(nameof(productTableClient));
            _blobContainerClient = blobContainerClient ?? throw new ArgumentNullException(nameof(blobContainerClient));
            _queueClient = queueClient ?? throw new ArgumentNullException(nameof(queueClient));
            _shareClient = shareClient ?? throw new ArgumentNullException(nameof(shareClient));
        }

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
