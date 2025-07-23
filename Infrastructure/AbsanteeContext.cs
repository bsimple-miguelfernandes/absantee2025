using Domain.Models;
using Infrastructure.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class AbsanteeContext : DbContext
    {
        public virtual DbSet<TrainingSubjectDataModel> TrainingSubjects { get; set; }
        public virtual DbSet<TrainingModuleDataModel> TrainingModules { get; set; }

        public AbsanteeContext(DbContextOptions<AbsanteeContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TrainingModuleDataModel>()
                .OwnsMany(t => t.Periods);

            base.OnModelCreating(modelBuilder);
        }
    }
}