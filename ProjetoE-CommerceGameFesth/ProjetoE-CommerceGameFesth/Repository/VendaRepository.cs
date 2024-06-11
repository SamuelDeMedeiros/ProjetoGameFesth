﻿using MySql.Data.MySqlClient;
using ProjetoE_CommerceGameFesth.Models;
using ProjetoE_CommerceGameFesth.Repository.Contract;
using System.Data;

namespace ProjetoE_CommerceGameFesth.Repository
{
    public class VendaRepository : IVendaRepository
    {
        private readonly string _conexaoMySQL;
        public VendaRepository(IConfiguration conf)
        {
            _conexaoMySQL = conf.GetConnectionString("ConexaoMySQL");
        }

        public void buscaIdVenda(Venda venda)
        {
            throw new NotImplementedException();
        }

        public void Cadastrar(Venda venda,Item item)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("call RegistrarVenda(@nf, @email, @qtd, @CodBarras)", conexao);
                cmd.Parameters.Add("@nf", MySqlDbType.Int64).Value = venda.NotaFiscal;
                cmd.Parameters.Add("@email", MySqlDbType.VarChar).Value = venda.EmailCliente;
                cmd.Parameters.Add("@CodBarras", MySqlDbType.VarChar).Value = item.Codbarras;
                cmd.Parameters.Add("@qtd", MySqlDbType.VarChar).Value = item.Quantidade;
                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }

        public IEnumerable<Venda> ObterTodasCompras()
        {
            List<Venda> Vendalist = new List<Venda>();
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select * from Tb_Vendas", conexao);
                MySqlDataAdapter sd = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                sd.Fill(dt);
                conexao.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    Vendalist.Add(
                        new Venda
                        {
                            NotaFiscal = (int)(dr["Nf"]),
                            ValorTotal = (decimal)(dr["ValorTotal"]),
                            DataVenda = (DateTime)(dr["Data_"]),
                            IdCliente = Convert.ToInt32(dr["Id_cli"])
                        });
                }
                return Vendalist;
            }
        }
        public Venda ObterVenda(int Id)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select * from Tb_Vendas where Nf=@NotaFiscal", conexao);
                cmd.Parameters.Add("@NotaFiscal", MySqlDbType.Int64).Value = Id;

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;

                Venda venda = new Venda();
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    venda.NotaFiscal = (int)(dr["Nf"]);
                    venda.ValorTotal = (decimal)(dr["ValorTotal"]);
                    venda.DataVenda = (DateTime)(dr["Data_"]);
                    venda.IdCliente = Convert.ToInt32(dr["Id_cli"]);

                }
                return venda;
            }
        }
        public Int64 ObtemNF()
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                Int64 NF = 0;
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT Nf from Tb_Vendas ORDER BY Nf DESC LIMIT 1", conexao);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;

                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                
                while (dr.Read())
                {
                    NF = Convert.ToInt64(dr["Nf"]);
                }
                NF++;
                return NF;
            }
        }
    }
}