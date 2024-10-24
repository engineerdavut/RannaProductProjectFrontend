using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RannaProductProjectFrontend.Service;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Razor Pages ve diger servisleri ekle
builder.Services.AddRazorPages();

// HttpClient ve IProductService bagimliligini ekle
builder.Services.AddHttpClient<IProductService, ProductService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7196/");  // API base adresi
    // Eger token eklemek istersen:
    // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "your-token");
});

var app = builder.Build();

// Middleware ekle
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();
