using Microsoft.AspNetCore.Mvc;
using ProjetoE_CommerceGameFesth.Repository.Contract;

namespace ProjetoE_CommerceGameFesth.Areas.Funcionario.Controllers
{
    [Area("Funcionario")]
    public class VendaController : Controller
    {
        private IVendaRepository _vendaRepository;

        public VendaController(IVendaRepository vendaRepository)
        {
            _vendaRepository = vendaRepository;
        }

        public IActionResult Index()
        {

            return View(_vendaRepository.ObterTodasCompras());
        }
        public IActionResult DetalhesVenda(int id)
        {
            return View(_vendaRepository.ObterVenda(id));
        }
    }
}
