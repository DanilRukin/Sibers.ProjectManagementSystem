using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sibers.ProjectManagementSystem.Application.Commands;
using Sibers.ProjectManagementSystem.Application.Dtos;
using Sibers.ProjectManagementSystem.Application.Queries.ProjectQueries;
using Sibers.ProjectManagementSystem.SharedKernel;

namespace Sibers.ProjectManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private IMediator _mediator;

        public ProjectsController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        public async Task<ActionResult<ProjectDto>> Create([FromBody]ProjectDto dto)
        {
            CreateProjectCommand createProjectCommand = new CreateProjectCommand(dto);
            Result<ProjectDto> result = await _mediator.Send(createProjectCommand);
            if (result.ResultStatus == ResultStatus.Error)
                return BadRequest(result.Errors);
            return Ok(result.Value);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetAll([FromQuery]bool includeAdditionalData = false)
        {
            GetAllProjectsQuery query = new GetAllProjectsQuery(includeAdditionalData);
            Result<IEnumerable<ProjectDto>> result = await _mediator.Send(query);
            if (result.ResultStatus == ResultStatus.NotFound)
                return NotFound(result.Value);
            else if (result.ResultStatus == ResultStatus.Error)
                return BadRequest(result.Errors);
            return Ok(result.Value);
        }
    }
}
