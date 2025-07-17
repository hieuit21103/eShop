namespace Order.API.Services;

public class HttpProductService(ILogger<HttpProductService> logger, HttpClient httpClient) : IProductService
{
    private readonly ILogger<HttpProductService> _logger = logger;
    private readonly HttpClient _httpClient = httpClient;

    public async Task<ProductDto?> GetProductByIdAsync(Guid productId)
    {
        var response = await _httpClient.GetAsync($"api/products/{productId}");
        if (response.IsSuccessStatusCode)
        {
            var product = await response.Content.ReadAsStringAsync();
            var productDto = JsonSerializer.Deserialize<ProductDto>(product, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return productDto;
        }
        else
        {
            _logger.LogError("Error fetching product with Id: {ProductId}", productId);
            return null;
        }
    }
}