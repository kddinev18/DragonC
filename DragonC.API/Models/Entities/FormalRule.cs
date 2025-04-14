namespace DragonC.API.Models.Entities
{
	public class FormalRule
	{
		public int Id { get; set; }
		public string Rule { get; set; }
		public bool IsStart { get; set; }

		public int ProjectId { get; set; }
		public Project Project { get; set; }
	}
}
