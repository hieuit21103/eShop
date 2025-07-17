namespace Catalog.API.Controllers;

public class ProductImageController(IGenericService<ProductImages> service, ILogger<ProductImageController> logger)
    : BaseController<ProductImages>(service, logger)
{
}
