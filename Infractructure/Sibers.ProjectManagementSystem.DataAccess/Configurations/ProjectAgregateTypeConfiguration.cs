using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sibers.ProjectManagementSystem.Domain;
using Sibers.ProjectManagementSystem.Domain.ProjectAgregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.DataAccess.Configurations
{
    internal class ProjectAgregateTypeConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.ToTable(nameof(Project));

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Ignore(p => p.DomainEvents);
            builder.Ignore(p => p.Employees);
            builder.Ignore(p => p.Manager);
            builder.Ignore(p => p.Tasks);

            builder.Property(p => p.Name).IsRequired();

            builder.OwnsOne(p => p.Priority);

            builder.Property(p => p.NameOfTheCustomerCompany).IsRequired();

            builder.Property(p => p.NameOfTheContractorCompany).IsRequired();

            builder.Property(p => p.StartDate).IsRequired();

            builder.Property(p => p.EndDate).IsRequired();

            builder.HasMany<EmployeeOnProject>("_employeesOnProject")  // nameof(Project._employeesOnProject) is not working
                .WithOne(ep => ep.Project)
                .HasForeignKey(ep => ep.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
