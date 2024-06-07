using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProjetoE_CommerceGameFesth.Models
{
    public class Cliente
    {

        [DisplayName("Codigo do cliente")]
        public int IdCliente { get; set; }

        [StringLength(200)]
        [Required(ErrorMessage = "O nome é obrigatório")]
        [DisplayName("Nome do cliente")]
        public string NomeCliente { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "A data de nascimento é obrigatória")]
        [DisplayName("Data de nascimento")]
        public DateTime Nascimento { get; set; }

        //[StringLength(1)]
        //[Required(ErrorMessage = "O Sexo é obrigatório")]
        //[DisplayName("Sexo")]
        public string Sexo { get; set; }

        //[Phone]
        //[StringLength(11)]
        //[Required(ErrorMessage = "O número de telefone é obrigatório")]
        //[DisplayName("Número de telefone")]
        public string Telefone { get; set; }

        //[Required(ErrorMessage = "O CNPJ é obrigatório")]
        //[DisplayName("CNPJ")]
        public string CNPJ { get; set; }

        //[StringLength(200)]
        //[Required(ErrorMessage = "A inscrição estadual é obrigatória")]
        //[DisplayName("IE")]
        public string IE { get; set; }

        //[StringLength(200)]
        //[DisplayName("Nome fantasia")]
        public string NomeFantasia { get; set; }
        //[StringLength(200)]
        //[DisplayName("Razão social")]
        public string Razaosocial { get; set; }

        //[Required(ErrorMessage = "O RG é obrigatório")]
        //[DisplayName("RG")]
        public string RG { get; set; }

        //[Required(ErrorMessage = "O CPF é obrigatório")]
        //[DisplayName("CPF")]
        public string CPF { get; set; }

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

        //[StringLength(1)]
        //[DisplayName("Situação")]
        public string? Situacao { get; set; }
        public string? tipo_login { get; set; }
    }
}
