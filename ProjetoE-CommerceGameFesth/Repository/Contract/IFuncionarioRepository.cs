using ProjetoE_CommerceGameFesth.Models;

namespace ProjetoE_CommerceGameFesth.Repository.Contract
{
    public interface IFuncionarioRepository
    {
        Funcionario Login(int id);

        void Cadastrar(Funcionario funcionario);
        void Atualizar(Funcionario funcionario);
        void AtualizarSenha(Funcionario funcionario);
        void AtualizarPerfil(Funcionario funcionario);
        void Excluir(int Id);
        Funcionario ObterFuncionario(int Id);
        List<Funcionario> ObterFuncionarioPorEmail(string email);
        Funcionario ObterEmailFuncionario(string email);
        IEnumerable<Funcionario> ObterFuncionarioList();
    }
}
