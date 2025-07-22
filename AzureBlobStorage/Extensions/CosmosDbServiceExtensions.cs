using AzureBlobStorage.Services;
using Microsoft.Azure.Cosmos;

namespace AzureBlobStorage.Extensions
{
    public static class CosmosDbServiceExtensions
    {
        public static async Task<StudentService> InitializeCosmosInstance(this IConfiguration configuration)
        {
            var account = configuration["CosmosDb:Account"];
            var key = configuration["CosmosDb:Key"];
            var databaseName = configuration["CosmosDb:DatabaseName"];
            var containerName = configuration["CosmosDb:ContainerName"];

            var client = new CosmosClient(account, key);
            var db= client.GetDatabase(databaseName);
            var container= db.GetContainer(containerName);

            //await database.Database.CreateContainerIfNotExistsAsync(containerName, "/id");
            
            return new StudentService(client,db,containerName);
        }
    }
}
