﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.SharedKernel.Interfaces
{
    public interface ISpecification<T>
    {
        public bool IsSatisfiedBy(T entity);
    }
}
