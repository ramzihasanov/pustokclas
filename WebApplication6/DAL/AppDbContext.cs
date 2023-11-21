using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebApplication6.Models;

namespace WebApplication6.DAL
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Slider> Slider { get; set; }
     
        public DbSet<Service> Services { get; set; }
    }
}
