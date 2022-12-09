using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sibers.ProjectManagementSystem.Domain.EmployeeAgregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = Sibers.ProjectManagementSystem.Domain.TaskAgregate.Task;
using TaskStatus = Sibers.ProjectManagementSystem.Domain.TaskAgregate.TaskStatus;

namespace Sibers.ProjectManagementSystem.DataAccess.Configurations
{
    internal class TaskEntityTypeConfiguration : IEntityTypeConfiguration<Task>
    {
        public void Configure(EntityTypeBuilder<Task> builder)
        {
            builder.ToTable(nameof(Task));

            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedNever();

            builder.Ignore(t => t.DomainEvents);

            builder.Property(t => t.TaskStatus)
                .HasColumnName(nameof(TaskStatus))
                .HasDefaultValue(TaskStatus.ToDo)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(t => t.Name).IsRequired();

            builder.Property(t => t.Description).IsRequired(false);

            builder.OwnsOne(t => t.Priority);

            builder.Property(t => t.ProjectId).IsRequired();

            builder.Property(t => t.AuthorEmployeeId).IsRequired();
            builder.HasOne<Employee>()
                .WithMany(DataAccessConstants.CreatedTasks)
                .HasForeignKey(t => t.AuthorEmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Property(t => t.ContractorEmployeeId).IsRequired(false);
            builder.HasOne<Employee>()
                .WithMany(DataAccessConstants.ExecutableTasks)
                .HasForeignKey(t => t.ContractorEmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
