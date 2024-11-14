using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SAKIB_PORTFOLIO.Models;

namespace SAKIB_PORTFOLIO.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext()
        {
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
        public virtual DbSet<RequestCounts> RequestCounts { get; set; }
        public virtual DbSet<Visitors> Visitors { get; set; }
        public virtual DbSet<MY_PROFILE> MY_PROFILE { get; set; }
        public virtual DbSet<MY_SKILLS> MY_SKILLS { get; set; }
        public virtual DbSet<EDUCATION> EDUCATION { get; set; }
        public virtual DbSet<EXPERIENCE> EXPERIENCE { get; set; }
        public virtual DbSet<PROJECTS> PROJECTS { get; set; }
        public virtual DbSet<CONTACTS> CONTACTS { get; set; }
        public virtual DbSet<PROFILE_COVER> PROFILE_COVER { get; set; }
        public virtual DbSet<DESCRIPTION> DESCRIPTION { get; set; } = default!;
        public virtual DbSet<DESCRIPTION_TYPE> DESCRIPTION_TYPE { get; set; } = default!;
    }
}