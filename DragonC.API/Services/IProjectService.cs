using DragonC.Domain.API;
using DragonC.Domain.API.Common;
using DragonC.Domain.API.Filters;
using System.Runtime.InteropServices;

namespace DragonC.API.Services
{
	public interface IProjectService
	{
		public void Create(ProjectDTO project, string userId);

		public IQueryable<ProjectDTO> GetPagedProjects(PagedCollection<ProjectFilters> pagedFilters, string userId);

		public void Delete(int Id);
	}
}
