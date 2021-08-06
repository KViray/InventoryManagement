using InventoryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Context
{
    public partial class InventoryDbContext : DbContext
    {
        public DbSet<Attendance> Attendance { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Item> Item { get; set; }
        public DbSet<Inventory> Inventory { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Leave> Leave { get; set; }
        public DbSet<ItemHistory> ItemHistory { get; set; }
        public DbSet<Salary> Salary { get; set; }
        public InventoryDbContext()
        {
        }

        public InventoryDbContext(DbContextOptions<InventoryDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
