using Microsoft.AspNetCore.Mvc;
using ProjetoE_CommerceGameFesth.CarrinhoCompra;
using ProjetoE_CommerceGameFesth.Libraries.Filtro;
using ProjetoE_CommerceGameFesth.Libraries.Login;
using ProjetoE_CommerceGameFesth.Models;
using ProjetoE_CommerceGameFesth.Models.Constants;
using ProjetoE_CommerceGameFesth.Repository;
using ProjetoE_CommerceGameFesth.Repository.Contract;

namespace ProjetoE_CommerceGameFesth.Areas.Funcionario.Controllers
{
    [Area("Funcionario")]
    public class HomeController : Controller
    {
        private IFuncionarioRepository _funcionarioRepository;
        private LoginFuncionario _loginfuncionario;
        private IClienteRepository _clienteRepository;
        private LoginCliente _loginCliente;
        private ILoginRepository _loginRepository;

        public HomeController(IFuncionarioRepository funcionarioRepository, LoginFuncionario loginfuncionario, ILoginRepository loginRepository, IClienteRepository clienteRepository, LoginCliente loginCliente)
        {
            _loginfuncionario = loginfuncionario;
            _funcionarioRepository = funcionarioRepository;
            _loginRepository = loginRepository;
            _clienteRepository = clienteRepository;
            _loginCliente = loginCliente;

        }
        [FuncionarioAutorizacao]
        public IActionResult Index()
        {
            if (_loginfuncionario.GetFuncionario().Tipo != FuncionarioTipoConstant.Comum)
            {
                string g = "Gerente";
                ViewBag.Cargo = g;
            }
            else
            {
                string c = "Comum";
                ViewBag.Cargo = c;
            }
            return View(_funcionarioRepository.ObterFuncionario(_loginfuncionario.GetFuncionario().IdFuncionario));
        }

        [HttpGet]
        [ValidateHttpRefer]
        public IActionResult AtualizarP(int id)
        {
            Models.Funcionario funcionario = _funcionarioRepository.ObterFuncionario(id);
            return View(funcionario);
        }
        [HttpPost]
        public IActionResult AtualizarP([FromForm] Models.Funcionario funcionario)
        {
            if (ModelState.IsValid)
            {
                _loginfuncionario.Atualizar(funcionario);
                _funcionarioRepository.AtualizarPerfil(funcionario);
                TempData["MSG_S"] = "Perfil atualizado com sucesso! ";
                return RedirectToAction(nameof(Index));
            }
            TempData["MSG_S"] = "Erro ao atualizar funcionario!";
            return View();

        }
        [FuncionarioAutorizacao]
        public IActionResult Logout()
        {
            _loginfuncionario.Logout();
            
            return RedirectToAction("Login", "Home", new { area = "" });
        }
        [FuncionarioAutorizacao]
        public IActionResult Painel()
        {
            return View();
        }
    }
}
