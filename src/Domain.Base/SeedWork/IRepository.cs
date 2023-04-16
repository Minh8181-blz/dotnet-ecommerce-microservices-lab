using System.Threading.Tasks;

namespace Domain.Base.SeedWork
{
    public interface IRepository<T,Tid> where T : IAggregateRoot
    {
        T Add(T entity);

        void Update(T entity);

        Task<T> GetAsync(Tid id);
    }
}
