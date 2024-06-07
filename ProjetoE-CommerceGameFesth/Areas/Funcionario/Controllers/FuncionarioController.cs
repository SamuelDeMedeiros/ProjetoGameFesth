using Microsoft.AspNetCore.Mvc;
using ProjetoE_CommerceGameFesth.Libraries.Filtro;
using ProjetoE_CommerceGameFesth.Libraries.Login;
using ProjetoE_CommerceGameFesth.Models;
using ProjetoE_CommerceGameFesth.Models.Constants;
using ProjetoE_CommerceGameFesth.Repository;
using ProjetoE_CommerceGameFesth.Repository.Contract;

namespace ProjetoE_CommerceGameFesth.Areas.Funcionario.Controllers
{
    [Area("Funcionario")]
    [FuncionarioAutorizacao(FuncionarioTipoConstant.Gerente)]
    public class FuncionarioController : Controller
    {
        private IFuncionarioRepository _funcionarioRepository;
        private LoginFuncionario _loginfuncionario;
        public FuncionarioController(IFuncionarioRepository funcionarioRepository, LoginFuncionario loginfuncionario)
        {
            _funcionarioRepository = funcionarioRepository;
            _loginfuncionario = loginfuncionario;
        }
        public IActionResult Index()
        {
            return View(_funcionarioRepository.ObterFuncionarioList());
        }
        [HttpGet]
        public IActionResult Cadastrar()
        {
            return View();
        }
        [FuncionarioAutorizacao]
        public IActionResult DetalhesFunc(int id)
        {

            Models.Funcionario funcionario = _funcionarioRepository.ObterFuncionario(id);
            if (funcionario.Tipo != FuncionarioTipoConstant.Comum)
            {
                ViewBag.Cargo = "Gerente";
            }
            else
            {
                ViewBag.Cargo = "Comum";
            }
            return View(_funcionarioRepository.ObterFuncionario(id));
        }
        [HttpPost]
        public IActionResult Cadastrar(Models.Funcionario funcionario) 
        {
            var EMAILexit = _funcionarioRepository.ObterEmailFuncionario(funcionario.Email).Email;
            if (!string.IsNullOrEmpty(EMAILexit))
            {
                ViewData["MSG_Email"] = "Email já cadastrado, por favor verifique o email digitado";
                return View();
            }
            else if (ModelState.IsValid)
            {
                funcionario.Tipo = FuncionarioTipoConstant.Comum;
                _funcionarioRepository.Cadastrar(funcionario);
                TempData["MSG_S"] = "Funcionario cadastrado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        [HttpGet]
        [ValidateHttpRefer]
        public IActionResult Atualizar(int id) 
        {
            Models.Funcionario funcionario = _funcionarioRepository.ObterFuncionario(id);
            return View(funcionario);
        }
        [HttpPost]
        public IActionResult Atualizar([FromForm]Models.Funcionario funcionario) 
        {
            _funcionarioRepository.Atualizar(funcionario);
            TempData["MSG_S"] = "Funcionario atualizado com sucesso! ";
            return RedirectToAction(nameof(Index));
        }
        [ValidateHttpRefer]
        public IActionResult Excluir(int id)
        {
            _funcionarioRepository.Excluir(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
