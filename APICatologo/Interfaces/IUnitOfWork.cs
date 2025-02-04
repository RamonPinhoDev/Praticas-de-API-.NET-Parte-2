namespace APICatologo.Interfaces
{
    public interface IUnitOfWork
    {
        public IProdutosRepository ProdutosRepository { get; }
        public ICategoriaRepository CategoriaRepository { get; }

        void Commit();



    }
    
}
