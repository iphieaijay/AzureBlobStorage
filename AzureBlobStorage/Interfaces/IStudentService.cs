using AzureBlobStorage.Contracts;

namespace AzureBlobStorage.Interfaces
{
    public interface IStudentService
    {
        public Task<ApiResponse> GetAllStudentsAsync(string query);
        public Task<ApiResponse> GetStudentByIdAsync(string id);
        public Task<ApiResponse> AddStudentAsync(CreateStudentDto studentDto);
        public Task<ApiResponse> UpdateStudentAsync(UpdateStudentDto studentDto);
        public Task<ApiResponse> DeleteStudentAsync(string id);
    }
}
