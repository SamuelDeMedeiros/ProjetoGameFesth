using ProjetoE_CommerceGameFesth.Models;

namespace ProjetoE_CommerceGameFesth.Repository.Contract
{
    public interface IItemRepository
    {
        IEnumerable<Item> ObterTodosItens();
        void Adicionar(Item item);
        void Atualizar(Item item);
        Item Obteritens(int Id);
        void Apagar(int Id);
    }
}
