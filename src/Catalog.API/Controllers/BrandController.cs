namespace Catalog.API.Controllers;

public class BrandController(IGenericService<Brand> service, ILogger<BrandController> logger)
    : BaseController<Brand>(service, logger)
{
}
