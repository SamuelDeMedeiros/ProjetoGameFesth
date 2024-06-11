using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MySql.Data.MySqlClient;
using ProjetoE_CommerceGameFesth.Models;
using ProjetoE_CommerceGameFesth.Repository.Contract;
using System.Data;
using System.Net.Mail;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace ProjetoE_CommerceGameFesth.Repository
{
    public class LoginRepository : ILoginRepository
    {
        private readonly string _conexaoMySQL;

        private readonly ConfiguracaoEmail _Configuracao;
        public LoginRepository(IConfiguration conf)
        {
            _conexaoMySQL = conf.GetConnectionString("ConexaoMySQL");
            ConfiguracaoEmail configuracao = new ConfiguracaoEmail("rbpeixotojr@hotmail.com", "GameFesth.co", "junior26", "smtp-mail.outlook.com", "587");
            _Configuracao = configuracao;
        }
        
        public async void EnviaEmail(string Email1, string Assunto, string Mensagem)
        {
            var menssagem = new MimeMessage();
            menssagem.From.Add(new MailboxAddress(_Configuracao.NomeRemetente, _Configuracao.EmailRemetente));
            menssagem.To.Add(MailboxAddress.Parse(Email1));
            menssagem.Subject = Assunto;
            var corpo = new BodyBuilder { TextBody = Mensagem };
            menssagem.Body = corpo.ToMessageBody();
            try
            {
                var SMTPcliente = new SmtpClient();
                await SMTPcliente.ConnectAsync(_Configuracao.EnderecoRemetente).ConfigureAwait(false);
                await SMTPcliente.AuthenticateAsync(_Configuracao.EmailRemetente,_Configuracao.SenhaRemetente).ConfigureAwait(false);
                await SMTPcliente.SendAsync(menssagem).ConfigureAwait(false);
                await SMTPcliente.DisconnectAsync(true).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
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
        public Login Login(string Email)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select * from Tb_Login where Email = @email ", conexao);
                cmd.Parameters.Add("@email", MySqlDbType.VarChar).Value = Email;

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
