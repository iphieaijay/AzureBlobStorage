using AzureBlobStorage.Interfaces;
using AzureBlobStorage.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AzureBlobStorage.Controllers
{
    [Route("api/blobstorage")]
    [ApiController]
    public class BlobStorageController : ControllerBase
    {
        private readonly IBlobStorageService _blobStorageService;
        public BlobStorageController(IBlobStorageService blobStorageService)
        {
            _blobStorageService = blobStorageService;
        }
        // GET: api/<BlobStorageController>
        [HttpGet("GetBlob")]
        public async Task<IActionResult> GetBlob(string blobName)
        {
            var stream = await _blobStorageService.GetBlobAsync(blobName);
            if (stream == null)
                return NotFound();

            return File(stream, "application/octet-stream", blobName);
        }

        [HttpGet("GetBlobs")]
        public async Task<IActionResult> GetBlobs()
        {

            var result = await _blobStorageService.ListBlobAsync();
            return Ok(result);

        }
        [HttpPost("uploadBlobFile")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload([FromForm] FileModel model)
        {
            if (model == null || model.File.Length == 0)
                return BadRequest("No file selected.");
            var allowedTypes = new[] { ".jpg", ".jpeg", ".png", ".pdf" };

            var extension = Path.GetExtension(model.File.FileName).ToLowerInvariant();

            if (!allowedTypes.Contains(extension))
                throw new InvalidOperationException("File type not allowed.");


            await _blobStorageService.UploadBlobFileAsync(model.File);
            return Ok($"Uploaded {model.File.FileName} successfully.");
        }
        [HttpPost("uploadBulkBlobFiles")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadBulkFiles([FromForm] MultiFileModel model)
        {
            if (model.Files == null || model.Files.Count == 0)
                return BadRequest("No files selected.");

            foreach (var file in model.Files)
            {
                if (file.Length > 0)
                {

                    await _blobStorageService.UploadBlobFileAsync(file);
                }
            }
            return Ok($"Uploaded {model.Files.Count} files successfully.");
        }
        [HttpPost("upload-content")]
        public async Task<IActionResult> UploadContentFile([FromBody] ContentFileUploadModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Content))
                return BadRequest("Content is empty.");

            var bytes = Encoding.UTF8.GetBytes(model.Content);
            using var stream = new MemoryStream(bytes);

            await _blobStorageService.UploadBlobContentAsync(model.Content, model.FileName);
            return Ok($"Uploaded {model.FileName} successfully.");
        }
        [HttpDelete("DeleteBlob")]
        public async Task<IActionResult> DeleteBlob(string blobName)
        {

            await _blobStorageService.DeleteBlobAsync(blobName);
            return NoContent();

        }
    }
}
