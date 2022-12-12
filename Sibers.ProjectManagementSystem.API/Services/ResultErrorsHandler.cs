using Microsoft.AspNetCore.Mvc;
using Sibers.ProjectManagementSystem.SharedKernel;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using IResult = Sibers.ProjectManagementSystem.SharedKernel.IResult;

namespace Sibers.ProjectManagementSystem.API.Services
{
    internal static class ResultErrorsHandler
    {
        internal static ActionResult<T> Handle<T>(Result<T> result)
        {
            if (result.ResultStatus == ResultStatus.NotFound)
                return new NotFoundObjectResult(result.Errors);
            else if (result.ResultStatus == ResultStatus.Error)
                return new BadRequestObjectResult(result.Errors);
            else
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        internal static ActionResult Handle(IResult result)
        {
            if (result.ResultStatus == ResultStatus.NotFound)
                return new NotFoundObjectResult(result.Errors);
            else if (result.ResultStatus == ResultStatus.Error)
                return new BadRequestObjectResult(result.Errors);
            else
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}
