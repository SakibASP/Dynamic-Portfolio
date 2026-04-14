using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Portfolio.Domain;
using Portfolio.Application.Common;

namespace Portfolio.Infrastructure.Persistence;

public class PortfolioDbContext : IdentityDbContext<IdentityUser>
{
    public PortfolioDbContext()
    {
    }

    public PortfolioDbContext(DbContextOptions<PortfolioDbContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            connectionString = EncryptionHelper.Decrypt(connectionString);
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<DESCRIPTION>()
            .Navigation(m => m.DESCRIPTION_TYPE_).AutoInclude();
        modelBuilder.Entity<DESCRIPTION>()
            .Navigation(m => m.PROJECT_).AutoInclude();
        modelBuilder.Entity<DESCRIPTION>()
            .Navigation(m => m.EXPERIENCE_).AutoInclude();
    }

    public virtual DbSet<RequestCounts> RequestCounts { get; set; } = default!;
    public virtual DbSet<Visitors> Visitors { get; set; } = default!;
    public virtual DbSet<MY_PROFILE> MY_PROFILE { get; set; } = default!;
    public virtual DbSet<MY_SKILLS> MY_SKILLS { get; set; } = default!;
    public virtual DbSet<EDUCATION> EDUCATION { get; set; } = default!;
    public virtual DbSet<EXPERIENCE> EXPERIENCE { get; set; } = default!;
    public virtual DbSet<PROJECTS> PROJECTS { get; set; } = default!;
    public virtual DbSet<CONTACTS> CONTACTS { get; set; } = default!;
    public virtual DbSet<PROFILE_COVER> PROFILE_COVER { get; set; } = default!;
    public virtual DbSet<DESCRIPTION> DESCRIPTION { get; set; } = default!;
    public virtual DbSet<DESCRIPTION_TYPE> DESCRIPTION_TYPE { get; set; } = default!;
}
