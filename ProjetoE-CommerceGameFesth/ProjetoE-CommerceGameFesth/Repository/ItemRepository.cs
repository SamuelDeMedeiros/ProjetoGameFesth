using MySql.Data.MySqlClient;
using ProjetoE_CommerceGameFesth.Models;
using ProjetoE_CommerceGameFesth.Repository.Contract;

namespace ProjetoE_CommerceGameFesth.Repository
{
    public class ItemRepository : IItemRepository
    {
        private readonly string _conexaoMySQL;
        public ItemRepository(IConfiguration conf)
        {
            _conexaoMySQL = conf.GetConnectionString("ConexaoMySQL");
        }
        public void Adicionar(Item item)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("insert into Tb_Itemvenda(Nf, Valor, CodBarras, Qtd) values(@Nf, @Valor, @CodBarras, @Qtd)", conexao);
                cmd.Parameters.Add("@Nf", MySqlDbType.Int32).Value = item.NotaFiscal;
                cmd.Parameters.Add("@Valor", MySqlDbType.VarChar).Value = item.Valor.Replace(".", "").Replace(",", ".");
                cmd.Parameters.Add("@CodBarras", MySqlDbType.VarChar).Value = item.Codbarras;
                cmd.Parameters.Add("@Qtd", MySqlDbType.Int32).Value = item.Quantidade;
                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }

        public void Apagar(int Id)
        {
            throw new NotImplementedException();
        }

        public void Atualizar(Item IItem)
        {
            throw new NotImplementedException();
        }

        public Item Obteritens(int Id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Item> ObterTodosItens()
        {
            throw new NotImplementedException();
        }
    }
}
