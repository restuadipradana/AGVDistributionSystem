using Microsoft.EntityFrameworkCore;
using AGVDistributionSystem.Models;

namespace AGVDistributionSystem.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<ProcessStatus> ProcessStatus {get; set;}
        public DbSet<ProcessStatusPreparation> ProcessStatusPreparation {get; set;}
        public DbSet<Roles> Roles {get; set;}
        public DbSet<RunningPO> RunningPO {get; set;}
        public DbSet<UserRole> UserRole {get; set;}
        public DbSet<V_PO2> V_PO2 {get; set;}
        public DbSet<VW_MES_Org> VW_MES_Org {get; set;}
        public DbSet<VW_UserAcc> VW_UserAcc {get; set;}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProcessStatus>().HasKey(x => new { x.Id, x.Kind });
            modelBuilder.Entity<ProcessStatusPreparation>().HasKey(x => new { x.Id, x.Kind });
            modelBuilder.Entity<Roles>().HasKey(x => new { x.Role });
            modelBuilder.Entity<RunningPO>().HasKey(x => new { x.Id });
            modelBuilder.Entity<UserRole>().HasKey(x => new {x.Account, x.Role });
            modelBuilder.Entity<V_PO2>().HasNoKey();
            modelBuilder.Entity<VW_MES_Org>().HasNoKey();
            modelBuilder.Entity<VW_UserAcc>().HasNoKey();
        }

    }
}