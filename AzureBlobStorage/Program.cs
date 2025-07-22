using AzureBlobStorage.Interfaces;
using AzureBlobStorage.Middleware;
using AzureBlobStorage.Services;
using Microsoft.Extensions.Azure;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddBlobServiceClient(builder.Configuration["BlobStorageConnection:blobServiceUri"]!).WithName("BlobStorageConnection");
    clientBuilder.AddQueueServiceClient(builder.Configuration["BlobStorageConnection:queueServiceUri"]!).WithName("BlobStorageConnection");
    clientBuilder.AddTableServiceClient(builder.Configuration["BlobStorageConnection:tableServiceUri"]!).WithName("BlobStorageConnection");
});
//builder.Services.AddSingleton<IStudentService>(builder.Configuration.GetSection("CosmosDb").InitializeCosmosInstance) );
builder.Services.AddSingleton<IBlobStorageService, BlobStorageService>();
builder.Services.AddSingleton<IStudentService, StudentService>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddProblemDetails();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseExceptionHandler();
app.MapControllers();

app.Run();
