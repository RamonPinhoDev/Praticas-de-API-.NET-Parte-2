using APICatologo.Data;
using Microsoft.EntityFrameworkCore;

namespace APICatologo.Interfaces
{
    public class UnitOfWork : IUnitOfWork
    {
        private IProdutosRepository _produtoRepo;

        private ICategoriaRepository _categoriaRepo;
        public AppDbContext _context;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }
        public IProdutosRepository ProdutosRepository
        {
           get {
                return _produtoRepo = _produtoRepo ?? new ProdutosRepository(_context);
                //if (_produtoRepo == null){
                //    _produtoRepo = new ProdutosRepository(_context);
                //}
                //    return _produtoRepo;

}

        }


        public ICategoriaRepository CategoriaRepository
        {
            get { return _categoriaRepo = _categoriaRepo ?? new CategoriaRepository(_context); }
        }
        public async Task CommitAsync()
        { await _context.SaveChangesAsync(); }


        public void Dispose() { _context.Dispose(); }
    }
}
