﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sibers.ProjectManagementSystem.Domain;
using Sibers.ProjectManagementSystem.Domain.EmployeeAgregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.DataAccess.Configurations
{
    internal class EmployeeAgregateTypeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employees");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.OwnsOne(e => e.Email);
            builder.OwnsOne(e => e.PersonalData);

            builder.Ignore(e => e.DomainEvents);
            builder.Ignore(e => e.OnTheseProjectsIsManager);
            builder.Ignore(e => e.OnTheseProjectsIsEmployee);
            builder.Ignore(e => e.Projects);

            builder.HasMany<EmployeeOnProject>("_employeeOnProjects") // nameof(Employee._employeeOnProjects) is not working
                .WithOne(ep => ep.Employee)
                .HasForeignKey(ep => ep.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
