using MySql.Data.MySqlClient;
using ProjetoE_CommerceGameFesth.Models;
using ProjetoE_CommerceGameFesth.Repository.Contract;
using System.Data;

namespace ProjetoE_CommerceGameFesth.Repository
{
    public class LoginRepository : ILoginRepository
    {
        private readonly string _conexaoMySQL;

        public LoginRepository(IConfiguration conf)
        {
            _conexaoMySQL = conf.GetConnectionString("ConexaoMySQL");
        }

        public Login Login(string Email, string Senha)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select * from Tb_Login where Email = @email and Senha = @senha", conexao);
                cmd.Parameters.Add("@email", MySqlDbType.VarChar).Value = Email;
                cmd.Parameters.Add("@senha", MySqlDbType.VarChar).Value = Senha;

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;

                Login login = new Login();
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    login.id_Login = Convert.ToInt32(dr["id_Login"]);
                    login.Email = Convert.ToString(dr["Email"]);
                    login.Senha = Convert.ToString(dr["Senha"]);
                    login.tipo_login = Convert.ToString(dr["tipo_login"]);
                }
                return login;
            }
        }


    }
}
