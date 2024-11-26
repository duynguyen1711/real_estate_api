using Microsoft.EntityFrameworkCore;
using real_estate_api.Data;
using real_estate_api.Interface.Repository;
using real_estate_api.Repositories;
using System;

namespace real_estate_api.UnitofWork
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IUserRepository _userRepository;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }
        public IUserRepository UserRepository => _userRepository ??= new UserRepository(_context);

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
