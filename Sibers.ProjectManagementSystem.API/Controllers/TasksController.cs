using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sibers.ProjectManagementSystem.API.Services;
using Sibers.ProjectManagementSystem.Application.Commands;
using Sibers.ProjectManagementSystem.Application.Dtos;

namespace Sibers.ProjectManagementSystem.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private IMediator _mediator;

        public TasksController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("{projectId}/{employeeId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TaskDto>> Create([FromRoute] int projectId, [FromRoute] int employeeId, [FromBody] TaskDto task)
        {
            CreateTaskCommand request = new(projectId, employeeId, task);
            var response = await _mediator.Send(request);
            if (response.IsSuccess)
                return Created("", response.Value);
            else
                return ResultErrorsHandler.Handle(response);
        }

        [HttpPut("{taskId}/{projectId}/{employeeId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Start(string taskId, int projectId, int employeeId)
        {
            StartTaskCommand request = new(Guid.Parse(taskId), employeeId, projectId);
            var response = await _mediator.Send(request);
            if (response.IsSuccess)
                return Ok();
            else
                return ResultErrorsHandler.Handle(response);
        }

        [HttpPut("{taskId}/{projectId}/{employeeId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Complete(string taskId, int projectId, int employeeId)
        {
            CompleteTaskCommand request = new(Guid.Parse(taskId), projectId, employeeId);
            var response = await _mediator.Send(request);
            if (response.IsSuccess)
                return Ok();
            else
                return ResultErrorsHandler.Handle(response);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TaskDto>> Update(TaskDto task)
        {
            UpdateTasksDataCommand request = new(task);
            var response = await _mediator.Send(request);
            if (response.IsSuccess)
                return Ok(response.GetValue());
            else
                return ResultErrorsHandler.Handle(response);
        }
    }
}
