using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI;
using ProjetoE_CommerceGameFesth.Areas.Funcionario.Controllers;
using ProjetoE_CommerceGameFesth.CarrinhoCompra;
using ProjetoE_CommerceGameFesth.Libraries.Filtro;
using ProjetoE_CommerceGameFesth.Libraries.Login;
using ProjetoE_CommerceGameFesth.Models;
using ProjetoE_CommerceGameFesth.Models.Constants;
using ProjetoE_CommerceGameFesth.Repository;
using ProjetoE_CommerceGameFesth.Repository.Contract;
using System;
using System.Data;
using System.Diagnostics;

namespace ProjetoE_CommerceGameFesth.Controllers
{
    public class HomeController : Controller
    {
        private IClienteRepository _clienteRepository;
        private IFuncionarioRepository _funcionarioRepository;
        private LoginCliente _loginCliente;
        private LoginFuncionario _loginfuncionario;
        private IProdutoRepository _produtoRepository;
        private IItemRepository _itemRepository;
        private IVendaRepository _vendaRepository;
        private ILoginRepository _loginRepository;
        private CookieCarrinhoCompra _cookieCarrinhoCompra;

        DateTime data;
        public HomeController(IClienteRepository clienteRepository, LoginCliente loginCliente,   IProdutoRepository produtoRepository, CookieCarrinhoCompra cookieCarrinhoCompra, IItemRepository itemRepository, IVendaRepository vendaRepository,ILoginRepository loginRepository, IFuncionarioRepository funcionarioRepository, LoginFuncionario loginFuncionario)
        {
            _loginfuncionario = loginFuncionario;
            _funcionarioRepository = funcionarioRepository;
            _loginRepository = loginRepository;
            _clienteRepository = clienteRepository;
            _loginCliente = loginCliente;
            _produtoRepository = produtoRepository;
            _itemRepository = itemRepository;
            _vendaRepository = vendaRepository;
            _cookieCarrinhoCompra = cookieCarrinhoCompra;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult FrontPage()
        {
            return View(_produtoRepository.ObterTodosProdutos());
        }
        public IActionResult AdicionarItem(Int64 id)
        {
            Produto produto = _produtoRepository.ObterProduto(id);

            if (produto == null)
            {
                return View("NaoExisteItem");
            }
            else
            {
                var item = new Produto()
                {
                    Codbarras = id,
                    QuantidadeEstoque = produto.QuantidadeEstoque,
                    ImagemProduto = produto.ImagemProduto,
                    NomeProduto = produto.NomeProduto,
                    Valor = produto.Valor,
                };

                _cookieCarrinhoCompra.Cadastrar(item);
                
                return RedirectToAction(nameof(Carrinho));
            }
        }
        public IActionResult Carrinho()
        {
            return View(_cookieCarrinhoCompra.Consultar());
        }
        public IActionResult DiminuirItem(Int64 id)
        {
            Produto produto = _produtoRepository.ObterProduto(id);
            if (produto == null)
            {
                return View("NaoExisteItem");
            }
            else
            {
                var item = new Produto()
                {
                    Codbarras = id,
                    QuantidadeEstoque = produto.QuantidadeEstoque,
                    ImagemProduto = produto.ImagemProduto,
                    NomeProduto = produto.NomeProduto,
                    Valor = produto.Valor,
                };

                _cookieCarrinhoCompra.DiminuirProduto(item);


                return RedirectToAction(nameof(Carrinho));
            }
        }
        public IActionResult RemoverItem(Int64 id) 
        {
            _cookieCarrinhoCompra.Remover(new Produto() { Codbarras = id });
            return RedirectToAction(nameof(Carrinho));
        }
        [ClienteAutorizacao]
        public IActionResult SalvarCarrinho(Venda venda)
        {
            List<Produto> carrinho = _cookieCarrinhoCompra.Consultar();

            Venda mdE = new Venda();
            Item mdI = new Item();

            data = DateTime.Now.ToLocalTime();

            mdE.DataVenda = data;
            mdE.IdCliente = _loginCliente.GetCliente().IdCliente;
            _vendaRepository.Cadastrar(mdE);

            _vendaRepository.buscaIdVenda(venda);

            for(int i = 0; i < carrinho.Count; i++)
            {
                mdI.NotaFiscal = Convert.ToInt32(venda.NotaFiscal);
                mdI.Codbarras = (int)Convert.ToInt64(carrinho[i].Codbarras);

                _itemRepository.Adicionar(mdI);
            }

            _cookieCarrinhoCompra.RemoverTodos();
            return RedirectToAction("confVenda");
        }
        public IActionResult confVenda()
        {
            return View();
        }
        public IActionResult Detalhes(Int64 Id) 
        {
            return View(_produtoRepository.ObterProduto(Id));
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login([FromForm] Login login)
        {
            Login loginDB = _loginRepository.Login(login.Email, login.Senha);

            if(loginDB.Senha != null && loginDB.Email != null && loginDB.tipo_login == LoginTipoConstant.Cliente)
            {
                Cliente clienteDB = _clienteRepository.Login(loginDB.id_Login);
                
                
                    _loginCliente.Login(clienteDB);
                    return new RedirectResult(Url.Action(nameof(PainelCliente)));
                
                
            }else if (loginDB.Senha != null && loginDB.Email != null && loginDB.tipo_login == LoginTipoConstant.Funcionario)
            {
                Funcionario funcionarioDB = _funcionarioRepository.Login(loginDB.id_Login);

                    _loginfuncionario.Login(funcionarioDB);
                //return RedirectToAction("Painel", "Home", new { area = "~/Funcionario" });
                return new RedirectResult("~/Funcionario/Home/Painel");
            }
            else
            {
                ViewData["MSG_E"] = "Registro não encontrado";
                return View();
            }

        }
        public IActionResult VerificaEmail()
        {
            return View();
        }
        [HttpPost]
        public IActionResult VerificaEmail([FromForm] Cliente cliente)
        {
            Cliente clienteDB = _clienteRepository.ObterClientePorEmail(cliente.Email);
            if (clienteDB.Email != null)
            {
                if (clienteDB.Situacao != SituacaoConstant.Desativado)
                {
                    ViewBag.Id = clienteDB.IdCliente;


                    return new RedirectResult(Url.Action(nameof(AtualizarSenha)));
                }
                else
                {
                    TempData["MSG_E"] = "Usuario desativado, por favor contatar os administradores!";
                    return View();
                }
            }
            else
            {
                TempData["MSG_E"] = "Usuario não localizado, por favor verifique o e-mail digitado";
                return View();
            }
        }

        [HttpGet]
        [ValidateHttpRefer]
        public IActionResult AtualizarSenha(int id)
        {
            CadastraEndereco cadastraEndereco = _clienteRepository.ObterCliente(id);
            return View(cadastraEndereco);
        }
        [HttpPost]
        public IActionResult AtualizarSenha([FromForm] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                _loginCliente.Atualizar(cliente);
                _clienteRepository.AtualizarSenha(cliente);
                ViewData["MSG_S"] = "Senha atualizada com sucesso! ";
                return RedirectToAction(nameof(Login));
            }
            ViewData["MSG_E"] = "Erro ao atualizar senha!";
            return View();

        }
        //public IActionResult Login([FromForm] Models.Funcionario funcionario)
        //{
        //    Models.Funcionario funcionarioDB = _funcionarioRepository.Login(funcionario.Email, funcionario.Senha);

        //    if (funcionarioDB.Email != null && funcionarioDB.Senha != null)
        //    {
        //        _loginfuncionario.Login(funcionarioDB);
        //        return new RedirectResult(Url.Action(nameof(Painel)));
        //    }
        //    else
        //    {
        //        ViewData["MSG_E"] = "Funcionario não encontrado, verifique os dados inseridos!";
        //        return View();
        //    }
        //}

        public IActionResult PainelCliente()
        {
            ViewBag.Nome = _loginCliente.GetCliente().NomeCliente;
            ViewBag.Email = _loginCliente.GetCliente().Email;
            ViewBag.Nascimento = _loginCliente.GetCliente().Nascimento.ToString("d/MM/yyyy");
            ViewBag.Sexo = _loginCliente.GetCliente().Sexo;
            ViewBag.Telefone = _loginCliente.GetCliente().Telefone;


            return View();
        }
        [ClienteAutorizacao]
        public IActionResult LogoutCliente()
        {
            _loginCliente.Logout();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cadastrar([FromForm] CadastraEndereco cadastraEndereco)
        {
            cadastraEndereco.cliente.Situacao = SituacaoConstant.Ativo;
            var CPFexit = _clienteRepository.ObterCpfCliente(cadastraEndereco.cliente.CPF).CPF;
            var EMAILexit = _clienteRepository.ObterEmailCliente(cadastraEndereco.cliente.Email).Email;
            if(!string.IsNullOrEmpty(CPFexit))
            {
                ViewData["MSG_CPF"] = "CPF já cadastrado, por favor verifique o cpf digitado";
                return View();
            }
            else if (!string.IsNullOrEmpty(EMAILexit))
            {
                ViewData["MSG_Email"] = "Email já cadastrado, por favor verifique o email digitado";
                return View();
            }
            else if (ModelState.IsValid)
            {

                _clienteRepository.Cadastrar(cadastraEndereco);
                TempData["MSG_S"] = "Registro salvo com sucesso!";
                return RedirectToAction(nameof(Cadastrar));
            }
            return View();
        }
    }
}
