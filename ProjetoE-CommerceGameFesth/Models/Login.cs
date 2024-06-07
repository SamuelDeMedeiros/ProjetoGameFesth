using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ProjetoE_CommerceGameFesth.Models
{
    public class Login
    {
        public int id_Login { get; set; }

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

        public string? tipo_login { get; set; }
    }
}
