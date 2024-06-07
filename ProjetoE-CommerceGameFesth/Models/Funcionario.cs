using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProjetoE_CommerceGameFesth.Models
{
    public class Funcionario
    {
        [DisplayName("Codigo do funcionario")]
        public int IdFuncionario {  get; set; }

        [StringLength(200)]
        [DisplayName("Nome do funcionario")]
        public string NomeFuncionario { get; set; }

        //[Phone]
        [DisplayName("Númerio de telefone")]
        public int  Telefone { get; set; }

        [DisplayName("Data de admissao")]
        public DateTime DataAdmissao { get; set; }

        [StringLength(8)]
        [DisplayName("Tipo")]
        public string?  Tipo { get; set; }

        [StringLength(1)]
        [DisplayName("TipoLogin")]
        public string? TipoLogin { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "O Email é obrigatório")]
        [EmailAddress]
        [DisplayName("Email")]
        public string Email { get; set; } 

        [StringLength(8, MinimumLength = 8, ErrorMessage = "A senha deve ter 8 caracteres")]
        [Required(ErrorMessage = "A senha é obrigatória")]
        [DataType(DataType.Password)]
        [DisplayName("Senha")]
        public string Senha { get; set; } 

        [StringLength(8, MinimumLength = 8, ErrorMessage = "A senha deve ter 8 caracteres")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "A confirmação da senha é obrigatória")]
        [Compare("Senha", ErrorMessage = "As senhas tem que ser iguais")]
        [DisplayName("Confirmar a senha")]
        public string ConfirmaSenha { get; set; }

        [Required(ErrorMessage = "O CEP é obrigatório")]
        [DisplayName("Cep")]
        public decimal CEP { get; set; }

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
        public int Numero { get; internal set; }
        public int id_Login { get; internal set; }
        public string? tipo_login { get; internal set; }
    }
}
