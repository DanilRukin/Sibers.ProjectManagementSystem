using Sibers.ProjectManagementSystem.Application.Dtos;
using Sibers.ProjectManagementSystem.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Application
{
    public interface ICrudService<T> where T : class
    {
        Task<Result<T>> CreateAsync(T dto);
        Task<IResult> UpdateAsync(T dto);
        Task<IResult> DeleteAsync(int id);
    }
}
