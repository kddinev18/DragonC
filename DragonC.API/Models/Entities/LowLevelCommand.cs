namespace DragonC.API.Models.Entities
{
	public class LowLevelCommand
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string MachineCode { get; set; }
		public bool IsConditional { get; set; }

		public int ProjectId { get; set; }
		public Project Project { get; set; }
	}
}
