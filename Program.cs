using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RetailWebApp_st10298329.Services;

var builder = WebApplication.CreateBuilder(args);

// Register services with the connection string from appsettings.json
var azureStorageConnectionString = builder.Configuration.GetConnectionString("AzureStorage");

builder.Services.AddSingleton(new TableStorageService(azureStorageConnectionString));
builder.Services.AddSingleton(new BlobStorageService(azureStorageConnectionString));
builder.Services.AddSingleton(new QueueStorageService(azureStorageConnectionString));
builder.Services.AddSingleton(new FileStorageService(azureStorageConnectionString));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
