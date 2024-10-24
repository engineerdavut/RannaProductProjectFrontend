using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RannaProductProjectFrontend.Model;

namespace RannaProductProjectFrontend.Pages
{
    public class ProductModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public ProductModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Urun listesini tutacak property
        public List<Product> Products { get; set; }

        // Token'i frontend'den alacak bir property
        public string Token { get; set; }

        // Sayfa yuklendiginde urunleri listeleme
        public async Task<IActionResult> OnGetAsync(string token)
        {
            Token = token;

            if (string.IsNullOrEmpty(Token))
            {
                return RedirectToPage("/Index"); // Token yoksa ana sayfaya yonlendir
            }

            // API'den urunleri cekmek icin GET istegi yap
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7196/api/product/products");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                Products = await response.Content.ReadFromJsonAsync<List<Product>>();
            }
            else
            {
                return BadRequest("Urunler getirilemedi.");
            }

            return Page();
        }
    }
}
