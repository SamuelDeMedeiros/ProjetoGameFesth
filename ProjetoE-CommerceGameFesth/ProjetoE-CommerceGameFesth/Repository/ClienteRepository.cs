using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using NuGet.Protocol.Plugins;
using ProjetoE_CommerceGameFesth.Models;
using ProjetoE_CommerceGameFesth.Models.Constants;
using ProjetoE_CommerceGameFesth.Repository.Contract;
using System.Collections.Generic;
using System.Data;

namespace ProjetoE_CommerceGameFesth.Repository
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly string _conexaoMySQL;

        public ClienteRepository(IConfiguration conf)
        {
            _conexaoMySQL = conf.GetConnectionString("ConexaoMySQL");
        }

        public void Ativar(int id)
        {
            string Situacao = SituacaoConstant.Ativo;
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("update Tb_Cliente set Situacao=@situacao where Id_cliente=@id", conexao);
                cmd.Parameters.Add("@id", MySqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("@situacao", MySqlDbType.VarChar).Value = Situacao;
                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }
        public Cliente ObterCpfCliente(string CPF)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select cpf from Tb_pf where cpf=@cpf", conexao);
                cmd.Parameters.AddWithValue("@cpf", CPF);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;
                Cliente cliente = new Cliente();
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    cliente.CPF = (string)(dr["cpf"]);

                }
                return cliente;
            }
        }
        public Cliente ObterCNPJCliente(string CNPJ)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select cnpj from Tb_pj where cnpj=@cnpj", conexao);
                cmd.Parameters.AddWithValue("@cnpj", CNPJ);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;
                Cliente cliente = new Cliente();
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    cliente.CNPJ = (string)(dr["cnpj"]);
                }
                return cliente;
            }
        }
        public Cliente ObterEmailCliente(string email)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select Email from Tb_login where Email=@email", conexao);
                cmd.Parameters.AddWithValue("@email", email);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;
                Cliente cliente = new Cliente();
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    cliente.Email = (string)(dr["Email"]);

                }
                return cliente;
            }
        }
        public Cliente ObterClientePorEmail(string email)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select * from vw_ClienteEnd where Email=@email", conexao);
                cmd.Parameters.AddWithValue("@email", email);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;

                Cliente cliente = new Cliente();
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    cliente.IdCliente = Convert.ToInt32(dr["Id_cliente"]);
                    cliente.IdLog = (Int32)(dr["Id_log"]);
                    cliente.NomeCliente = (string)dr["Nome"];
                    cliente.Nascimento = (DateTime)dr["Nascimento"];
                    cliente.Sexo = (string)dr["Sexo"];

                    cliente.Telefone = (string)(dr["Telefone"]);
                    cliente.Situacao = (string)dr["Situacao"];
                    cliente.Email = (string)dr["Email"];
                    cliente.Senha = (string)dr["Senha"];
                }
                return cliente;
            }
        }


        public void AtualizarDados(Cliente cliente)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE Tb_Cliente SET Nome = @nome, Sexo = @sexo, Telefone = @telefone WHERE Id_cliente = @id", conexao);
                
                cmd.Parameters.Add("@id", MySqlDbType.VarChar).Value = cliente.IdCliente;
                cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = cliente.NomeCliente;
                cmd.Parameters.Add("@sexo", MySqlDbType.VarChar).Value = cliente.Sexo;
                cmd.Parameters.Add("@telefone", MySqlDbType.VarChar).Value = cliente.Telefone.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "");

                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }
        public void AtualizarEmail(Cliente cliente)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("update Tb_login set email=@email where id_Login=@id", conexao);

                cmd.Parameters.Add("@id", MySqlDbType.VarChar).Value = cliente.IdLog;
                cmd.Parameters.Add("@email", MySqlDbType.VarChar).Value = cliente.Email;

                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }
        public void AtualizarSenha(Cliente cliente)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("update Tb_login set senha=@Senha where id_Login=@id", conexao);

                cmd.Parameters.Add("@id", MySqlDbType.VarChar).Value = cliente.IdLog;
                cmd.Parameters.Add("@Senha", MySqlDbType.VarChar).Value = cliente.Senha;

                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }
        public void Cadastrar(CadastraEndereco cadastraEndereco)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                MySqlCommand cmd;
                conexao.Open();
                if (cadastraEndereco.cliente.CNPJ == null)
                {
                    cmd = new MySqlCommand("CALL InserirPF(@Nome, @Nascimento, @Sexo, @Telefone, @Rg, @CPF, @CEP, @Logradouro, @Bairro,@Num, " +
                    "@NomeCid, @NomeUF, @Email, @Senha);", conexao);

                    cmd.Parameters.Add("@Nome", MySqlDbType.VarChar).Value = cadastraEndereco.cliente.NomeCliente;
                    cmd.Parameters.Add("@Nascimento", MySqlDbType.DateTime).Value = cadastraEndereco.cliente.Nascimento.ToString("yyyy/MM/dd");
                    cmd.Parameters.Add("@Sexo", MySqlDbType.VarChar).Value = cadastraEndereco.cliente.Sexo;
                    cmd.Parameters.Add("@Telefone", MySqlDbType.VarChar).Value = cadastraEndereco.cliente.Telefone.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "");
                    cmd.Parameters.Add("@Rg", MySqlDbType.VarChar).Value = cadastraEndereco.cliente.RG.Replace(".", "").Replace("-", "");
                    cmd.Parameters.Add("@CPF", MySqlDbType.VarChar).Value = cadastraEndereco.cliente.CPF.Replace(".", "").Replace("-", "");
                    cmd.Parameters.Add("@CEP", MySqlDbType.VarChar).Value = cadastraEndereco.endereco.CEP.Replace("-", "");
                    cmd.Parameters.Add("@Logradouro", MySqlDbType.VarChar).Value = cadastraEndereco.endereco.Lougradouro;
                    cmd.Parameters.Add("@Bairro", MySqlDbType.VarChar).Value = cadastraEndereco.endereco.Bairro;
                    cmd.Parameters.Add("@Num", MySqlDbType.VarChar).Value = cadastraEndereco.endereco.NumLougradouro;
                    cmd.Parameters.Add("@NomeCid", MySqlDbType.VarChar).Value = cadastraEndereco.endereco.NomeCidade;
                    cmd.Parameters.Add("@NomeUF", MySqlDbType.VarChar).Value = cadastraEndereco.endereco.NomeUF;
                    cmd.Parameters.Add("@Email", MySqlDbType.VarChar).Value = cadastraEndereco.cliente.Email;
                    cmd.Parameters.Add("@Senha", MySqlDbType.VarChar).Value = cadastraEndereco.cliente.Senha;
                }
                else
                {
                    cmd = new MySqlCommand("CALL InserirPJ(@Nome, @Nascimento, @Sexo, @Telefone, @Cnpj, @IE, @NomeFantasia, @RazaoSocial, @CEP," +
                        " @Logradouro, @Bairro, @Num, @NomeCid, @NomeUF, @Email, @Senha);", conexao);

                    cmd.Parameters.Add("@Nome", MySqlDbType.VarChar).Value = cadastraEndereco.cliente.NomeCliente;
                    cmd.Parameters.Add("@Nascimento", MySqlDbType.DateTime).Value = cadastraEndereco.cliente.Nascimento.ToString("yyyy/MM/dd");
                    cmd.Parameters.Add("@Sexo", MySqlDbType.VarChar).Value = cadastraEndereco.cliente.Sexo;
                    cmd.Parameters.Add("@Telefone", MySqlDbType.VarChar).Value = cadastraEndereco.cliente.Telefone.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "");
                    cmd.Parameters.Add("@Cnpj", MySqlDbType.VarChar).Value = cadastraEndereco.cliente.CNPJ.Replace(".", "").Replace("/", "").Replace("-", "");
                    cmd.Parameters.Add("@IE", MySqlDbType.VarChar).Value = cadastraEndereco.cliente.IE;
                    cmd.Parameters.Add("@NomeFantasia", MySqlDbType.VarChar).Value = cadastraEndereco.cliente.NomeFantasia;
                    cmd.Parameters.Add("@RazaoSocial", MySqlDbType.VarChar).Value = cadastraEndereco.cliente.Razaosocial;
                    cmd.Parameters.Add("@CEP", MySqlDbType.VarChar).Value = cadastraEndereco.endereco.CEP.Replace("-", "");
                    cmd.Parameters.Add("@Logradouro", MySqlDbType.VarChar).Value = cadastraEndereco.endereco.Lougradouro;
                    cmd.Parameters.Add("@Bairro", MySqlDbType.VarChar).Value = cadastraEndereco.endereco.Bairro;
                    cmd.Parameters.Add("@Num", MySqlDbType.VarChar).Value = cadastraEndereco.endereco.NumLougradouro;
                    cmd.Parameters.Add("@NomeCid", MySqlDbType.VarChar).Value = cadastraEndereco.endereco.NomeCidade;
                    cmd.Parameters.Add("@NomeUF", MySqlDbType.VarChar).Value = cadastraEndereco.endereco.NomeUF;
                    cmd.Parameters.Add("@Email", MySqlDbType.VarChar).Value = cadastraEndereco.cliente.Email;
                    cmd.Parameters.Add("@Senha", MySqlDbType.VarChar).Value = cadastraEndereco.cliente.Senha;
                }


                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }

        public void Desativar(int id)
        {
            string Situacao = SituacaoConstant.Desativado;
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("update Tb_Cliente set Situacao=@situacao where Id_cliente=@id", conexao);
                cmd.Parameters.Add("@id", MySqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("@situacao", MySqlDbType.VarChar).Value = Situacao;
                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }

        public void Excluir(int Id)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("delete from Tb_Cliente where Id_cliente=@id", conexao);
                cmd.Parameters.AddWithValue("@id", Id);
                int i = cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }

        public Cliente Login(int id)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM vw_ClienteEnd where id_log = @id ", conexao);
                cmd.Parameters.AddWithValue("@id", id);


                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;

                Cliente cliente = new Cliente();
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    cliente.IdCliente = Convert.ToInt32(dr["Id_cliente"]);
                    cliente.NomeCliente = Convert.ToString(dr["Nome"]);
                    cliente.Nascimento = Convert.ToDateTime(dr["Nascimento"]);
                    cliente.Sexo = Convert.ToString(dr["Sexo"]);
                    cliente.Telefone = (string)(dr["Telefone"]);
                    cliente.Email = Convert.ToString(dr["email"]);
                    cliente.Senha = Convert.ToString(dr["senha"]);
                    //cliente.Situacao = Convert.ToString(dr["Situacao"]);
                }
                return cliente;
            }
        }

        public CadastraEndereco ObterCliente(int Id)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select * from vw_ClienteEnd where Id_cliente=@id", conexao);
                cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = Id;

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;

                CadastraEndereco cadastraEndereco = new CadastraEndereco();
                cadastraEndereco.cliente = new Cliente();
                cadastraEndereco.endereco = new Endereco();
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    cadastraEndereco.cliente.IdCliente = (Int32)(dr["Id_cliente"]);
                    cadastraEndereco.cliente.IdLog = (Int32)(dr["Id_log"]);
                    cadastraEndereco.cliente.NomeCliente = (string)(dr["Nome"]);
                    cadastraEndereco.cliente.Nascimento = (DateTime)(dr["Nascimento"]);
                    cadastraEndereco.cliente.Sexo = (string)(dr["Sexo"]);
                    cadastraEndereco.cliente.Telefone = (string)(dr["Telefone"]);
                    cadastraEndereco.endereco.NumLougradouro = Convert.ToString(dr["Num"]);
                    cadastraEndereco.endereco.CEP = Convert.ToString(dr["CEP"]);
                    cadastraEndereco.endereco.Lougradouro = (string)(dr["Logradouro"]);
                    cadastraEndereco.endereco.NomeUF = (string)(dr["NomeUf"]);
                    cadastraEndereco.endereco.NomeCidade = (string)(dr["NomeCid"]);
                    cadastraEndereco.cliente.Situacao = (string)(dr["Situacao"]);
                    cadastraEndereco.cliente.Email = (string)(dr["Email"]);
                    cadastraEndereco.cliente.Senha = (string)(dr["Senha"]);
                }
                return cadastraEndereco;
            }
        }

        public IEnumerable<Cliente> ObterClienteList(string por, string campo)
        {
            List<Cliente> cliList = new List<Cliente>();
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("call sp_ordenaCliente(@por,@campo); ", conexao);
                cmd.Parameters.Add("@por", MySqlDbType.VarChar).Value = por;
                cmd.Parameters.Add("@campo", MySqlDbType.VarChar).Value = campo;
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);
                conexao.Close();
                foreach (DataRow dr in dt.Rows)
                {
                    cliList.Add(
                        new Cliente
                        {
                            IdCliente = Convert.ToInt32(dr["Id_cliente"]),
                            NomeCliente = Convert.ToString(dr["Nome"]),
                            Nascimento = Convert.ToDateTime(dr["Nascimento"]),
                            Sexo = Convert.ToString(dr["Sexo"]),
                            Telefone = (string)(dr["Telefone"]),
                            Situacao = Convert.ToString(dr["Situacao"])
                        });
                }
                return cliList;
            }
        }
        public Cliente ObterSituacaoCliente(int id)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select Situacao from vw_ClienteEnd where Id_log=@id", conexao);
                cmd.Parameters.AddWithValue("@id", id);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;
                Cliente cliente = new Cliente();
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    cliente.Situacao = (string)(dr["Situacao"]);

                }
                return cliente;
            }
        }
        public Venda ObterVendaCliente(int id)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select * from vw_detaVenda where NotaFiscal=@id", conexao);
                cmd.Parameters.AddWithValue("@id", id);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;
                Venda venda = new Venda();
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    venda.NotaFiscal = (int)(dr["NotaFiscal"]);
                    venda.ValorTotal = (decimal)(dr["Total"]);
                    venda.DataVenda = (DateTime)(dr["datacao"]);

                }
                return venda;
            }
        }
        public IEnumerable<Venda> ObterVendaList(int id)
        {
            List<Venda> vendaList = new List<Venda>();
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select * from vw_detaVenda where ClienteiD = @id", conexao);
                cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);
                conexao.Close();
                foreach (DataRow dr in dt.Rows)
                {
                    vendaList.Add(
                        new Venda
                        {
                            DataVenda = Convert.ToDateTime(dr["datacao"]),
                            ValorTotal = Convert.ToDecimal(dr["Total"])
                        });
                }
                return vendaList;
            }
        }
        public Endereco ObterEndereco(string cep, string num)
        {
            using (HttpClient client = new HttpClient())
            {
                string strURL = string.Format("https://viacep.com.br/ws/{0}/json/", cep);

                Endereco endereco = new Endereco();

                var resposta = client.GetAsync(strURL).Result;
                if (resposta.IsSuccessStatusCode)
                {
                    var result = resposta.Content.ReadAsStringAsync().Result;

                    endereco = JsonConvert.DeserializeObject<Endereco>(result);
                    endereco.NumLougradouro = num;
                }
                return endereco;
            }
        }
    }
}


