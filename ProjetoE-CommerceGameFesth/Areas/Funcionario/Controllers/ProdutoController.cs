using Microsoft.AspNetCore.Mvc;
using ProjetoE_CommerceGameFesth.GerenciaArquivos;
using ProjetoE_CommerceGameFesth.Libraries.Filtro;
using ProjetoE_CommerceGameFesth.Libraries.Login;
using ProjetoE_CommerceGameFesth.Models;
using ProjetoE_CommerceGameFesth.Repository.Contract;

namespace ProjetoE_CommerceGameFesth.Areas.Funcionario.Controllers
{
    [Area("Funcionario")]
    public class ProdutoController : Controller
    {
        private IFuncionarioRepository _funcionarioRepository;
        private IProdutoRepository _produtoRepository;
        public ProdutoController (IFuncionarioRepository funcionarioRepository,IProdutoRepository produtorepository)
        {
            _funcionarioRepository = funcionarioRepository;
            _produtoRepository = produtorepository;
        }
        public IActionResult Index()
        {
            return View(_produtoRepository.ObterTodosProdutos());
        }
        public IActionResult DetalhesProd(Int64 Id)
        {
            
            return View(_produtoRepository.ObterProduto(Id));
        }
        [HttpGet]
        public IActionResult Cadastrar()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Cadastrar(Produto produto, IFormFile file)
        {

            
            if (ModelState.IsValid)
            {
                var Caminho = GerenciadorArquivos.CadastrarImagemProduto(file);
                produto.ImagemProduto = Caminho;
                _produtoRepository.Adicionar(produto);
                ViewBag.msg = "Produto adicionado";
                return RedirectToAction("Index", "Produto");
            }
            return View();


        }
        public IActionResult Deletar(Int64 id)
        {
            _produtoRepository.Apagar(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
