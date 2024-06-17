using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using ProjetoE_CommerceGameFesth.Models;
using ProjetoE_CommerceGameFesth.Models.Constants;
using ProjetoE_CommerceGameFesth.Repository.Contract;
using System.Data;

namespace ProjetoE_CommerceGameFesth.Repository
{
    public class FuncionarioRepository : IFuncionarioRepository
    {
        private readonly string _conexaoMySQL;

        public FuncionarioRepository(IConfiguration conf)
        {
            _conexaoMySQL = conf.GetConnectionString("ConexaoMySQL");
        }

        public void Atualizar(CadastraEndereco cadastra)
        {
           

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("CALL updFuncEnd(@id,@nome, @cep,@log, @num, @nc,  @nuf, @email, @senha, @tel,@tipo);", conexao);

                cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = cadastra.funcionario.NomeFuncionario;
                cmd.Parameters.Add("@id", MySqlDbType.VarChar).Value = cadastra.funcionario.IdFuncionario;
                cmd.Parameters.Add("@cep", MySqlDbType.VarChar).Value = cadastra.endereco.CEP.Replace("-", "");
                cmd.Parameters.Add("@log", MySqlDbType.VarChar).Value = cadastra.endereco.Lougradouro;
                cmd.Parameters.Add("@num", MySqlDbType.VarChar).Value = cadastra.endereco.NumLougradouro;
                cmd.Parameters.Add("@nc", MySqlDbType.VarChar).Value = cadastra.endereco.NomeCidade;
                cmd.Parameters.Add("@nuf", MySqlDbType.VarChar).Value = cadastra.endereco.NomeUF;
                cmd.Parameters.Add("@email", MySqlDbType.VarChar).Value = cadastra.funcionario.Email;
                cmd.Parameters.Add("@senha", MySqlDbType.VarChar).Value = cadastra.funcionario.Senha;
                cmd.Parameters.Add("@tel", MySqlDbType.VarChar).Value = cadastra.funcionario.Telefone;
                cmd.Parameters.Add("@tipo", MySqlDbType.VarChar).Value = cadastra.funcionario.Tipo;

                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }

        public void AtualizarSenha(Funcionario funcionario)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("update Tb_Login set Senha=@senha where id_Login=@id", conexao);

                cmd.Parameters.Add("@id", MySqlDbType.VarChar).Value = funcionario.id_Login;
                cmd.Parameters.Add("@senha", MySqlDbType.VarChar).Value = funcionario.Senha;


                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }
        public void AtualizarPerfil(Funcionario funcionario)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("update Tb_Funcionario set Nome =@nome, Tel=@tel," +
                    " Email=@email, Senha=@senha where Id=@id", conexao);

