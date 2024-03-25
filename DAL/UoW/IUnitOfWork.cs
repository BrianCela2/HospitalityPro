using System;
using System.Threading.Tasks;

namespace DAL.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveAsync();
        IRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : class;
        void Save();
        IUserRolesRepository GetUserRolesRepository();
        T GetRepository<T>();
    }
}
