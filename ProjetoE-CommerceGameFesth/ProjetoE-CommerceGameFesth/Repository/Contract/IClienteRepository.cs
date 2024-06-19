﻿using ProjetoE_CommerceGameFesth.Models;

namespace ProjetoE_CommerceGameFesth.Repository.Contract
{
    public interface IClienteRepository
    {

        Cliente Login(int id);

        void Cadastrar(CadastraEndereco cadastraEndereco);
        void AtualizarDados(Cliente cliente);
        void AtualizarEmail(Cliente cliente);
        void AtualizarSenha(Cliente cliente);
        void Excluir(int Id);
        CadastraEndereco ObterCliente(int Id);
        void Ativar(int id);
        void Desativar(int id);
        Cliente ObterClientePorEmail(string email);
        Cliente ObterCpfCliente(string CPF);
        public Cliente ObterCNPJCliente(string CNPJ);
        Cliente ObterEmailCliente(string email);
        IEnumerable<Cliente> ObterClienteList(string por, string campo);
        Cliente ObterSituacaoCliente(int id);
        Venda ObterVendaCliente(int id);
        public IEnumerable<Venda> ObterVendaList(int id);
        public Endereco ObterEndereco(string cep, string num);
    }
}
