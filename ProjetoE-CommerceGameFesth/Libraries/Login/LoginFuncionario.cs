using Newtonsoft.Json;
using ProjetoE_CommerceGameFesth.Models;

namespace ProjetoE_CommerceGameFesth.Libraries.Login
{
    public class LoginFuncionario
    {
        private string Key = "Login.Funcionario";
        private Sessao.Sessao _sessao;

        public LoginFuncionario(Sessao.Sessao sessao)
        {
            _sessao = sessao;
        }
        //public void Atualizar(string Key, string Valor)
        //{
        //    if (Existe(Key))
        //    {
        //        _context.HttpContext.Session.Remove(Key);
        //    }
        //    _context.HttpContext.Session.SetString(Key, Valor);
        //}
        public void Atualizar(Funcionario funcionario)
        {
            string funcionarioJSONString = JsonConvert.SerializeObject(funcionario);
            _sessao.Atualizar(Key, funcionarioJSONString);
        }
        public void Login(Funcionario funcionario)
        {
            string funcionarioJSONString = JsonConvert.SerializeObject(funcionario);
            _sessao.Cadastrar(Key, funcionarioJSONString);
        }
        public Funcionario GetFuncionario()
        {
            if (_sessao.Existe(Key))
            {
                string funcionarioJSONString = _sessao.Consultar(Key);
                return JsonConvert.DeserializeObject<Funcionario>(funcionarioJSONString);

            }
            else
            {
                return null;
            }
        }
        public void Logout()
        {
            _sessao.RemoverTodos();
        }
    }
}
