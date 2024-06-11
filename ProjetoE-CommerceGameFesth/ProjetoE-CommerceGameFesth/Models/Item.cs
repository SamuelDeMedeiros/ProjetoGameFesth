using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProjetoE_CommerceGameFesth.Models
{
    public class Item
    {
        [DisplayName("Codigo de barras")]
        public Int64 Codbarras { get; set; }

        [Required(ErrorMessage = "A nota fiscal é obrigatória")]
        [DisplayName("Nota fiscal")]
        public int NotaFiscal { get; set; }

        //[Range(0, 9999.99)]
        [DisplayFormat(DataFormatString = "{0:0,0.000000}")]
        [Required(ErrorMessage = "O preço é obrigatório")]
        [DisplayName("Preço")]
        public string Valor { get; set; }

        [Required(ErrorMessage = "A quantidade é obrigatória")]
        [DisplayName("Quantidade")]
        public int Quantidade { get; set; }
    }
}
