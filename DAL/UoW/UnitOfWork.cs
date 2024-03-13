using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lamar;

namespace DAL.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IContainer _container;

        //private readonly RecrutimentContext _context;

        public UnitOfWork(IContainer container /*RecrutimentContext context*/)
        {
            _container = container;
            //_context = context;
        }

        public TRepository GetRepository<TRepository>() where TRepository : class
        {
            return _container.GetInstance<TRepository>();
        }

        public int Save()
        {
            //    return _context.SaveChanges();
            //}
            return 1;
        }
            public void Dispose()
            {
                //_context.Dispose();
                GC.SuppressFinalize(this);
            }
        }

    }

