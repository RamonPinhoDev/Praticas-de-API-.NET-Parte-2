

using APICatologo.Data;
using APICatologo.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace APICatologo.Interfaces
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
       

        public CategoriaRepository(AppDbContext context) : base(context)
        {
            
        }

        





        //public Categoria Create(Categoria categoria)
        //{
        //    if (categoria == null) 
        //     throw new ArgumentNullException(nameof(categoria)); 
        //    _context.Categorias.Add(categoria);
        //    _context.SaveChanges();
        //    return categoria;

        //}



        //public Categoria Delete(int id)
        //{
        //    var categoria = _context.Categorias.FirstOrDefault(c=> id == c.CategoriaId);
        //    if (categoria == null)
        //        throw new ArgumentNullException(nameof(categoria));
        //    _context.Remove(categoria);
        //    _context.SaveChanges();
        //    return categoria;
        //}

        //public Categoria GetCategoria(int categoriaId)
        //{
        //    var categoria = _context.Categorias.Find(categoriaId);
        //    return categoria;

        //}

        //public IEnumerable<Categoria> GetCategorias()
        //{
        //    var list = _context.Categorias.ToList();
        //    return list;
        //}

        //public Categoria Update(Categoria categoria)
        //{
        //     _context.Entry(categoria).State = EntityState.Modified;
        //    _context.SaveChanges();
        //    return categoria;

        //}


    }
}
