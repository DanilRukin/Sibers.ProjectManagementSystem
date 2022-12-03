using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Sibers.ProjectManagementSystem.DataAccess;
using Sibers.ProjectManagementSystem.Domain.ProjectAgregate;
using Sibers.ProjectManagementSystem.SharedKernel;
using Sibers.ProjectManagementSystem.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.IntegrationTests.Data
{
    public abstract class BaseEfRepoTestFixture<TRepository, TAgregateRoot> : BaseEfRepoTestFixture<TAgregateRoot>
        where TAgregateRoot : class, IAgregateRoot
        where TRepository : class, IRepository<TAgregateRoot>
    {
        protected override abstract TRepository GetRepository();
    }

    public abstract class BaseEfRepoTestFixture<TAgregateRoot> 
        where TAgregateRoot : class, IAgregateRoot
    {
        protected ProjectManagementSystemContext _context;
        public BaseEfRepoTestFixture()
        {
            var options = CreateDbContextOptions();
            var mockEventDispatcher = new Mock<IDomainEventDispatcher>();

            _context = new ProjectManagementSystemContext(mockEventDispatcher.Object, options);
        }

        protected static DbContextOptions<ProjectManagementSystemContext> CreateDbContextOptions()
        {
            IServiceProvider serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<ProjectManagementSystemContext>();
            builder.UseInMemoryDatabase("TestInMemoryDatabase")
                .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }

        protected abstract IRepository<TAgregateRoot> GetRepository();
    }
}
