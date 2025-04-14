namespace DragonC.API.Models.Entities
{
	public class Project
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public string UserId { get; set; }
		public User User { get; set; }

		public int ProcessorFileId { get; set; }
		public File File { get; set; }
	}
}
