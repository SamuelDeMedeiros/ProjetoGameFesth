using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult FrontPage(string por, string campo, string pesquisa)
        {
            if (por != null && campo != null)
            {
                if (pesquisa != null)
                {
                    ViewBag.Pesquisa = pesquisa;
                    ViewBag.Por = por;
                    ViewBag.Campo = campo;


                    return View(_produtoRepository.ObterTodosProdutos(por, campo, pesquisa));

                }
                else
                {
                    return View(_produtoRepository.ObterTodosProdutos(por, campo, ""));
                }
                

            }
            
            else
            {
                return View(_produtoRepository.ObterTodosProdutos("Por Maior", "CodBarras", ""));
            }


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
            return Json(new { success = true });
        }  
        public IActionResult SalvarCarrinho(Venda venda)
        {
            if (_loginCliente.GetCliente() != null)
            {
                List<Produto> carrinho = _cookieCarrinhoCompra.Consultar();

                Venda mdE = new Venda();
                Item mdI = new Item();
                Int64 NF;
                
                NF = _vendaRepository.ObtemNF();

                for (int i = 0; i < carrinho.Count; i++)
                {
                    mdE.NotaFiscal = Convert.ToInt32(NF);
                    mdE.EmailCliente = _loginCliente.GetCliente().Email;
                    mdI.Codbarras = (Int64)carrinho[i].Codbarras;
                    mdI.Quantidade = carrinho[i].Quantidade;
                    _vendaRepository.Cadastrar(mdE, mdI);

                    // _itemRepository.Adicionar(mdI);
                }

                _cookieCarrinhoCompra.RemoverTodos();
                return RedirectToAction("confVenda", new { id = mdE.NotaFiscal });
            }
            return RedirectToAction("Login");
        }
        public IActionResult confVenda(int id)
        {
            Venda clienteV = _clienteRepository.ObterVendaCliente(id);
            CadastraEndereco cliente = _clienteRepository.ObterCliente(clienteV.IdCliente);
            ViewBag.Email = cliente.cliente.Email ;
            return View(clienteV);
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
        public IActionResult Login([FromForm] Models.Login login)
        {
            Models.Login loginDB = _loginRepository.Login(login.Email, login.Senha);

            if(loginDB.Senha != null && loginDB.Email != null && loginDB.tipo_login == LoginTipoConstant.Cliente)
            {
                Cliente sit = _clienteRepository.ObterSituacaoCliente(loginDB.id_Login);
                if (sit.Situacao == SituacaoConstant.Ativo && sit.Situacao != null)
                {
                    Cliente clienteDB = _clienteRepository.Login(loginDB.id_Login);
                    _loginCliente.Login(clienteDB);
                    return new RedirectResult(Url.Action(nameof(PainelCliente)));
                }

                ViewData["MSG_E"] = "Conta desativada, por favor contatar os administradores!";
                return View();
            }
            else if (loginDB.Senha != null && loginDB.Email != null && loginDB.tipo_login == LoginTipoConstant.Funcionario)
            {
                Funcionario sit = _funcionarioRepository.ObterSituacaoFuncionario(loginDB.id_Login);
                if (sit.Situacao == SituacaoConstant.Ativo && sit.Situacao != null)
                {
                    Funcionario funcionarioDB = _funcionarioRepository.Login(loginDB.id_Login);
                    _loginfuncionario.Login(funcionarioDB);
                    //return RedirectToAction("Painel", "Home", new { area = "~/Funcionario" });
                    return new RedirectResult("~/Funcionario/Home/Painel");
                }
                ViewData["MSG_E"] = "Conta desativada, por favor contatar os administradores!";
                return View();
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
        public IActionResult VerificaEmail([FromForm] Models.Login login)
        {
          
            Models.Login loginDB = _loginRepository.Login(login.Email);

            if ( loginDB.Email != null && loginDB.tipo_login == LoginTipoConstant.Cliente)
            {
                Cliente sit = _clienteRepository.ObterSituacaoCliente(loginDB.id_Login);
                if (sit.Situacao == SituacaoConstant.Ativo && sit.Situacao != null)
                {
                    Cliente clienteDB = _clienteRepository.Login(loginDB.id_Login);
                    ViewBag.Id = clienteDB.IdCliente;
                    clienteDB.Senha = loginDB.GerarCodigo();
                    _clienteRepository.AtualizarSenha(clienteDB);
                    _loginRepository.EnviaEmail(clienteDB.Email, "Nova senha", $"Sua nova senha é {clienteDB.Senha}");
                    return RedirectToAction("Login");
                }

                ViewData["MSG_E"] = "Conta desativada, por favor contatar os administradores!";
                return View();
            }
            else if ( loginDB.Email != null && loginDB.tipo_login == LoginTipoConstant.Funcionario)
            {
                Funcionario sit = _funcionarioRepository.ObterSituacaoFuncionario(loginDB.id_Login);
                if (sit.Situacao == SituacaoConstant.Ativo && sit.Situacao != null)
                {
                    Funcionario funcionarioDB = _funcionarioRepository.Login(loginDB.id_Login);
                    ViewBag.Id = funcionarioDB.IdFuncionario;
                    funcionarioDB.Senha = loginDB.GerarCodigo();
                    _funcionarioRepository.AtualizarSenha(funcionarioDB);
                    _loginRepository.EnviaEmail(funcionarioDB.Email, "Nova senha", $"Sua nova senha é {funcionarioDB.Senha}");
                    return RedirectToAction("Login");
                }
                ViewData["MSG_E"] = "Conta desativada, por favor contatar os administradores!";
                return View();
            }
            else
            {
                ViewData["MSG_E"] = "Registro não encontrado";
                return View();
            }

        }

        public IActionResult PainelCliente()
        {
            ViewBag.IdCliente = _loginCliente.GetCliente().IdCliente;
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
            return RedirectToAction(nameof(FrontPage));
        }
        public IActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cadastrar([FromForm] CadastraEndereco cadastraEndereco)
        {
            if(cadastraEndereco.endereco.CEP != null) 
            {
                cadastraEndereco.endereco = _clienteRepository.ObterEndereco(cadastraEndereco.endereco.CEP.Replace(".", "").Replace("-", ""));
            }
            

            var CPFexit = _clienteRepository.ObterCpfCliente(cadastraEndereco.cliente.CPF).CPF;
            var EMAILexit = _clienteRepository.ObterEmailCliente(cadastraEndereco.cliente.Email).Email;
            int Ano = cadastraEndereco.cliente.Nascimento.Year;
            int Anohj = DateTime.Now.Year;
            int AnoLimite = 1920;
            if (Ano < AnoLimite || Ano > Anohj)
            {
                ViewData["MSG_Data"] = "Data invalida";
                return View();
            }
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
            else if(ModelState.IsValid)
            {
                _clienteRepository.Cadastrar(cadastraEndereco);
                TempData["MSG_S"] = "Registro salvo com sucesso!";
                return RedirectToAction(nameof(Cadastrar));
            }
            return View();
        }
        public IActionResult AtualizarP(string email)
        {
            return View(_clienteRepository.ObterClientePorEmail(email));
        }

        [HttpPost]
        public IActionResult AtualizarP([FromForm] Cliente cliente)
        {

            if (ModelState.IsValid)
            {
                _clienteRepository.AtualizarP(cliente);
                _loginCliente.Atualizar(cliente);
                TempData["MSG_S"] = "Registro atualizado com sucesso!";
                return RedirectToAction(nameof(PainelCliente));
            }
            return View();
        }
        public IActionResult sobre()
        {
            return View();
        }
        public IActionResult GetCartItemCount()
        {
            var carrinho = _cookieCarrinhoCompra.Consultar();
            int itemCount = carrinho.Sum(item => item.Quantidade);
            return Json(new { count = itemCount });
        }
        public IActionResult DetalhesVendas(int Id)
        {
            return View(_clienteRepository.ObterVendaList(Id));
        }
    }
}
