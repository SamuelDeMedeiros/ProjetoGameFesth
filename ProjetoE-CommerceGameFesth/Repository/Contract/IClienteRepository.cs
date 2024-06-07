using ProjetoE_CommerceGameFesth.Models;

namespace ProjetoE_CommerceGameFesth.Repository.Contract
{
    public interface IClienteRepository
    {

        Cliente Login(int id);

        void Cadastrar(CadastraEndereco cadastraEndereco);
        void Atualizar(CadastraEndereco cadastraEndereco);
        void AtualizarP(Cliente cliente);
        void AtualizarSenha(Cliente cliente);
        void Excluir(int Id);
        CadastraEndereco ObterCliente(int Id);
        void Ativar(int id);
        void Desativar(int id);
        Cliente ObterClientePorEmail(string email);
        Cliente ObterCpfCliente(string CPF);
        Cliente ObterEmailCliente(string email);
        IEnumerable<Cliente> ObterClienteList();
    }
}
