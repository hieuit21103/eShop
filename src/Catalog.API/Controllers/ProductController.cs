namespace Catalog.API.Controllers;

public class ProductController(IGenericService<Product> service, ILogger<ProductController> logger)
    : BaseController<Product>(service, logger)
{
}
