using ProjetoE_CommerceGameFesth.Models;

namespace ProjetoE_CommerceGameFesth.Repository.Contract
{
    public interface IProdutoRepository
    {
        IEnumerable<Produto> ObterTodosProdutos(string por, string campo, string pesquisa);
        IEnumerable<Produto> PesquisaProdutos(string Nome);
        void Adicionar(Produto Iproduto);
        void Atualizar(Produto Iproduto);
        Produto ObterProduto(long Id);
        void Apagar(long Id);
    }
}
