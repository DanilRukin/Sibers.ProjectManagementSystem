using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Application.Services.Interfaces
{
    public interface IMapper<TSource, TDest> 
        where TSource : class 
        where TDest : class
    {
        TDest Map(TSource source);
    }
}
