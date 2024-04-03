using Microsoft.EntityFrameworkCore;
using PassIn.Infrastructure.Entitties;

namespace PassIn.Infrastructure
{
    public class PassInDbContext : DbContext
    {
        public DbSet<Event> Events { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=\"C:\\Dev\\C#_Projects\\PassInDb.db\"");
        }
    }
}
