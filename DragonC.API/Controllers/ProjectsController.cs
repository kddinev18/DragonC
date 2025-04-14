using DragonC.API.Services;
using DragonC.Domain.API;
using DragonC.Domain.API.Common;
using DragonC.Domain.API.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DragonC.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            this._projectService = projectService;
        }

		// create, POST, param: [fromform] ProjectDTO
		[Authorize]
		[HttpPost("create")]
        public IActionResult Create([FromBody] ProjectDTO dto)
        {
			string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			this._projectService.Create(dto, userId);
            return Ok();
        }

        // getAllPaged, POST, param: PagedCollection<ProjectFilters>
        [Authorize]
        [HttpPost("getAllPaged")]
        public IQueryable<ProjectDTO> getAllPaged([FromBody] PagedCollection<ProjectFilters> filters)
        {
            return this._projectService.GetPagedProjects(filters);
        }

		// delete, DELETE
		// delete all code cofigurations linked to this projects
		[Authorize]
		[HttpDelete]
        public IActionResult Delete([FromQuery] int id)
        {
            this._projectService.Delete(id);
            return Ok();
        }

    }
}
