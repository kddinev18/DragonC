using DragonC.API.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DragonC.API.Data
{
	public class DragonCDbContext : IdentityDbContext<User>
	{
		public DragonCDbContext(DbContextOptions<DragonCDbContext> options)
		   : base(options) { }

		public DbSet<Project> Projects { get; set; }
		public DbSet<FormalRule> FormalRules { get; set; }
		public DbSet<HighLevelCommand> HighLevelCommands { get; set; }
		public DbSet<LowLevelCommand> LowLevelCommands { get; set; }
		public DbSet<TokenSeparator> TokenSeparators { get; set; }
		public DbSet<Models.Entities.File> Files { get; set; }

	}
}
