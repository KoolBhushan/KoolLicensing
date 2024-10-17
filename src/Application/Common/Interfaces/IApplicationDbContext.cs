using KoolLicensing.Domain.Entities;

namespace KoolLicensing.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TodoList> TodoLists { get; }

    DbSet<TodoItem> TodoItems { get; }

    DbSet<License> Licenses { get; }

    DbSet<Product> Products { get; }

    DbSet<Customer> Customers { get; }

    DbSet<Machine> Machines { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
