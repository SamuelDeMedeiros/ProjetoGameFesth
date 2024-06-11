using ProjetoE_CommerceGameFesth.Models;

namespace ProjetoE_CommerceGameFesth.Repository.Contract
{
    public interface ILoginRepository
    {

        Login Login(string Email, string Senha);
        Login Login(string Email);

        void EnviaEmail(string Email, string Assunto, string Mensagem);
    }
}
