using Org.BouncyCastle.Bcpg.OpenPgp;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProjetoE_CommerceGameFesth.Models
{
    public class Cliente
    {

        [DisplayName("Codigo do cliente")]
        public int IdCliente { get; set; }
        public int IdLog { get; set; }

        [StringLength(200)]
        [Required(ErrorMessage = "O nome é obrigatório")]
        [DisplayName("Nome do cliente")]
        public string NomeCliente { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "A data de nascimento é obrigatória")]
        [DisplayName("Data de nascimento")]
        public DateTime Nascimento { get; set; }

        [StringLength(1)]
        [Required(ErrorMessage = "O Sexo é obrigatório")]
        [DisplayName("Sexo")]
        public string Sexo { get; set; }

        [Phone]
        [DisplayName("Número de telefone")]
        public string Telefone { get; set; }

        [DisplayName("CNPJ")]
        public string CNPJ { get; set; }

        [StringLength(200)]
        [Required(ErrorMessage = "A inscrição estadual é obrigatória")]
        [DisplayName("IE")]
        public string IE { get; set; }

        [StringLength(200)]
        [DisplayName("Nome fantasia")]
        [Required(ErrorMessage = "O nome fantasia é obrigatório")]
        public string NomeFantasia { get; set; }
        [StringLength(200)]
        [DisplayName("Razão social")]
        [Required(ErrorMessage = "O razão social é obrigatório")]
        public string Razaosocial { get; set; }

        [DisplayName("RG")]
        public string RG { get; set; }
        [DisplayName("CPF")]
        public string CPF { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "O Email é obrigatório")]
        [EmailAddress]
        [DisplayName("Email")]
        public string Email { get; set; }

        [StringLength(20, MinimumLength = 4, ErrorMessage = "A senha deve ter de 4 a 20 caracteres")]
        [Required(ErrorMessage = "A senha é obrigatória")]
        [DataType(DataType.Password)]
        [DisplayName("Senha")]
        public string Senha { get; set; }

        [Required(ErrorMessage = "A confirmação da senha é obrigatória")]
        [Compare("Senha", ErrorMessage = "As senhas tem que ser iguais")]
        [DisplayName("Confirmar a senha")]
        public string ConfirmaSenha { get; set; }

        //[StringLength(1)]
        //[DisplayName("Situação")]
        public string? Situacao { get; set; }
        public string? tipo_login { get; set; }
        public string? TipoPessoa { get; set; }
        public string Codigo {  get; set; }

    }
}
