using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class ProviderRepository : IProviderRepository
    {
        private readonly ApplicationContext _context;

        public ProviderRepository(ApplicationContext context) => _context = context;

        public async Task<IEnumerable<Provider>> GetAllByNameAsync(string name, CancellationToken token = default)
            => await _context.Provider.Where(x => x.Name.ToLower().Contains(name.ToLower())).ToListAsync(token);

        public void RemoveRange(IEnumerable<Provider> providers)
            => _context.Provider.RemoveRange(providers);
    }
}