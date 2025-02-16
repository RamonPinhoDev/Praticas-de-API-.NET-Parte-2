using APICatologo.Models;
using APICatologo.Pagination;

namespace APICatologo.Interfaces
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
       Task< PagedList<Categoria>> GetCategoriasAsync(CategoriaParametrs paramters); 

       Task< PagedList<Categoria> >GetCategoriasFiltroNomeAsync(CategoriaFiltroNome paramters);

    }
}
