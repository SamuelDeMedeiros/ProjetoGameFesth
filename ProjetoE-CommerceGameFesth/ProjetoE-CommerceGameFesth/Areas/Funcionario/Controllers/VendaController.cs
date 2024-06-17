using Microsoft.AspNetCore.Mvc;
using ProjetoE_CommerceGameFesth.Models;
using ProjetoE_CommerceGameFesth.Repository.Contract;

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
            DescricaoVenda desc = _vendaRepository.ObterVenda(id);
            CadastraEndereco cadastra = _clienteRepository.ObterCliente(desc.venda.IdCliente);
            ViewBag.Email = cadastra.cliente.Email;
            ViewBag.Nome = cadastra.cliente.NomeCliente;

            return View(desc);
        }
    }
}
