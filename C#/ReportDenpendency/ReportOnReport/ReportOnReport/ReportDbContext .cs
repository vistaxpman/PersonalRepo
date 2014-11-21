using System.Data.Entity;

namespace ReportOnReport
{
    public class ReportDbContext : DbContext
    {
        public DbSet<Report> Reports { get; set; }

        public DbSet<Source> Sources { get; set; }

        public DbSet<Set> Sets { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Report <-> Source
            modelBuilder.Entity<Report>().HasMany(r => r.Sources).WithMany(s => s.Reports).Map(m =>
                {
                    m.MapLeftKey("ReportId");
                    m.MapRightKey("SourceId");
                    m.ToTable("ReportSource");
                }
            );
            // Report <-> Set
            modelBuilder.Entity<Report>().HasMany(r => r.Sets).WithMany(s => s.Reports).Map(m =>
                {
                    m.MapLeftKey("ReportId");
                    m.MapRightKey("SetId");
                    m.ToTable("ReportSet");
                }
            );
        }
    }
}