using System.ComponentModel.DataAnnotations;

namespace DragonC.API.Models.DTOs
{
	public class RegisterDTO
	{
		[Required]
		public string Email { get; set; }

		[Required]
		public string Password { get; set; }
	}
}
