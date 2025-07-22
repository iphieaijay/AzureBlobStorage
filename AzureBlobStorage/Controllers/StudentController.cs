using AzureBlobStorage.Contracts;
using AzureBlobStorage.Interfaces;
using AzureBlobStorage.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AzureBlobStorage.Controllers
{
    [Route("api/student/v1")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }
        [HttpPost("addStudent")]
        public async Task<IActionResult> AddStudent([FromBody] CreateStudentDto student)
        {
            var response = await _studentService.AddStudentAsync(student);
            if (!response.IsSuccess)
                return BadRequest(response);
            return Ok(response);
        }
        [HttpGet("getStudent")]
        public async Task<IActionResult> GeStudent(string id)
        {
            var response = await _studentService.GetStudentByIdAsync(id);
            if (!response.IsSuccess)
                return NotFound();

            return Ok(response);
        }

        [HttpGet("getAllStudents")]
        public async Task<IActionResult> GetStudents()
        {
            var query= "SELECT * FROM c";
            var result = await _studentService.GetAllStudentsAsync(query);
            return Ok(result);

        }
        [HttpPut("updateStudent")]
        public async Task<IActionResult> UpdateStudent([FromBody] UpdateStudentDto student)
        {
            var response = await _studentService.UpdateStudentAsync(student);
            if (!response.IsSuccess)
                return BadRequest(response);
            return Ok(response);
        }
        [HttpDelete("deleteStudent")]
        public async Task<IActionResult> DeleteStudent(string id)
        {

            await _studentService.DeleteStudentAsync(id);
            return NoContent();

        }
    }
}
