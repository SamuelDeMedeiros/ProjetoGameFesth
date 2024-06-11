using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProjetoE_CommerceGameFesth.Models
{
    public class Endereco
    {
        [Required(ErrorMessage = "O CEP é obrigatório")]
        [DisplayName("Cep")]
        public string CEP { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "O logradouro é obrigatório")]
        [DisplayName("Logradouro")]
        public string Lougradouro { get; set; }

        [Required(ErrorMessage = "O número é obrigatório")]
        [DisplayName("Número")]
        public int NumLougradouro { get; set; }

        [StringLength(2)]
        [Required(ErrorMessage = "O estado é obrigatório")]
        [DisplayName("Estado")]
        public string NomeUF { get; set; }

        [StringLength(200)]
        [Required(ErrorMessage = "O cidade é obrigatório")]
        [DisplayName("Cidade")]
        public string NomeCidade { get; set; }
    }
}
