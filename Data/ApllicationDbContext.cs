using Code_plus.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Code_plus.Data
{
    public class ApllicationDbContext : DbContext
    {
        public ApllicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
    }
}
