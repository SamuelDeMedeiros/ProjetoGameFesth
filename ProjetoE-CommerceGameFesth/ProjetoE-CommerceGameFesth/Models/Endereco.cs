using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProjetoE_CommerceGameFesth.Models
{
    public class Endereco
    {
        [Required(ErrorMessage = "O CEP é obrigatório")]
        [DisplayName("Cep")]
        [JsonProperty(PropertyName = "cep")]
        public string CEP { get; set; }

        [DisplayName("Logradouro")]
        [JsonProperty(PropertyName = "logradouro")]
        public string Lougradouro { get; set; }

        [DisplayName("Número")]
        public string NumLougradouro { get; set; }

        [DisplayName("Estado")]
        [JsonProperty(PropertyName = "uf")]
        public string NomeUF { get; set; }

        [DisplayName("Cidade")]
        [JsonProperty(PropertyName = "localidade")]
        public string NomeCidade { get; set; }

        [DisplayName("Bairro")]
        [JsonProperty(PropertyName = "bairro")]
        public string Bairro { get; set; }
    }
}
