using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using Azure.Storage.Files.Shares;
using RetailWebApp_st10298329.Services;

var builder = WebApplication.CreateBuilder(args);

// Register services with the connection string from appsettings.json
var azureStorageConnectionString = builder.Configuration.GetConnectionString("AzureStorage");

if (azureStorageConnectionString != null)
{
    // Register BlobContainerClient with the DI container
    builder.Services.AddSingleton<BlobContainerClient>(sp =>
    {
        var blobServiceClient = new BlobServiceClient(azureStorageConnectionString);
        return blobServiceClient.GetBlobContainerClient("product-images");
    });

    // Register TableClient instances for each table
    builder.Services.AddSingleton(sp =>
        new TableClient(azureStorageConnectionString, "CustomerProfilesTable"));

    builder.Services.AddSingleton(sp =>
        new TableClient(azureStorageConnectionString, "ProductsTable"));

    // Register QueueClient for Azure Queue Storage
    builder.Services.AddSingleton<QueueClient>(sp =>
    {
        var queueServiceClient = new QueueServiceClient(azureStorageConnectionString);
        return queueServiceClient.GetQueueClient("order-queue");
    });

    // Register ShareClient for Azure File Storage
    builder.Services.AddSingleton<ShareClient>(sp =>
    {
        var shareServiceClient = new ShareServiceClient(azureStorageConnectionString);
        return shareServiceClient.GetShareClient("logs");
    });
}

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
