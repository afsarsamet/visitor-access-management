using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public class dbContextClass : DbContext
    {
        public dbContextClass(DbContextOptions<dbContextClass> options) : base(options) { }
        public DbSet<visitorLog> visitorLog { get; set; }
        public DbSet<Personel> Personel { get; set; }
        public DbSet<Firma> Firmalar { get; set; }
        public DbSet<FirmaTalep> FirmaTalepleri { get; set; }

    }
}
