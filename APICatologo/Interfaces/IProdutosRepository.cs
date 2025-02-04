using APICatologo.Models;

namespace APICatologo.Interfaces
{
    public interface IProdutosRepository : IRepository<Produto>
    {
        IEnumerable<Produto> GetProdutosPorCategoria(int id);
    }
}
