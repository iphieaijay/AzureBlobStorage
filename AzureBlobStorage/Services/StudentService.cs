using AzureBlobStorage.Contracts;
using AzureBlobStorage.Interfaces;
using AzureBlobStorage.Models;
using Microsoft.Azure.Cosmos;

namespace AzureBlobStorage.Services
{
    public class StudentService : IStudentService
    {
        private readonly CosmosClient _client;
        private readonly Container _container;

        public StudentService(IConfiguration config)
        {
            var account = config["CosmosDb:Account"];
            var key = config["CosmosDb:Key"];
            var databaseName = config["CosmosDb:DatabaseName"];
            var containerName = config["CosmosDb:ContainerName"];

            _client = new CosmosClient(account, key);

            Database db=_client.GetDatabase(databaseName);

            _container = db.GetContainer(containerName);
        }

        public async Task<ApiResponse> AddStudentAsync(CreateStudentDto studentDto)
        {
            var student = new Student
            {
                StudentId = Guid.NewGuid().ToString(),
                Name=studentDto.Name,
                Email=studentDto.Email,
                PhoneNumber = studentDto.PhoneNumber,
                Age = studentDto.Age,
                                
            };
            student.PartitionKey = student.StudentId;
            var response = await _container.CreateItemAsync(student);
            if (response.StatusCode.ToString() == "Created")
            {
                return new ApiResponse
                {
                    IsSuccess = true,
                    Message = "Student created successfully",
                    Result = student
                };
                   
            }
            else
            {
                return new ApiResponse
                {
                    IsSuccess = false,
                    Message = "unable to create student.",
                    Result = null
                };
            }
                        
        }

        public async Task<ApiResponse> GetAllStudentsAsync(string query)
        {
            var queryIterator = _container.GetItemQueryIterator<Student>(new QueryDefinition(query));
            var students = new List<Student>();
            while (queryIterator.HasMoreResults)
            {
                var response = await queryIterator.ReadNextAsync();
                students.AddRange(response);
            }
            return new ApiResponse
            {
                IsSuccess = true,
                Message = "Students retrieved successfully",
                Result = students
            };
        }

        public async Task<ApiResponse> GetStudentByIdAsync(string id)
        {
            try
            {
                var response = await _container.ReadItemAsync<Student>(id, new PartitionKey(id));
                return new ApiResponse
                {
                    IsSuccess = true,
                    Message = "Student retrieved successfully",
                    Result = response.Resource
                };
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new ApiResponse
                {
                    IsSuccess = false,
                    Message = "Student not found",
                    Result = null
                };
            }
            catch (CosmosException ex)
            {
                return new ApiResponse
                {
                    IsSuccess = false,
                    Message = $"Cosmos DB error: {ex.Message}",
                    Result = null
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse
                {
                    IsSuccess = false,
                    Message = $"Unexpected error: {ex.Message}",
                    Result = null
                };
            }
        }

        public async Task<ApiResponse> UpdateStudentAsync(UpdateStudentDto studentDto)
        {
            var studentObj = await GetStudentByIdAsync(studentDto.Id.ToString());
            if (!studentObj.IsSuccess || studentObj.Result is not Student std)
            {
                return new ApiResponse
                {
                    IsSuccess = false,
                    Message = "Student not found",
                    Result = null
                };
            }

            // Update only if new values are provided
            std.Name = !string.IsNullOrWhiteSpace(studentDto.Name) ? studentDto.Name.Trim() : std.Name;
            std.Age = studentDto.Age > 0 ? studentDto.Age : std.Age;
            std.Email = !string.IsNullOrWhiteSpace(studentDto.Email) ? studentDto.Email.Trim() : std.Email;
            std.PhoneNumber = !string.IsNullOrWhiteSpace(studentDto.PhoneNumber) ? studentDto.PhoneNumber.Trim() : std.PhoneNumber;

            try
            {
                var result = await _container.UpsertItemAsync(std, new PartitionKey(std.StudentId));
                return new ApiResponse
                {
                    IsSuccess = true,
                    Message = "Student updated successfully",
                    Result = result.Resource
                };
            }
            catch (CosmosException ex)
            {
                return new ApiResponse
                {
                    IsSuccess = false,
                    Message = $"Failed to update student: {ex.Message}",
                    Result = null
                };
            }
        }
        public async Task<ApiResponse> DeleteStudentAsync(string id)
        {
            ApiResponse response = null;
            var entityToDelete = await GetStudentByIdAsync(id);
            if (entityToDelete.IsSuccess)
            {
                await _container.DeleteItemAsync<Student>(id, new PartitionKey(id));
                return new ApiResponse
                {
                    IsSuccess = true,
                    Message = "Student deleted successfully",
                    Result = null
                };
            }
            else
            {
                return new ApiResponse
                {
                    IsSuccess = false,
                    Message = "Student not found",
                    Result = null
                };
            }
        }
    }

    
}
