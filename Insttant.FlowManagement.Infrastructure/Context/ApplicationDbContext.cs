using Insttantt.FlowManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Insttantt.FlowManagement.Infrastructure.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Flow> flow { get; set; }
        public virtual DbSet<StepFlow> stepFlow { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Flow>().ToTable("Flow");
            modelBuilder.Entity<StepFlow>().ToTable("StepFlow");

        }
    }
}
