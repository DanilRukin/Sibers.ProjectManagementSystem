using Sibers.ProjectManagementSystem.Application.Dtos;
using Sibers.ProjectManagementSystem.Domain.Exceptions;
using Sibers.ProjectManagementSystem.SharedKernel;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Application.Services
{
    internal class DomainExceptionHandler
    {
        internal static Result<T> Handle<T>(DomainException domainException)
        {
            Exception ex = domainException;
            List<string> errors = new List<string>();
            while (ex != null)
            {
                errors.Add(ex.Message);
                ex = ex.InnerException;
            }
            return Result<T>.Error(errors.ToArray());
        }

        internal static IResult Handle(DomainException domainException)
        {
            Exception ex = domainException;
            List<string> errors = new List<string>();
            while (ex != null)
            {
                errors.Add(ex.Message);
                ex = ex.InnerException;
            }
            return Result.Error(errors.ToArray());
        }
    }
}
