using MySql.Data.MySqlClient;
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
                MySqlCommand cmd = new MySqlCommand("select * from tb_vendas", conexao);
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
                            DataVenda = (DateTime)(dr["data_"]),
                            IdCliente = Convert.ToInt32(dr["id_cli"])
                        });
                }
                return Vendalist;
            }
        }
        public DescricaoVenda ObterVenda(int nf, Int64 cod)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select * from vw_detaVenda where NotaFiscal=@NotaFiscal and CodBarras = @CodBarras", conexao);
                cmd.Parameters.Add("@NotaFiscal", MySqlDbType.Int64).Value = nf;
                cmd.Parameters.Add("@CodBarras", MySqlDbType.Int64).Value = cod;

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;

                DescricaoVenda desc = new DescricaoVenda();
                desc.venda = new Venda();
                desc.produto = new Produto();
                desc.item = new Item();
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    desc.venda.NotaFiscal = (int)(dr["NotaFiscal"]);
                    desc.venda.ValorTotal = (decimal)(dr["Total"]);
                    desc.venda.DataVenda = (DateTime)(dr["datacao"]);
                    desc.venda.IdCliente = Convert.ToInt32(dr["ClienteiD"]);
                    desc.produto.NomeProduto = Convert.ToString(dr["Produto"]);
                    desc.produto.Marca = Convert.ToString(dr["Marca"]);
                    desc.produto.Descricao = Convert.ToString(dr["Descricao"]);
                    desc.produto.ImagemProduto = Convert.ToString(dr["ImagemProduto"]);
                    desc.produto.Valor = Convert.ToString(dr["Valor"]);
                    desc.produto.QuantidadeEstoque = Convert.ToUInt16(dr["QtdEst"]);
                    desc.produto.Quantidade = Convert.ToUInt16(dr["Qtd"]);

                }
                return desc;
            }
        }
        public Int64 ObtemNF()
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                Int64 NF = 0;
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT NotaFiscal from vw_detaVenda ORDER BY NotaFiscal DESC LIMIT 1", conexao);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;

                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                
                while (dr.Read())
                {
                    NF = Convert.ToInt64(dr["NotaFiscal"]);
                }
                NF++;
                return NF;
            }
        }
        public Venda ObterCodCli(int nf)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * from tb_vendas where nf = @nf", conexao);
                cmd.Parameters.Add("@nf", MySqlDbType.Int64).Value = nf;
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;
                Venda venda = new Venda();
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    venda.IdCliente = Convert.ToInt32(dr["Id_cli"]);
                    venda.ValorTotal = Convert.ToInt32(dr["ValorTotal"]);
                }
                
                return venda;
            }
        }
        public IEnumerable<Produto> ObterCodBarras(int nf)
        {
            List<Produto> produto = new List<Produto>();
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {

                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select * from vw_detaVenda where NotaFiscal = @nf", conexao);
                cmd.Parameters.Add("@nf", MySqlDbType.Int64).Value = nf;
                MySqlDataAdapter sd = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                    sd.Fill(dt);
                    conexao.Close();

                    foreach (DataRow dr in dt.Rows)
                    {
                    produto.Add(
                            new Produto
                            {
                                Codbarras = Convert.ToInt64(dr["CodBarras"]),
                                NomeProduto = (string)(dr["Produto"])

                                
                            });
                    }
                    return produto;
               
            }
        }
    }
}
