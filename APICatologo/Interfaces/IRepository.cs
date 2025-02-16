using System.Linq.Expressions;

namespace APICatologo.Interfaces
{
    public interface IRepository<T>
    {// cuidado para nao violar o princinpio ISP
       Task< IEnumerable<T>> GetAllAsync();
       Task< T?> GetAsync(Expression<Func<T, bool>>predicate);
        T Create(T entity);
        T Update(T entity);
        T Delete(T entity);
    }
}
