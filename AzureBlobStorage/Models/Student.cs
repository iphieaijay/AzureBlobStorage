using Newtonsoft.Json;

namespace AzureBlobStorage.Models
{
    public class Student
    {
        [JsonProperty(PropertyName="id")]
        public string StudentId { get; set; }            
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("phoneNo")]
        public string PhoneNumber { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("age")]
        public int Age{ get; set; }
        // Add your partition key property if needed
        public string PartitionKey { get; set; }

    }
}
