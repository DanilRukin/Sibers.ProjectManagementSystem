using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sibers.ProjectManagementSystem.API.Services;
using Sibers.ProjectManagementSystem.Application.Commands;
using Sibers.ProjectManagementSystem.Application.Dtos;
using Sibers.ProjectManagementSystem.Application.Queries.EmployeeQueries;

namespace Sibers.ProjectManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private IMediator _mediator;

        public EmployeesController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EmployeeDto>> Create([FromBody]EmployeeDto employee)
        {
            CreateEmployeeCommand request = new(employee);
            var response = await _mediator.Send(request);
            if (response.IsSuccess)
                return Created("", response.Value);
            else
                return ResultErrorsHandler.Handle(response);
        }

        [HttpGet("{id:int}/{includeAdditionalData:bool}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EmployeeDto>> GetById(int id, bool includeAdditionalData = false)
        {
            GetEmployeeByIdQuery request = new(id, includeAdditionalData);
            var response = await _mediator.Send(request);
            if (response.IsSuccess)
                return Ok(response.GetValue());
            else
                return ResultErrorsHandler.Handle(response);
        }

        [HttpPost("[action]/{projectId}/{employeeId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TaskDto>> CreateTask([FromRoute]int projectId, [FromRoute]int employeeId, [FromBody] TaskDto task)
        {
            CreateTaskCommand request = new(projectId, employeeId, task);
            var response = await _mediator.Send(request);
            if (response.IsSuccess)
                return Created("", response.Value);
            else
                return ResultErrorsHandler.Handle(response);
        }

        [HttpPut("[action]/{taskId}/{projectId}/{employeeId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> StartTask(string taskId, int projectId, int employeeId)
        {
            StartTaskCommand request = new(Guid.Parse(taskId), employeeId, projectId);
            var response = await _mediator.Send(request);
            if (response.IsSuccess)
                return Ok();
            else
                return ResultErrorsHandler.Handle(response);
        }

        [HttpPut("[action]/{taskId}/{projectId}/{employeeId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CompleteTask(string taskId, int projectId, int employeeId)
        {
            CompleteTaskCommand request = new(Guid.Parse(taskId), projectId, employeeId);
            var response = await _mediator.Send(request);
            if (response.IsSuccess)
                return Ok();
            else
                return ResultErrorsHandler.Handle(response);
        }
    }
}
