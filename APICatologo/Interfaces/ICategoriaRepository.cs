using APICatologo.Models;
using APICatologo.Pagination;

namespace APICatologo.Interfaces
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        PagedList<Categoria> GetCategorias(CategoriaParametrs paramters);

        PagedList<Categoria> GetCategoriasFiltroNome(CategoriaFiltroNome paramters);

    }
}
