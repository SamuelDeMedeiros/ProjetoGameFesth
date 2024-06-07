using ProjetoE_CommerceGameFesth.Models;

namespace ProjetoE_CommerceGameFesth.Repository.Contract
{
    public interface IVendaRepository
    {
        void buscaIdVenda(Venda venda);
        void Cadastrar(Venda venda);
        IEnumerable<Venda> ObterTodasCompras();
        Venda ObterVenda(int Id);
    }
}
