﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sibers.ProjectManagementSystem.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.DataAccess.Configurations
{
    public class EmployeeOnProjectTypeConfiguration : IEntityTypeConfiguration<EmployeeOnProject>
    {
        public void Configure(EntityTypeBuilder<EmployeeOnProject> builder)
        {
            builder.HasKey(ep => new { ep.ProjectId, ep.EmployeeId });
            builder.HasOne(e => e.Project)
                .WithMany(DataAccessConstants.EmployeesOnProject)
                .HasForeignKey(ep => ep.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(ep => ep.Employee)
                .WithMany(DataAccessConstants.EmployeeOnProjects)
                .HasForeignKey(ep => ep.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Property(ep => ep.Role)
                .HasColumnName("Role")
                .HasDefaultValue(EmployeeRoleOnProject.Employee)
                .HasConversion<string>()
                .IsRequired();
            builder.Navigation(eop => eop.Employee).AutoInclude();
            builder.Navigation(eop => eop.Project).AutoInclude();
        }
    }
}
