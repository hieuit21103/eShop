namespace Catalog.API.Controllers;

public class CategoryController(IGenericService<Category> service, ILogger<CategoryController> logger)
    : BaseController<Category>(service, logger)
{
}
