using Microsoft.EntityFrameworkCore;
using TodoMvc.Models;

namespace TodoMvc.Data
{
    public class AppDbContext : DbContext   
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        //public DbSet<TodoMvc.Models.TodoItem> TodoItems { get; set; } = null!;

        public DbSet<TodoItem> TodoItems { get; set; }
    }
}
