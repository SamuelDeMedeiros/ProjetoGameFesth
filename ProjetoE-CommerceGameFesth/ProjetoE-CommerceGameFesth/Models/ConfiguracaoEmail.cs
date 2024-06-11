using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace ProjetoE_CommerceGameFesth.Models
{
    public class ConfiguracaoEmail
    {
        public string EmailRemetente { get; set; }
        public string NomeRemetente { get; set; }
        public string SenhaRemetente { get; set; }
        public string EnderecoRemetente { get; set; }
        public string PortaRemetente { get; set; }

        public ConfiguracaoEmail (string email,string nome,string senha,string endereco,string porta) 
        {
            EmailRemetente = email;
            NomeRemetente = nome;
            SenhaRemetente = senha;
            EnderecoRemetente = endereco;
            PortaRemetente = porta;
        }
        
    }
}
