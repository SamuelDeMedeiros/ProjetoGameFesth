using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProjetoE_CommerceGameFesth.Models
{
    public class Venda
    {
        [DisplayName("Nota fiscal")]
        [Required(ErrorMessage = "A nota fiscal é obrigatória")]
        public Int64 NotaFiscal { get; set; }

        [Range(0, 9999.99)]
        [Required(ErrorMessage = "O valor total é obrigatório")]
        [DisplayName("Valor total")]
        public decimal ValorTotal { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "A data da venda é obrigatória")]
        [DisplayName("Data da venda")]
        public DateTime DataVenda { get; set; }

        [DisplayName("Email do cliente")]
        public string EmailCliente { get; set; }

        [DisplayName("Codigo do cliente")]
        public int IdCliente { get; set; }
    }
}
