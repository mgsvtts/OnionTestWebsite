using Domain.Entities;

namespace Domain.Repositories
{
    public interface IProviderRepository
    {
        public Task<IEnumerable<Provider>> GetAllByNameAsync(string name, CancellationToken token = default);

        public void RemoveRange(IEnumerable<Provider> providers);
    }
}