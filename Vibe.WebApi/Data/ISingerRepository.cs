using Vibe.Entities;

namespace Vibe.WebApi.Data
{
    public interface ISingerRepository
    {
        Task<Singer> Add(Singer singer);
        Task<IEnumerable<Singer>> GetAll();
        Task<Singer> GetOne(int id);
    }
}
