using APICatologo.Data;
using APICatologo.Models;
using APICatologo.Pagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Linq.Expressions;

namespace APICatologo.Interfaces
{
    public class ProdutosRepository :  Repository<Produto>, IProdutosRepository
    {

        public ProdutosRepository(AppDbContext context) : base(context)
        {
            
        }

        public /*IEnumerable<Produto>*/ async Task<PagedList<Produto>> GetProdutosAsync(ProdutosParametrs paramters)
        {
            //return GetAll().OrderBy(p=> p.Nome).Skip((paramters.PageNumber -1) * paramters.PageNumber).Take(paramters.PageSize).ToList();

            var produtos = await GetAllAsync();
            var produtosOrdenados = produtos.OrderBy(p => p.ProdutoId).AsQueryable();
            var produtosPaginados = PagedList<Produto>.ToPagedList(produtosOrdenados, paramters.PageNumber, paramters.PageSize);

            return produtosPaginados;
        }

        public async Task<PagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutosFiltroPreco ProdutosFiltroPreço)
        {
            var produtos = await GetAllAsync();
            if (ProdutosFiltroPreço.Preco.HasValue && !string.IsNullOrEmpty(ProdutosFiltroPreço.PrecoCriterio))
            {
                if (ProdutosFiltroPreço.PrecoCriterio.Equals("menor", StringComparison.OrdinalIgnoreCase))
                {
                    produtos = produtos.Where(p=> p.Preco < ProdutosFiltroPreço.Preco).OrderBy(p=> p.Preco);
                }

                if (ProdutosFiltroPreço.PrecoCriterio.Equals("maior", StringComparison.OrdinalIgnoreCase))
                {
                    produtos = produtos.Where(p=> p.Preco > ProdutosFiltroPreço.Preco).OrderBy(p=> p.Preco);
                }

                if (ProdutosFiltroPreço.PrecoCriterio.Equals("Igual", StringComparison.OrdinalIgnoreCase))
                {
                    produtos = produtos.Where(p=> p.Preco == ProdutosFiltroPreço.Preco).OrderBy(p=> p.Preco);
                }
            }
            var produtosfiltrados = PagedList<Produto>.ToPagedList(produtos.AsQueryable() , ProdutosFiltroPreço.PageNumber, ProdutosFiltroPreço.PageSize);
            return produtosfiltrados;
        }

        public async Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(int id)
        {
            var produtos = await GetAllAsync();
            var produtosFiltrados = produtos.Where(c => c.CategoriaId == id);
            return produtosFiltrados;
        }





        //public IQueryable<Produto> GetProdutos()
        //{

        //    return _context.Produtos;

        //}



        //public Produto GetProduto(int id)
        //{
        //    return _context.Produtos.FirstOrDefault(p=> p.ProdutoId==id);

        //}

        //public Produto Create(Produto produto)
        //{
        //    _context.Produtos.Add(produto);
        //    _context.SaveChanges();
        //    return produto;
        //}
        //public bool Update(/*int id*/Produto produto)
        //{
        //    if (_context.Produtos.Any(p=> p.ProdutoId == produto.ProdutoId))
        //    {_context.Produtos.Update(produto);
        //        _context.SaveChanges();
        //        return true;
        //    }
        //    //if (id != null) {
        //    //    var list = _context.Produtos.Find(id);
        //    //    _context.Produtos.Update(list);
        //    //    return true;
        //    //}
        //    return false;
        //}

        //public bool Delete(int id)
        //{
        //    if (id != null) {
        //        var list = _context.Produtos.Find(id);
        //        _context.Produtos.Remove(list);
        //        _context.SaveChanges();
        //        return true;
        //    }
        //    return false;
        //}

        //public IEnumerable<Produto> GetAll()
        //{
        //    throw new NotImplementedException();
        //}

        //public Produto? Get(Expression<Func<Produto, bool>> predicate)
        //{
        //    throw new NotImplementedException();
        //}

        //Produto IRepository<Produto>.Update(Produto entity)
        //{
        //    throw new NotImplementedException();
        //}

        //public Produto Delete(Produto entity)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
