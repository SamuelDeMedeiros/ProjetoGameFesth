using ProjetoE_CommerceGameFesth.Models;

namespace ProjetoE_CommerceGameFesth.Repository.Contract
{
    public interface IFuncionarioRepository
    {
        Funcionario Login(int id);

        void Cadastrar(CadastraEndereco cadastra);
        void Atualizar(CadastraEndereco cadastra);
        void AtualizarSenha(Funcionario funcionario);
        void AtualizarPerfil(Funcionario funcionario);
        void Excluir(int Id);
        CadastraEndereco ObterFuncionario(int Id);
        List<Funcionario> ObterFuncionarioPorEmail(string email);
        Funcionario ObterEmailFuncionario(string email);
        IEnumerable<Funcionario> ObterFuncionarioList(string por, string campo);
        Funcionario ObterSituacaoFuncionario(int id);
        void Ativar(int id);
        void Desativar(int id);
        void Promover(int id);
        void Rebaixar(int id);
    }
}
