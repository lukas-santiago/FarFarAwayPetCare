using Application.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Configuration;
public class ApiContext : DbContext
{
    public ApiContext() { }
    public ApiContext(DbContextOptions<ApiContext> options) : base(options) { }

    public virtual DbSet<Device> Device { get; set; }
    public virtual DbSet<DeviceConfig> DeviceConfig { get; set; }
    public virtual DbSet<DeviceConfigType> DeviceConfigType { get; set; }
    public virtual DbSet<DeviceData> DeviceData  { get; set; }
    public virtual DbSet<User> User { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

    }
}