                cmd.Parameters.Add("@id", MySqlDbType.VarChar).Value = funcionario.IdFuncionario;
                cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = funcionario.NomeFuncionario;
                cmd.Parameters.Add("@tel", MySqlDbType.VarChar).Value = funcionario.Telefone;
                cmd.Parameters.Add("@email", MySqlDbType.VarChar).Value = funcionario.Email;
                cmd.Parameters.Add("@senha", MySqlDbType.VarChar).Value = funcionario.Senha;

                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }
        public void Ativar(int id)
        {
            string Situacao = SituacaoConstant.Ativo;
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("update TB_FUNCIONARIO set Situacao=@situacao where Id_func=@id", conexao);
                cmd.Parameters.Add("@id", MySqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("@situacao", MySqlDbType.VarChar).Value = Situacao;
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
                MySqlCommand cmd = new MySqlCommand("update TB_FUNCIONARIO set Situacao=@situacao where Id_func=@id", conexao);
                cmd.Parameters.Add("@id", MySqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("@situacao", MySqlDbType.VarChar).Value = Situacao;
                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }
        public void Cadastrar(CadastraEndereco cadastra)
        {
            string Tipo = FuncionarioTipoConstant.Comum;

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("CALL InserirFuncionarios (@nome,@tel,@tipo,@cep,@logradouro,@num,@cid,@uf,@email,@senha)", conexao);

                cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = cadastra.funcionario.NomeFuncionario;
                cmd.Parameters.Add("@tel", MySqlDbType.VarChar).Value = cadastra.funcionario.Telefone.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "");
                cmd.Parameters.Add("@tipo", MySqlDbType.VarChar).Value = Tipo;
                cmd.Parameters.Add("@cep", MySqlDbType.VarChar).Value = cadastra.endereco.CEP.Replace("-","");
                cmd.Parameters.Add("@logradouro", MySqlDbType.VarChar).Value = cadastra.endereco.Lougradouro;
                cmd.Parameters.Add("@num", MySqlDbType.VarChar).Value = cadastra.endereco.NumLougradouro;
                cmd.Parameters.Add("@cid", MySqlDbType.VarChar).Value = cadastra.endereco.NomeCidade;
                cmd.Parameters.Add("@uf", MySqlDbType.VarChar).Value = cadastra.endereco.NomeUF;
                cmd.Parameters.Add("@email", MySqlDbType.VarChar).Value = cadastra.funcionario.Email;
                cmd.Parameters.Add("@senha", MySqlDbType.VarChar).Value = cadastra.funcionario.Senha;
                
                
                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }

        public void Excluir(int Id)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("delete from Tb_Funcionario where Id=@id", conexao);
                cmd.Parameters.Add("@id", MySqlDbType.VarChar).Value = Id;
                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }

        public Funcionario Login(int id)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select * from vw_FuncEnd where id_log = @id", conexao);
                cmd.Parameters.AddWithValue("id", id);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;

                Funcionario funcionario = new Funcionario();
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    funcionario.IdFuncionario = (Int32)(dr["Id_func"]);
                    funcionario.id_Login = Convert.ToInt32(dr["id_log"]);
                    funcionario.NomeFuncionario = (string)(dr["Nome_Func"]);
                    funcionario.DataAdmissao = (DateTime)(dr["DataAdmissao"]);
                    funcionario.Telefone = Convert.ToString(dr["Tel"]);
                    funcionario.Tipo = (string)(dr["Tipo"]);
                    funcionario.Email = Convert.ToString(dr["email"]);
                    funcionario.Senha = Convert.ToString(dr["senha"]);
                }
                return funcionario;
            }
        }

        public CadastraEndereco ObterFuncionario(int Id)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select * from vw_FuncEnd where Id_func = @id", conexao);
                cmd.Parameters.AddWithValue("id", Id);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;
                CadastraEndereco cadastraEndereco = new CadastraEndereco();
                cadastraEndereco.funcionario = new Funcionario();
                cadastraEndereco.endereco = new Endereco();
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    cadastraEndereco.funcionario.IdFuncionario = (Int32)(dr["Id_func"]);
                    cadastraEndereco.funcionario.id_Login = (Int32)(dr["id_log"]);
                    cadastraEndereco.funcionario.NomeFuncionario = (string)(dr["Nome_Func"]);
                    cadastraEndereco.funcionario.DataAdmissao = (DateTime)(dr["DataAdmissao"]);
                    cadastraEndereco.funcionario.Telefone = Convert.ToString(dr["Tel"]);
                    cadastraEndereco.funcionario.Tipo = (string)(dr["Tipo"]);
                    cadastraEndereco.endereco.NumLougradouro = Convert.ToInt32(dr["num"]);
                    cadastraEndereco.endereco.CEP = Convert.ToString(dr["Cep_Func"]);
                    cadastraEndereco.endereco.Lougradouro = (string)dr["Logradouro"];
                    cadastraEndereco.endereco.NomeUF = (string)dr["NomeUf"];
                    cadastraEndereco.endereco.NomeCidade = (string)dr["NomeCid"];
                    cadastraEndereco.funcionario.Email = (string)(dr["Email"]);
                    cadastraEndereco.funcionario.Senha = (string)(dr["Senha"]);
                }
                return cadastraEndereco;
            }
        }
        public Funcionario ObterEmailFuncionario(string email)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select Email from vw_FuncEnd where Email=@email", conexao);
                cmd.Parameters.AddWithValue("@email", email);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;
                Funcionario funcionario = new Funcionario();
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    funcionario.Email = (string)(dr["Email"]);

                }
                return funcionario;
            }
        }
        public IEnumerable<Funcionario> ObterFuncionarioList(string por, string campo)
        {
            List<Funcionario> funcList = new List<Funcionario>();
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("call sp_ordenaFuncionario(@por,@campo); ", conexao);
                cmd.Parameters.Add("@por", MySqlDbType.VarChar).Value = por;
                cmd.Parameters.Add("@campo", MySqlDbType.VarChar).Value = campo;
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);
                conexao.Close();
                foreach (DataRow dr in dt.Rows)
                {
                    funcList.Add(
                        new Funcionario
                        {
                            IdFuncionario = Convert.ToInt32(dr["Id_func"]),
                            NomeFuncionario = (string)(dr["Nome_Func"]),
                            DataAdmissao = (DateTime)(dr["DataAdmissao"]),
                            Telefone = Convert.ToString(dr["Tel"]),
                            Tipo = (string)(dr["Tipo"]),
                            Situacao = (string)(dr["Situacao"])

                        });

                }
                return funcList;
            }
        }
        public Funcionario ObterSituacaoFuncionario(int id)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select Situacao from vw_FuncEnd where Id_log=@id", conexao);
                cmd.Parameters.AddWithValue("@id", id);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;
                Funcionario funcionario = new Funcionario();
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    funcionario.Situacao = (string)(dr["Situacao"]);

                }
                return funcionario;
            }
        }
        public List<Funcionario> ObterFuncionarioPorEmail(string email)
        {
            throw new NotImplementedException();
        }
    }
}
