using Newtonsoft.Json;

namespace FoodShop.Contract.DataTransferObjects.Respone.V1
{

    public class FacebookUserInfoResponse
    {
        [JsonProperty("first_name")]
        public string? FirstName { get; set; }

        [JsonProperty("last_name")]
        public string? LastName { get; set; }

        [JsonProperty("email")]
        public string? Email { get; set; }

        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }
    }
    
}
