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

        [StringLength(50)]
        [Required(ErrorMessage = "O logradouro é obrigatório")]
        [DisplayName("Logradouro")]
        [JsonProperty(PropertyName = "logradouro")]
        public string Lougradouro { get; set; }

        [Required(ErrorMessage = "O número é obrigatório")]
        [DisplayName("Número")]
        public int NumLougradouro { get; set; }

        [StringLength(2)]
        [Required(ErrorMessage = "O estado é obrigatório")]
        [DisplayName("Estado")]
        [JsonProperty(PropertyName = "uf")]
        public string NomeUF { get; set; }

        [StringLength(200)]
        [Required(ErrorMessage = "O cidade é obrigatório")]
        [DisplayName("Cidade")]
        [JsonProperty(PropertyName = "localidade")]
        public string NomeCidade { get; set; }
        [StringLength(200)]
        [Required(ErrorMessage = "O bairro é obrigatório")]
        [DisplayName("Bairro")]
        [JsonProperty(PropertyName = "bairro")]
        public string Bairro { get; set; }
    }
}
