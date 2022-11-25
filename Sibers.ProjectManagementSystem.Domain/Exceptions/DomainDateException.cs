using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Domain.Exceptions
{
    public class DomainDateException : DomainException
    {
        public DomainDateException(string message) : base(message)
        {
        }
    }
}
