using DragonC.API.Data;
using DragonC.API.Models.Entities;
using DragonC.Domain.API;
using DragonC.Domain.API.Common;
using DragonC.Domain.API.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace DragonC.API.Services
{
    public class ProjectService : IProjectService
    {
        private readonly DragonCDbContext _context;

        public ProjectService(DragonCDbContext context)
        {
            _context = context;
        }
        public int Create(ProjectDTO project, string userId)
        {
            Models.Entities.File fileEntity = new Models.Entities.File()
            {
                FileName = project.FileName,
                FileData = project.FileData,
            };

            Project entity = new Project()
            {
                Name = project.Name,
                File = fileEntity,
                UserId = userId
            };

            this._context.Projects.Add(entity);
            this._context.Files.Add(fileEntity);
            this._context.SaveChanges();

            return entity.Id;
        }

        public void Delete(int Id)
        {
            List<TokenSeparator> tokens = (from tok in this._context.TokenSeparators
                                           where tok.ProjectId == Id
                                           select tok).ToList();
            List<Models.Entities.File> files = (from pro in this._context.Projects
                                                join fil in this._context.Files on pro.ProcessorFileId equals fil.Id
                                                where pro.Id == Id
                                                select fil).ToList();
            List<HighLevelCommand> highLevelCommands = (from hig in this._context.HighLevelCommands
                                                        where hig.ProjectId == Id
                                                        select hig).ToList();

            List<FormalRule> formalRules = (from fru in this._context.FormalRules
                                            where fru.ProjectId == Id
                                            select fru).ToList();

            List<LowLevelCommand> lowLevelCommands = (from llc in this._context.LowLevelCommands
                                                      where llc.ProjectId == Id
                                                      select llc).ToList();

            this._context.TokenSeparators.RemoveRange(tokens);
            this._context.Files.RemoveRange(files);
            this._context.HighLevelCommands.RemoveRange(highLevelCommands);
            this._context.FormalRules.RemoveRange(formalRules);
            this._context.LowLevelCommands.RemoveRange(lowLevelCommands);

            Project project = (from pro in this._context.Projects
                               where pro.Id == Id
                               select pro).FirstOrDefault();

            this._context.Projects.Remove(project);

            this._context.SaveChanges();
        }

        public IQueryable<ProjectDTO> GetPagedProjects(PagedCollection<ProjectFilters> pagedFilters)
        {
            var query = (from pro in this._context.Projects
                         join use in this._context.Users on pro.UserId equals use.Id
                         select new
                         {
                             pro,
                             use
                         });

            // Apply filters
            if (!string.IsNullOrWhiteSpace(pagedFilters.Filters?.ProjectName))
            {
                query = query.Where(p => p.pro.Name.ToLower().Contains(pagedFilters.Filters.ProjectName.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(pagedFilters.Filters?.AuthorUserName))
            {

                query = query.Where(p => p.use.UserName.ToLower().Contains(pagedFilters.Filters.AuthorUserName.ToLower()));
            }

            // Project to DTO
            var result = query
                .OrderBy(p => p.pro.Id) // Important for pagination
                .Skip((pagedFilters.Page - 1) * pagedFilters.PageSize)
                .Take(pagedFilters.PageSize)
                .Select(p => new ProjectDTO
                {
                    Id = p.pro.Id,
                    Name = p.pro.Name
                });

            return result;
        }

    }
}
