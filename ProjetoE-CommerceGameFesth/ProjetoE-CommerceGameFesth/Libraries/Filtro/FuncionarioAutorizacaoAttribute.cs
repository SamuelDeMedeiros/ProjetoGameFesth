using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProjetoE_CommerceGameFesth.Libraries.Login;
using ProjetoE_CommerceGameFesth.Models;
using ProjetoE_CommerceGameFesth.Models.Constants;
namespace ProjetoE_CommerceGameFesth.Libraries.Filtro
{
    public class FuncionarioAutorizacaoAttribute : Attribute, IAuthorizationFilter
    {
        private string _tipoFuncionarioAutorizado;
        public FuncionarioAutorizacaoAttribute(string TipoFuncionarioAutorizado = FuncionarioTipoConstant.Comum) 
        {
            _tipoFuncionarioAutorizado=TipoFuncionarioAutorizado;
        }
        LoginFuncionario _loginFuncionario;
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            _loginFuncionario = (LoginFuncionario)context.HttpContext.RequestServices.GetService(typeof(LoginFuncionario));
            Models.Funcionario funcionario = _loginFuncionario.GetFuncionario();
            if(funcionario == null)
            {
                context.Result = new RedirectToActionResult("Login", "Home", null);

            }
            else
            {
                if (funcionario.Tipo == FuncionarioTipoConstant.Comum && _tipoFuncionarioAutorizado == FuncionarioTipoConstant.Gerente)
                {
                    context.Result = new ForbidResult();
                }
            }
        }
    }
}
