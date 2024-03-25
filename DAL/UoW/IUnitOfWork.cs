using System;
using System.Threading.Tasks;

namespace DAL.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        TRepository GetRepository<TRepository>() where TRepository : class;
        int Save();
       
    }
}
