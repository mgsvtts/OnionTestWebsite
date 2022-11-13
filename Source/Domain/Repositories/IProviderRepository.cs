using Domain.Entities;

namespace Domain.Repositories
{
    public interface IProviderRepository
    {
        public Task<Provider> GetByIdAsync(int id, CancellationToken token = default);

        public Task<Provider?> GetByNameAsync(string name, CancellationToken token = default);

        public Task<IEnumerable<Provider>> GetAllByNameAsync(string name, CancellationToken token = default);   

        public Task<IEnumerable<Provider>> GetAllAsync(CancellationToken token = default);

        public void RemoveRange(IEnumerable<Provider> providers);
    }
}