using DragonC.Domain.API;

namespace DragonC.API.Services
{
	public interface ICodeConfigurationService
	{

		public CodeConfigurationDTO Get(int Id);

		public bool Create(CodeConfigurationDTO codeConfigurationDTO);

		public bool Update(CodeConfigurationDTO codeConfigurationDTO);
	}
}
