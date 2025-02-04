using APICatologo.Data;
using APICatologo.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace APICatologo.Interfaces
{
    public class ProdutosRepository :  Repository<Produto>, IProdutosRepository
    {

        public ProdutosRepository(AppDbContext context) : base(context)
        {
            
        }

        public IEnumerable<Produto> GetProdutosPorCategoria(int id)
        {
            return GetAll().Where(c=> c.CategoriaId == id);
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
