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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoItem>()
                .Property(t => t.DueDate)
                .IsRequired(false);
        }

        /*
         You reach for Fluent API specifically when:

        A Data Annotation and your intended schema disagree (today's case)
        You need something Data Annotations can't express at all — relationships between entities (one-to-many, many-to-many), 
        composite keys, indexes, default SQL values, specific column types/precision.
        You want configuration kept out of the model class entirely, for cleaner separation (some teams prefer all EF config in Fluent API, 
        zero Data Annotations on models, precisely to avoid this exact kind of hidden cross-framework interaction).
         */
    }
}
