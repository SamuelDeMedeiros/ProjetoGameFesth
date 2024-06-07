using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace ProjetoE_CommerceGameFesth.Models
{
    //trocar a classe inteira para a que usaremos
    // classe ilustrativa
    public class Produto
    {

        [DisplayName("Codigo de barras")]
        public Int64 Codbarras { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "O Nome do produto é obrigatório")]
        [DisplayName("Nome do produto")]
        public string NomeProduto { get; set; } = null!;

        [Required(ErrorMessage = "A imagem é obrigatória")]
        [DisplayName("Imagem do produto")]
        public string ImagemProduto { get; set; } = null!;
        public string Descricao { get; set; }

        //[Range(0, 999999.99)]
        [DisplayFormat(DataFormatString = "{0:0,0.000000}")]
        //[DataType(DataType.Currency)]
        [Required(ErrorMessage = "O valor é obrigatório")]
        [DisplayName("Preço")]
        public string Valor { get; set; }
        public decimal Valor1 { get; set; }

        [Required(ErrorMessage = "A quantidade em estoque é obrigatória")]
        [DisplayName("Quantidade em estoque")]
        public int QuantidadeEstoque { get; set; }
        public int Quantidade { get; set; }
        public decimal valorTotal { get; set; }
    }
}
