namespace DragonC.API.Models.Entities
{
	public class File
	{
		public int Id { get; set; }
		public string FileName { get; set; }
		public byte[] FileData { get; set; }
	}
}
