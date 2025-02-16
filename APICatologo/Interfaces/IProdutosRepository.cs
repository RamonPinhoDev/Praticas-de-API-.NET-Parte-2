using APICatologo.Models;
using APICatologo.Pagination;

namespace APICatologo.Interfaces
{
    public interface IProdutosRepository : IRepository<Produto>
    {
        Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(int id);

       /* IEnumerable<Produto>*/ Task<PagedList<Produto>> GetProdutosAsync(ProdutosParametrs paramters);

       Task< PagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutosFiltroPreco ProdutosFiltroPreço);
    }
}
