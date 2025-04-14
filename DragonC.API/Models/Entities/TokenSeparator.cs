namespace DragonC.API.Models.Entities
{
	public class TokenSeparator
	{
		public int Id { get; set; }
		public string Separator { get; set; }

		public int ProjectId { get; set; }
		public ICollection<Project> Projects { get; set; }
	}
}
