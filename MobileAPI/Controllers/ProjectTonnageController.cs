using ArmsModels.BaseModels;
using Core.BaseModels.Operations;
using Core.IDataServices.Operations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MobileAPI.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class ProjectTonnageController : ControllerBase
    {
        private readonly IProjectTonnageService _projectTonnageService;
        public ProjectTonnageController (IProjectTonnageService projectTonnageService)
        {
            _projectTonnageService = projectTonnageService;
        }

        [HttpGet("[action]/")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IEnumerable<ProjectTonnageModel> SelectProjectedTonnage(string SelectedBranches, DateTime? Date)
        {
            return _projectTonnageService.SelectProjectedTonnage(SelectedBranches, Date);
        }
    }
}
