using Vibe.Entities;

namespace Vibe.UI.ApiServices
{
    public interface ISingerApiService
    {
        Task<Singer?> Add(Singer singer);
        Task<IEnumerable<Singer>> GetAll();        
    }
}
