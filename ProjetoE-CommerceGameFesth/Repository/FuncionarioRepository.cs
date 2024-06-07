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

        public void Atualizar(Funcionario funcionario)
        {
           

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("update Tb_Funcionario set Nome =@nome, DataAdmissao=@dataadmissao,  Tel=@tel, Email=@email, Senha=@senha, Tipo=@tipo where Id=@id", conexao);

                cmd.Parameters.Add("@id", MySqlDbType.VarChar).Value = funcionario.IdFuncionario;
                cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = funcionario.NomeFuncionario;
                cmd.Parameters.Add("@dataadmissao", MySqlDbType.Datetime).Value = funcionario.DataAdmissao.ToString("yyyy/MM/dd");
                cmd.Parameters.Add("@tel", MySqlDbType.VarChar).Value = funcionario.Telefone;
                cmd.Parameters.Add("@email", MySqlDbType.VarChar).Value = funcionario.Email;
                cmd.Parameters.Add("@senha", MySqlDbType.VarChar).Value = funcionario.Senha;
                cmd.Parameters.Add("@tipo", MySqlDbType.VarChar).Value = funcionario.Tipo;

                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }

        public void AtualizarSenha(Funcionario funcionario)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("update Tb_Funcionario set Senha=@senha where Id=@id", conexao);

                cmd.Parameters.Add("@id", MySqlDbType.VarChar).Value = funcionario.IdFuncionario;
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

        public void Cadastrar(Funcionario funcionario)
        {
            string Tipo = FuncionarioTipoConstant.Comum;

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("CALL InserirFuncionarios (@nome,@tel,@tipo,@cep,@logradouro,@num,@cid,@uf,@email,@senha)", conexao);

                cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = funcionario.NomeFuncionario;
                cmd.Parameters.Add("@tel", MySqlDbType.Int32).Value = funcionario.Telefone;
                cmd.Parameters.Add("@tipo", MySqlDbType.VarChar).Value = Tipo;
                cmd.Parameters.Add("@cep", MySqlDbType.VarChar).Value = funcionario.CEP;
                cmd.Parameters.Add("@logradouro", MySqlDbType.VarChar).Value = funcionario.Lougradouro;
                cmd.Parameters.Add("@num", MySqlDbType.VarChar).Value = funcionario.NumLougradouro;
                cmd.Parameters.Add("@cid", MySqlDbType.VarChar).Value = funcionario.NomeCidade;
                cmd.Parameters.Add("@uf", MySqlDbType.VarChar).Value = funcionario.NomeUF;
                cmd.Parameters.Add("@email", MySqlDbType.VarChar).Value = funcionario.Email;
                cmd.Parameters.Add("@senha", MySqlDbType.VarChar).Value = funcionario.Senha;
                
                
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
                    funcionario.Telefone = Convert.ToInt32(dr["Tel"]);
                    funcionario.Tipo = (string)(dr["Tipo"]);
                    funcionario.Numero = Convert.ToInt32(dr["num"]);
                    funcionario.Email = Convert.ToString(dr["email"]);
                    funcionario.Senha = Convert.ToString(dr["senha"]);
                }
                return funcionario;
            }
        }

        public Funcionario ObterFuncionario(int Id)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select * from vw_FuncEnd where Id_func = @id", conexao);
                cmd.Parameters.AddWithValue("id", Id);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;

                Funcionario funcionario = new Funcionario();
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    funcionario.IdFuncionario = (Int32)(dr["Id_func"]);
                    funcionario.id_Login = (Int32)(dr["id_log"]);
                    funcionario.NomeFuncionario = (string)(dr["Nome_Func"]);
                    funcionario.DataAdmissao = (DateTime)(dr["DataAdmissao"]);
                    funcionario.Telefone = Convert.ToInt32(dr["Tel"]);
                    funcionario.Tipo = (string)(dr["Tipo"]);
                    funcionario.Numero = Convert.ToInt32(dr["num"]);
                    funcionario.CEP =  (decimal)dr["Cep_Func"];
                    funcionario.Lougradouro = (string)dr["Logradouro"];
                    funcionario.NomeUF = (string)dr["NomeUf"];
                    funcionario.NomeCidade = (string)dr["NomeCid"];
                    funcionario.Email = (string)(dr["Email"]);
                    funcionario.Senha = (string)(dr["Senha"]);
                }
                return funcionario;
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
        public IEnumerable<Funcionario> ObterFuncionarioList()
        {
            List<Funcionario> funcList = new List<Funcionario>();
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select * from vw_FuncEnd order by Id_func ", conexao);
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
                            Email = (string)(dr["Email"]),
                            Senha = (string)(dr["Senha"]),
                            DataAdmissao = (DateTime)(dr["DataAdmissao"]),
                            Numero = Convert.ToInt32(dr["num"]),
                            Telefone = Convert.ToInt32(dr["Tel"]),
                            Tipo = (string)(dr["Tipo"])
                        });

                }
                return funcList;
            }
        }

        public List<Funcionario> ObterFuncionarioPorEmail(string email)
        {
            throw new NotImplementedException();
        }
    }
}
