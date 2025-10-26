using Newtonsoft.Json;
using TrainMaster.Domain.General;
using JsonIgnoreAttribute = System.Text.Json.Serialization.JsonIgnoreAttribute;

namespace TrainMaster.Domain.Entity
{
    public class AddressEntity : BaseEntity
    {
        [JsonProperty("cep")]
        public string? PostalCode { get; set; }

        [JsonProperty("logradouro")]
        public string? Street { get; set; }

        [JsonProperty("bairro")]
        public string? Neighborhood { get; set; }

        [JsonProperty("localidade")]
        public string? City { get; set; }

        [JsonProperty("uf")]
        public string? Uf { get; set; }

        [JsonIgnore]
        public PessoalProfileEntity? PessoalProfile { get; set; }        
        public int PessoalProfileId { get; set; }
    }
}