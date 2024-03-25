using DAL.Concrete;
using DAL.Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Lamar;

namespace DAL.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HospitalityProContext _context;
        private readonly IContainer _container;

        public UnitOfWork(IContainer container, HospitalityProContext context)
        {
            _context = context;
            _container = container;
        }


        public TRepository GetRepository<TRepository>() where TRepository : class
        {
            return _container.GetInstance<TRepository>();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public int Save()
        {
            return _context.SaveChanges();
        }
    }
      
}
