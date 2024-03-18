using Microsoft.EntityFrameworkCore;

class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<Task> Todos => Set<Task>();
}