using Newtonsoft.Json;

namespace AzureBlobStorage.Contracts
{
    public record CreateStudentDto 
    {
        [JsonProperty("name")]
        public string Name { get; init; }
        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; init; }
        [JsonProperty("email")]
        public string Email { get; init; } 
        [JsonProperty("age")]
        public int Age { get; init; }
    }
    public record UpdateStudentDto
    {
        [JsonProperty("id")]
        public string Id { get; init; }
        [JsonProperty("name")]
        public string Name { get; init; }
        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; init; }
        [JsonProperty("email")]
        public string Email { get; init; }
        [JsonProperty("age")]
        public int Age { get; init; }
    }

}
