using System.ComponentModel.DataAnnotations;

namespace AzureBlobStorage.Models
{
    public class FileModel
    {
        [Required]
       public IFormFile File { get; set; }
    }
    public class MultiFileModel
    {
        [Required]
        public List<IFormFile> Files { get; set; }
    }
    public class ContentFileUploadModel
    {
        [Required]
        public string FileName { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
