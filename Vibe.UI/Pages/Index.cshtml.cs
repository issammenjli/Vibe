using Microsoft.AspNetCore.Mvc.RazorPages;
using Vibe.Entities;
using Vibe.UI.ApiServices;

namespace Vibe.UI.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ISingerApiService _singerApiService;

        public IndexModel(ILogger<IndexModel> logger, ISingerApiService singerApiService)
        {
            _logger = logger;
            _singerApiService = singerApiService;
        }


        public IEnumerable<Singer> Singers{ get; set; } = Enumerable.Empty<Singer>();

        //methode handler
        public async Task OnGet()
        {
            Singers = await _singerApiService.GetAll();
        }
    }
}