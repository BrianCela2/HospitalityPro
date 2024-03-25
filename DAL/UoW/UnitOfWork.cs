using DAL.Concrete;
using DAL.Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DAL.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HospitalityProContext _context;

        public UnitOfWork(HospitalityProContext context)
        {
            _context = context;
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public IRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : class
        {
            return new BaseRepository<TEntity, TKey>(_context);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public IUserRolesRepository GetUserRolesRepository()
        {
            return new UserRolesRepository(_context);
        }

        T IUnitOfWork.GetRepository<T>()
        {
            throw new NotImplementedException();
        }
    }
}
