using Vibe.Entities;

namespace Vibe.UI.ApiServices
{
    public class SingerApiService : ISingerApiService
    {
        public HttpClient _httpClient { get; set; }
        public SingerApiService(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }
        #region  methodes Add GetAll
        public async Task<Singer?> Add(Singer singer)
        {
            var jsonContent = JsonContent.Create(singer);
            var response = await _httpClient.PostAsync("singer",jsonContent);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Singer>();
            
        }

        public async Task<IEnumerable<Singer>> GetAll()
        {
            var response = await _httpClient.GetAsync("singer");
            response.EnsureSuccessStatusCode();
            var singers = await response.Content.ReadFromJsonAsync<IEnumerable<Singer>>();
            if (singers == null)
                return Enumerable.Empty<Singer>();
            return singers;
        }
                
        #endregion
    }
}
