using Microsoft.AspNetCore.Mvc;
using ProjetoE_CommerceGameFesth.Models;
using ProjetoE_CommerceGameFesth.Repository.Contract;
using System.Collections.Generic;

namespace ProjetoE_CommerceGameFesth.Areas.Funcionario.Controllers
{
    [Area("Funcionario")]
    public class VendaController : Controller
    {
        private IVendaRepository _vendaRepository;
        private IClienteRepository _clienteRepository;

        public VendaController(IVendaRepository vendaRepository, IClienteRepository clienteRepository)
        {
            _vendaRepository = vendaRepository;
            _clienteRepository = clienteRepository;
        }

        public IActionResult Index()
        {
            

            return View(_vendaRepository.ObterTodasCompras());
        }
        public IActionResult DetalhesVenda(int id)
        {
            ViewBag.NotaFiscal = id;
            Venda venda = new Venda();
            venda = _vendaRepository.ObterCodCli(id);
            CadastraEndereco cadastra = _clienteRepository.ObterCliente(venda.IdCliente);
            ViewBag.Email = cadastra.cliente.Email;
            ViewBag.Nome = cadastra.cliente.NomeCliente;
            ViewBag.Valor = venda.ValorTotal;
            IEnumerable<Produto> listCod = _vendaRepository.ObterCodBarras(id);

            return View(listCod);
        }
        public IActionResult DetalhesProduto(Int64 id, int nf)
        {
            ViewBag.NotaFiscal = nf;
            DescricaoVenda desc = _vendaRepository.ObterVenda(nf, id);
            CadastraEndereco cadastra = _clienteRepository.ObterCliente(desc.venda.IdCliente);
            ViewBag.Email = cadastra.cliente.Email;
            ViewBag.Nome = cadastra.cliente.NomeCliente;
            

            return View(desc);
        }
    }
}
