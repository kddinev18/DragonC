using DragonC.Domain.API;

namespace DragonC.API.Services
{
	public interface ICodeConfigurationService
	{

		public CodeConfigurationDTO Get(int Id);

		public void Create(CodeConfigurationDTO codeConfigurationDTO);

		public void Update(CodeConfigurationDTO codeConfigurationDTO);
	}
}
