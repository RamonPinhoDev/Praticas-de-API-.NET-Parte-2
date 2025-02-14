using APICatologo.Models;
using APICatologo.Pagination;

namespace APICatologo.Interfaces
{
    public interface IProdutosRepository : IRepository<Produto>
    {
        IEnumerable<Produto> GetProdutosPorCategoria(int id);

       /* IEnumerable<Produto>*/ PagedList<Produto> GetProdutos(ProdutosParametrs paramters);

        PagedList<Produto> GetProdutosFiltroPreco(ProdutosFiltroPreco ProdutosFiltroPreço);
    }
}
