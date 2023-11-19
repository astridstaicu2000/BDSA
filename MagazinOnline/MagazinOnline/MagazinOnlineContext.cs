using Microsoft.EntityFrameworkCore;
using MagazinOnline.Models.Entities;

namespace MagazinOnline
{
    public class MagazinOnlineContext : DbContext
    {
        public MagazinOnlineContext(DbContextOptions<MagazinOnlineContext> options)
            : base(options) 
        {

        }

        public DbSet<Product> Products { get; set; }
    }
}
