using Domain.Repositories;

namespace Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext _context;

        public UnitOfWork(ApplicationContext context) => _context = context;

        public async Task SaveChangesAsync(CancellationToken token = default) => await _context.SaveChangesAsync();
    }
}