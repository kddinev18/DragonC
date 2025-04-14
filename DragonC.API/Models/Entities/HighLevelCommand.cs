namespace DragonC.API.Models.Entities
{
	public class HighLevelCommand
	{
		public int Id { get; set; }
		public string Code { get; set; }

		public int ProjectId { get; set; }
		public Project Project { get; set; }
	}
}
