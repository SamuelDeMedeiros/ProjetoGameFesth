using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
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
        public IActionResult Index(string por, string campo, string nome)
        {

            Models.Funcionario func = _loginfuncionario.GetFuncionario();
            ViewBag.Funcionario = func.IdFuncionario;
            if (por != null && campo != null)
            {

                return View(_funcionarioRepository.ObterFuncionarioList(por, campo));

            }
            else if (nome != null)
            {
                nome = nome + "%";
                return View(_funcionarioRepository.ObterFuncionarioList(por, campo));
            }
            else
            {
                return View(_funcionarioRepository.ObterFuncionarioList("Por Maior", "Id"));
            }

        }
        public IActionResult DesativadosF()
        {
            IEnumerable<Models.Funcionario> funci = _funcionarioRepository.ObterFuncionarioList("Por Maior", "Id");
            if (funci.Any())
            {
                ViewBag.Func = "v";
            }
            Models.Funcionario func = _loginfuncionario.GetFuncionario();
            ViewBag.Funcionario = func.IdFuncionario;
            return View(_funcionarioRepository.ObterFuncionarioList("Por Maior", "Id"));
        }
        
        [HttpGet]
        public IActionResult Cadastrar()
        {
            return View();
        }
        [FuncionarioAutorizacao]
        public IActionResult DetalhesFunc(int id)
        {
            Models.Funcionario funcionario1 = _funcionarioRepository.ObterSituacaoFuncionario
                (_funcionarioRepository.ObterFuncionario(id).funcionario.id_Login);
            ViewBag.sit = funcionario1.Situacao;

            Models.CadastraEndereco cadastra = _funcionarioRepository.ObterFuncionario(id);
            Models.Funcionario funcionario = _loginfuncionario.GetFuncionario();
            if (funcionario.IdFuncionario == 1)
            {
                ViewBag.Cargo = "Gerente";
            }
            else
            {
                ViewBag.Cargo = "Subgerente";
            }
            

            if (cadastra.funcionario.Tipo != FuncionarioTipoConstant.Comum)
            {
                if (cadastra.funcionario.IdFuncionario == 1)
                {
                    ViewBag.Cargo1 = "Gerente";
                }
                else
                {
                    ViewBag.Cargo1 = "Subgerente";
                }
                
            }
            else
            {
                ViewBag.Cargo1 = "Comum";
            }
            return View(_funcionarioRepository.ObterFuncionario(id));
        }
        public IActionResult Ativar(int id)
        {
            _funcionarioRepository.Ativar(id);
            return RedirectToAction(nameof(Index));
        }
        [ValidateHttpRefer]
        public IActionResult Desativar(int id)
        {
            _funcionarioRepository.Desativar(id);
            return RedirectToAction(nameof(Index));

        }
        public IActionResult DetalhesFuncDesativado(int id)
        {

            Models.CadastraEndereco cadastra = _funcionarioRepository.ObterFuncionario(id);
            Models.Funcionario funcionario = _loginfuncionario.GetFuncionario();
            if (funcionario.IdFuncionario == 1)
            {
                ViewBag.Cargo = "Gerente";
            }
            else
            {
                ViewBag.Cargo = "Subgerente";
            }


            if (cadastra.funcionario.Tipo != FuncionarioTipoConstant.Comum)
            {
                if (cadastra.funcionario.IdFuncionario == 1)
                {
                    ViewBag.Cargo1 = "Gerente";
                }
                ViewBag.Cargo1 = "Subgerente";
            }
            else
            {
                ViewBag.Cargo1 = "Comum";
            }
            return View(_funcionarioRepository.ObterFuncionario(id));
        }
        [HttpPost]
        public IActionResult Cadastrar(Models.CadastraEndereco cadastra) 
        {
            var EMAILexit = _funcionarioRepository.ObterEmailFuncionario(cadastra.funcionario.Email).Email;
            if (!string.IsNullOrEmpty(EMAILexit))
            {
                ViewData["MSG_Email"] = "Email já cadastrado, por favor verifique o email digitado";
                return View();
            }
            else if (ModelState.IsValid)
            {
                cadastra.funcionario.Tipo = FuncionarioTipoConstant.Comum;
                _funcionarioRepository.Cadastrar(cadastra);
                TempData["MSG_S"] = "Funcionario cadastrado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        [HttpGet]
        [ValidateHttpRefer]
        public IActionResult Atualizar(int id) 
        {
            Models.CadastraEndereco cadastra = _funcionarioRepository.ObterFuncionario(id);
            return View(cadastra);
        }
        [HttpPost]
        public IActionResult Atualizar([FromForm] Models.CadastraEndereco cadastra) 
        {
            if(ModelState.IsValid) 
            {
                _funcionarioRepository.Atualizar(cadastra);
                TempData["MSG_S"] = "Funcionario atualizado com sucesso! ";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        [ValidateHttpRefer]
        public IActionResult Excluir(int id)
        {
            _funcionarioRepository.Excluir(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
