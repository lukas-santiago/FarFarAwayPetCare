using Microsoft.EntityFrameworkCore;

namespace Application.Configuration;
public class ApiContext : DbContext
{
    public ApiContext() { }
    public ApiContext(DbContextOptions<ApiContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

    }
}