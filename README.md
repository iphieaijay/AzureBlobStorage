<img width="1877" height="1016" alt="image" src="https://github.com/user-attachments/assets/8b8d3e68-cca8-4f66-a94c-6ad5ebb29740" />

.NET 8 Serverless Project: Azure Blob Storage + Cosmos DB (Local Development)
This project demonstrates a .NET 8 Azure Functions serverless API that implements:

Azure Blob Storage (via Azurite)

Cosmos DB CRUD operations (via Cosmos DB Emulator)

All services run locally, making it perfect for development, testing, and CI/CD pipelines without needing an active Azure subscription.

### Features
Upload, list, and delete files in Azure Blob Storage

Create, Read, Update, Delete (CRUD) documents in Cosmos DB

Full local setup using Azurite and Cosmos DB Emulator

Clean, modular architecture using .NET 8 minimal APIs and Azure Functions v4

Local.settings.json support for local development

Docker instructions for running Azurite and Cosmos DB Emulator

### Technologies Used

.NET 8
Azure Functions v4
Azure Blob Storage (Azurite)
Azure Cosmos DB Emulator
Azure SDKs for .NET
Docker

Project Structure



 ### Prerequisites
.NET 8 SDK

Azure Functions Core Tools v4

Docker Desktop

Postman or cURL for testing

 ### Getting Started
1. Clone the Repo

git clone https://github.com/yourusername/dotnet8-serverless-blob-cosmos-local.git
cd dotnet8-serverless-blob-cosmos-local
2. Start Local Emulators
Start Azurite (Blob Storage Emulator)

docker run -d -p 10000:10000 --name azurite mcr.microsoft.com/azure-storage/azurite
Azurite Blob endpoint: http://localhost:10000/devstoreaccount1

Start Cosmos DB Emulator (Windows only)

"C:\Program Files\Azure Cosmos DB Emulator\CosmosDB.Emulator.exe" /AllowNetworkAccess /NoFirewall /EnablePreview=EnableMongoDbEndpoint=0
OR via Docker (Linux/macOS experimental):


docker run -p 8081:8081 -p 8900:8900 --name=cosmosdb -it mcr.microsoft.com/cosmosdb/linux/azure-cosmos-emulator
3. Update local.settings.json

{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
    "BlobStorageConnection": "UseDevelopmentStorage=true",
    "CosmosDbConnection": "https://localhost:8081/",
    "CosmosDbKey": "C2y6yDjf5/R+ob0N8A7Cgv30VRPJ...==",
    "CosmosDbDatabase": "LocalDb",
    "CosmosDbContainer": "Items"
  }
}
NB: Replace CosmosDbKey with the key shown in your emulator’s portal.

4. Build and Run the Function App

func start
The function will start on http://localhost:7071

### API Endpoints
1.  Blob Storage API
Method	Route	Description
POST	/api/blostoreage/uploadBlobFile	Upload a file
POST	/api/blostoreage/upload-content	Upload a content file eg json file
POST	/api/blostoreage/uploadBulkBlobFiles	Upload multiple image files
GET	/api/blobstorage/GetBlob	List all blobs
GET	/api/blobstorage/GetBlobs	List all blobs
DELETE	/api/blobstorage/deleteBlob?blobName=filename.txt	Delete a blob

 2.Student API
Method	Route	Description
POST	/api/student/v1/addStudent	Create a student document
GET	/api/student/v1/getStudent/{id}	Get student document by ID
PUT	/api/student/v1/ student document
DELETE	/api/student/v1/{id}	Delete document

### Sample Payloads
Student API

POST /api/student/v1/createStudent
{
 {
  "name": "string",
  "phoneNumber": "string",
  "email": "string",
  "age": 0
}
Update Student

PUT /api/student/v1/updateStudent
{
  "id": "string",
  "name": "string",
  "phoneNumber": "string",
  "email": "string",
  "age": 0
}
### Testing Blob Upload via cURL

curl -X POST http://localhost:7005/api/blobstorage/uploadBlobFile \
  -H "Content-Type: multipart/form-data" \
  -F "file=@student001.jpg"

### Troubleshooting
Issue	Solution
Cosmos DB connection error	Check if emulator is running with /AllowNetworkAccess
Azurite not reachable	Ensure Docker container is up and port 10000 is mapped
Functions not detecting storage	Use UseDevelopmentStorage=true in settings

### Resources
 Azurite GitHub

Cosmos DB Emulator Docs

Azure SDK for .NET

### Cleanup
To stop emulators:

docker stop azurite
docker stop cosmosdb
To remove containers:

docker rm azurite
docker rm cosmosdb

### License
This project is licensed under the MIT License.










Ask ChatGPT

