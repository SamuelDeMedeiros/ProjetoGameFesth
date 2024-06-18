namespace ProjetoE_CommerceGameFesth.Models
{
    // PaginacaoViewModel.cs
    public class PaginacaoViewModel
    {
        public List<Produto> Produtos { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }

    }

}
