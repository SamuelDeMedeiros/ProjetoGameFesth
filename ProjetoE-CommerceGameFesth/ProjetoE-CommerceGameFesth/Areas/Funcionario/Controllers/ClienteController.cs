﻿using Microsoft.AspNetCore.Mvc;
using ProjetoE_CommerceGameFesth.Libraries.Filtro;
using ProjetoE_CommerceGameFesth.Models;
using ProjetoE_CommerceGameFesth.Models.Constants;
using ProjetoE_CommerceGameFesth.Repository.Contract;

namespace ProjetoE_CommerceGameFesth.Areas.Funcionario.Controllers
{
    [Area("Funcionario")]
    public class ClienteController : Controller
    {
        private IClienteRepository _clienteRepository;
        public ClienteController(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }
        public IActionResult Index()
        {
            IEnumerable<Cliente> cli = _clienteRepository.ObterClienteList();
            if (cli.Any())
            {
                ViewBag.Cliente = "v";
            }
            return View(_clienteRepository.ObterClienteList());
        }
        public IActionResult DetalhesCli(int id)
        {
            Cliente cad = _clienteRepository.ObterSituacaoCliente(_clienteRepository.ObterCliente(id).cliente.IdLog);
            ViewBag.sit = cad.Situacao;
            return View(_clienteRepository.ObterCliente(id));
        }
        public IActionResult Desativados()
        {
            IEnumerable<Cliente> cli = _clienteRepository.ObterClienteList();
            if (cli.Any())
            {
                ViewBag.Cliente = "v";
            }
            return View(_clienteRepository.ObterClienteList());
        }
        public IActionResult DetalhesCliDesativado(int id)
        {
            Cliente cad = _clienteRepository.ObterSituacaoCliente(_clienteRepository.ObterCliente(id).cliente.IdLog);
            ViewBag.sit = cad.Situacao;
            return View(_clienteRepository.ObterCliente(id));
        }
        //public IActionResult Cadastrar()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public IActionResult Cadastrar([FromForm] Cliente cliente)
        //{
        //    cliente.Situacao = SituacaoConstant.Ativo;
        //    if(ModelState.IsValid) 
        //    {
        //        _clienteRepository.Cadastrar(cliente);
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View();
        //}
        [ValidateHttpRefer]
        public IActionResult Ativar(int id) 
        {
            _clienteRepository.Ativar(id);
            return RedirectToAction(nameof(Index));
        }
        [ValidateHttpRefer]
        public IActionResult Desativar(int id) 
        {
            _clienteRepository.Desativar(id);
            return RedirectToAction(nameof(Index));

        }
    }
}
