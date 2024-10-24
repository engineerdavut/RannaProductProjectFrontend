using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using RannaProductProjectFrontend.Model;
namespace RannaProductProjectFrontend.Service
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Product>>("https://localhost:7196/api/product/products");
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Product>($"api/product/{id}");
        }

        public async Task AddProductAsync(Product product)
        {
            await _httpClient.PostAsJsonAsync("api/product", product);
        }

        public async Task UpdateProductAsync(Product product)
        {
            await _httpClient.PutAsJsonAsync($"api/product/{product.Id}", product);
        }

        public async Task DeleteProductAsync(int id)
        {
            await _httpClient.DeleteAsync($"api/product/{id}");
        }
    }
}
