using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Vibe.WebApi.Data;
using Vibe.Entities;
using Vibe.UI.ApiServices;

namespace Vibe.UI.Pages
{
    public class CreateModel : PageModel
    {
        private readonly ISingerApiService _singerApiService;

        public CreateModel(ISingerApiService singerApiService)
        {
            this._singerApiService = singerApiService;
        }
        
        [BindProperty]
        public Singer NewSinger { get; set; } = new();
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            await _singerApiService.Add(NewSinger);

            return RedirectToPage("Index");
        }
    }
}
