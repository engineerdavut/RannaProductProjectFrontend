using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RannaProductProjectFrontend.Service;
using RannaProductProjectFrontend.Model;
using System.Text.Json;

namespace RannaProductProjectFrontend.Pages
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public IndexModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Token bilgisini tutacak property
        public string Token { get; set; }

        // Sayfa ilk yuklendiginde token'i kontrol et
        public void OnGet()
        {
            // Token'i local storage'dan alip kontrol edebilirsin, burada tarayici ile is yapmadigimiz icin bos birakiyoruz
        }

        // Token generate islemi icin
        public async Task<IActionResult> OnPostGenerateTokenAsync()
        {
            // Token generate endpointine istek gonder
            var response = await _httpClient.PostAsync("https://localhost:7196/api/token/generate", null);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var tokenData = JsonSerializer.Deserialize<Token>(jsonResponse);

                // Token'i property'e ata
                Token = tokenData.TokenString;

                // Token'i localStorage'a veya session'a saklamak front-end tarafinda yapilacak
                return Page();
            }

            return BadRequest("Token olusturulamadi.");
        }
    }
}
