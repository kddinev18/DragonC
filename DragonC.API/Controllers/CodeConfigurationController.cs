using DragonC.API.Services;
using DragonC.Domain.API;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DragonC.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CodeConfigurationController : ControllerBase
    {
		private ICodeConfigurationService _codeConfigurationService;

		public CodeConfigurationController(ICodeConfigurationService _codeConfigurationService)
		{
			this._codeConfigurationService = _codeConfigurationService;
		}
		// get, GET, param: int projectId
		[HttpGet]
		[Authorize]
		public CodeConfigurationDTO Get([FromQuery] int projectId)
        {
            return this._codeConfigurationService.Get(projectId);
        }
        // create, POST, param: CodeConfigurationDTO
        [HttpPost("create")]
		[Authorize]
		public void Create([FromBody] CodeConfigurationDTO codeConfigurationDTO) {
            this._codeConfigurationService.Create(codeConfigurationDTO);    
        }


        // update, POST, param: CodeConfigurationDTO
        // do not update, just drop and insert again, it will be easier
        [HttpPost("update")]
		[Authorize]
		public void Update([FromBody] CodeConfigurationDTO codeConfigurationDTO) {
            this._codeConfigurationService.Update(codeConfigurationDTO);
        }
    }
}
