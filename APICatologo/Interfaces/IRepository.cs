using System.Linq.Expressions;

namespace APICatologo.Interfaces
{
    public interface IRepository<T>
    {// cuidado para nao violar o princinpio ISP
        IEnumerable<T> GetAll();
        T? Get(Expression<Func<T, bool>>predicate);
        T Create(T entity);
        T Update(T entity);
        T Delete(T entity);
    }
}
