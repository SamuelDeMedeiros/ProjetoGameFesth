using ProjetoE_CommerceGameFesth.Models;

namespace ProjetoE_CommerceGameFesth.Repository.Contract
{
    public interface IVendaRepository
    {
        void buscaIdVenda(Venda venda);
        void Cadastrar(Venda venda, Item item);
        IEnumerable<Venda> ObterTodasCompras();
        DescricaoVenda ObterVenda(int nf, Int64 cod);
        public Int64 ObtemNF();
        int ObterCodCli(int nf);
        IEnumerable<Produto> ObterCodBarras(int nf);
    }
}
