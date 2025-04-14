using DragonC.API.Services;
using DragonC.Domain.API;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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
		public IActionResult Get([FromQuery] int projectId)
        {
            return Ok(this._codeConfigurationService.Get(projectId));
        }
        // create, POST, param: CodeConfigurationDTO
        [HttpPost("create")]
		[Authorize]
		public IActionResult Create([FromBody] CodeConfigurationDTO codeConfigurationDTO) {
			return Ok(this._codeConfigurationService.Create(codeConfigurationDTO));
		}


        // update, POST, param: CodeConfigurationDTO
        // do not update, just drop and insert again, it will be easier
        [HttpPut("update")]
		[Authorize]
		public IActionResult Update([FromBody] CodeConfigurationDTO codeConfigurationDTO) {
            return Ok(this._codeConfigurationService.Update(codeConfigurationDTO));
			
		}
    }
}
