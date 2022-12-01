using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
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

            builder.Property(p => p.Name).IsRequired();

            builder.OwnsOne(p => p.Priority);

            builder.Property(p => p.NameOfTheCustomerCompany).IsRequired();

            builder.Property(p => p.NameOfTheContractorCompany).IsRequired();

            builder.Property(p => p.StartDate).IsRequired();

            builder.Property(p => p.EndDate).IsRequired();
        }
    }
}
