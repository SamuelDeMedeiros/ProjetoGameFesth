using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Newtonsoft.Json;
using ProjetoE_CommerceGameFesth.Models;
using System.Data;

namespace ProjetoE_CommerceGameFesth.CarrinhoCompra
{
    public class CookieCarrinhoCompra
    {
        private string Key = "Carrinho.Compras";
        private Cookie.Cookie _cookie;

        public CookieCarrinhoCompra(Cookie.Cookie cookie)
        {
            _cookie = cookie;
        }

        public void Salvar(List<Produto> lista)
        {
            string Valor = JsonConvert.SerializeObject(lista);
            _cookie.Cadastrar(Key, Valor);
        }
        public List<Produto> Consultar()
        {
            if (_cookie.Existe(Key))
            {
                string valor = _cookie.Consultar(Key);
                return JsonConvert.DeserializeObject<List<Produto>>(valor);
            }
            else
            {
                return new List<Produto>();
            }
        }
        public void Cadastrar(Produto item)
        {
            List<Produto> Lista;
            if (_cookie.Existe(Key))
            {
                Lista = Consultar();
                var ItemLocalizado = Lista.SingleOrDefault(a => a.Codbarras == item.Codbarras);

                if (ItemLocalizado == null)
                {
                    Lista.Add(item);
                    ItemLocalizado = Lista.SingleOrDefault(a => a.Codbarras == item.Codbarras);
                    ItemLocalizado.Quantidade = ItemLocalizado.Quantidade + 1;
                }
                else if (ItemLocalizado.QuantidadeEstoque <= ItemLocalizado.Quantidade)
                {
                    ItemLocalizado.Quantidade = ItemLocalizado.Quantidade;
                }
                else
                {
                    ItemLocalizado.Quantidade = ItemLocalizado.Quantidade + 1;
                }
                ItemLocalizado.valorTotal = Convert.ToDecimal(ItemLocalizado.Valor) * ItemLocalizado.Quantidade;
            }
            else
            {
                Lista = new List<Produto>();
                Lista.Add(item);
            }

            Salvar(Lista);
        }
        public void Atualizar(Produto item)
        {
            var Lista = Consultar();
            var ItemLocalizado = Lista.SingleOrDefault(a => a.Codbarras == item.Codbarras);

            if (ItemLocalizado != null)
            {
                ItemLocalizado.Quantidade = item.Quantidade + 1;
                Salvar(Lista);
            }
        }

        public void Remover(Produto item)
        {
            var Lista = Consultar();
            var ItemLocalizado = Lista.SingleOrDefault(a => a.Codbarras == item.Codbarras);

            if (ItemLocalizado != null)
            {
                Lista.Remove(ItemLocalizado);
                Salvar(Lista);
            }
        }
        public void DiminuirProduto(Produto item)
        {
            var Lista = Consultar();
            var ItemLocalizado = Lista.SingleOrDefault(a => a.Codbarras == item.Codbarras);

            if (ItemLocalizado != null && ItemLocalizado.Quantidade > 1)
            {

                ItemLocalizado.Quantidade = ItemLocalizado.Quantidade - 1;
                ItemLocalizado.valorTotal = Convert.ToDecimal(ItemLocalizado.Valor) * ItemLocalizado.Quantidade;
                Salvar(Lista);
            }
        }
        public bool Existe(string Key)
        {
            if (_cookie.Existe(Key))
            {
                return false;
            }
            return true;
        }
        public void RemoverTodos()
        {
            _cookie.Remover(Key);
        }
    }
}
