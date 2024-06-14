using MySql.Data.MySqlClient;
using ProjetoE_CommerceGameFesth.Models;
using ProjetoE_CommerceGameFesth.Repository.Contract;
using System.Data;

namespace ProjetoE_CommerceGameFesth.Repository
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly string _conexaoMySQL;
        public ProdutoRepository(IConfiguration conf)
        {
            _conexaoMySQL = conf.GetConnectionString("ConexaoMySQL");
        }
        public IEnumerable<Produto> ObterTodosProdutos(string por, string campo)
        {
            List<Produto> Produtolist = new List<Produto>();
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("call sp_ordenaProduto(@por);", conexao);
                cmd.Parameters.Add("@por", MySqlDbType.VarChar).Value = por;
                
                MySqlDataAdapter sd = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                sd.Fill(dt);
                    conexao.Close();

                    foreach (DataRow dr in dt.Rows)
                    {
                        Produtolist.Add(
                            new Produto
                            {
                                Codbarras = Convert.ToInt64(dr["CodBarras"]),
                                NomeProduto = (string)dr["Nome"],
                                Valor = Convert.ToString(dr["Valor"]),
                                QuantidadeEstoque = Convert.ToInt32(dr["QtdEst"]),
                                ImagemProduto = (string)dr["ImagemProduto"],
                                Marca = (string)dr["Marca"],
                            });
                    }
                    return Produtolist;
            }
        }
        public void Adicionar(Produto produto)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("CALL spInsereProduto(@CodBarras,@Nome,@Marca, @ImagemProduto, @Descricao, @Valor, @Qtd);", conexao);
                cmd.Parameters.Add("@CodBarras", MySqlDbType.Int64).Value = produto.Codbarras;
                cmd.Parameters.Add("@Nome", MySqlDbType.VarChar).Value = produto.NomeProduto;
                cmd.Parameters.Add("@Descricao", MySqlDbType.VarChar).Value = produto.Descricao;
                cmd.Parameters.Add("@Marca", MySqlDbType.VarChar).Value = produto.Marca;
                cmd.Parameters.Add("@ImagemProduto", MySqlDbType.VarChar).Value = produto.ImagemProduto;
                cmd.Parameters.Add("@Valor", MySqlDbType.VarChar).Value = produto.Valor.Replace(".", "").Replace(",", ".");
                cmd.Parameters.Add("@Qtd", MySqlDbType.Int32).Value = produto.QuantidadeEstoque;
                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }

        public void Atualizar(Produto produto)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("update Tb_Produto set Nome = @Nome,Descricao = @Descricao ,ImagemProduto = @ImagemProduto, Valor = @Valor, QtdEst = @QtdEst where codbarras = @CodBarras;", conexao);
                cmd.Parameters.Add("@Nome", MySqlDbType.VarChar).Value = produto.NomeProduto;
                cmd.Parameters.Add("@Descricao", MySqlDbType.VarChar).Value = produto.Descricao;
                cmd.Parameters.Add("@Marca", MySqlDbType.VarChar).Value = produto.Marca;
                cmd.Parameters.Add("@ImagemProduto", MySqlDbType.VarChar).Value = produto.ImagemProduto;
                cmd.Parameters.Add("@Valor", MySqlDbType.VarChar).Value = produto.Valor.Replace(".", "").Replace(",", ".");
                cmd.Parameters.Add("@QtdEst", MySqlDbType.Int32).Value = produto.QuantidadeEstoque;
                cmd.Parameters.Add("@CodBarras", MySqlDbType.Int64).Value = produto.Codbarras;

                cmd.ExecuteNonQuery();
            }
        }
        public Produto ObterProduto(long Id)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select * from Tb_Produto where CodBarras=@CodBarras", conexao);
                cmd.Parameters.Add("@CodBarras", MySqlDbType.Int64).Value = Id;

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;

                Produto produto = new Produto();
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    produto.Codbarras = Convert.ToInt64(dr["CodBarras"]);
                    produto.NomeProduto = (string)dr["Nome"];
                    produto.ImagemProduto = (string)dr["ImagemProduto"];
                    produto.Marca = (string)dr["Marca"];
                    produto.Descricao = (string)dr["Descricao"];
                    produto.Valor = Convert.ToString(dr["Valor"]);
                    produto.Valor1 = Convert.ToDecimal(produto.Valor);
                    produto.QuantidadeEstoque = Convert.ToInt32(dr["QtdEst"]);
                }
                return produto;
            }
        }
       
        public IEnumerable<Produto> PesquisaProdutos(string Nome)
        {
            List<Produto> Produtolist = new List<Produto>();
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select * from tb_produto where Nome Like @Nome;", conexao);
                cmd.Parameters.Add("@Nome", MySqlDbType.VarChar).Value = Nome;

                MySqlDataAdapter sd = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                sd.Fill(dt);
                conexao.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    Produtolist.Add(
                        new Produto
                        {
                            Codbarras = Convert.ToInt64(dr["CodBarras"]),
                            NomeProduto = (string)dr["Nome"],
                            Valor = Convert.ToString(dr["Valor"]),
                            QuantidadeEstoque = Convert.ToInt32(dr["QtdEst"]),
                            ImagemProduto = (string)dr["ImagemProduto"],
                            Marca = (string)dr["Marca"],
                        });
                }
                return Produtolist;
            }
        }
        public void Apagar(long Id)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("DELETE FROM Tb_Produto WHERE CodBarras = @CodBarras;", conexao);
                cmd.Parameters.Add("@CodBarras", MySqlDbType.Int64).Value = Id;

                cmd.ExecuteNonQuery();
            }
        }
    }
}

