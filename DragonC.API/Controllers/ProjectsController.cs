using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DragonC.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        // create, POST, param: [fromform] ProjectDTO

        // getAllPaged, POST, param: PagedCollection<ProjectFilters>

        // delete, DELETE
        // delete all code cofigurations linked to this projects
    }
}
