using ILSPMS.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILSPMS.Data
{
    public class ILSPMSContext : DbContext
    {
        public ILSPMSContext() : base("ILSPMS")
        {
            Database.SetInitializer<ILSPMSContext>(null);
        }

        #region Entity Sets
        public IDbSet<ApproverFlowByRole> ApproverFlowByRoleSet { get; set; }
        public IDbSet<Error> ErrorSet { get; set; }
        public IDbSet<Milestone> MilestoneSet { get; set; }
        public IDbSet<Project> ProjectSet { get; set; }
        public IDbSet<ProjectActivityFile> ProjectActivityFileSet { get; set; }
        public IDbSet<ProjectActivity> ProjectActivitySet { get; set; }
        public IDbSet<ProjectMovement> ProjectmovementSet { get; set; }
        public IDbSet<ProjectMovementType> ProjectmovementTypeSet { get; set; }
        public IDbSet<Role> RoleSet { get; set; }
        public IDbSet<User> UserSet { get; set; }
        public IDbSet<Division> DivisionSet { get; set; }
        #endregion

        public virtual void Commit()
        {
            base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Configurations.Add(new ApproverFlowByRoleConfiguration());
            modelBuilder.Configurations.Add(new ErrorConfiguration());
            modelBuilder.Configurations.Add(new MilestoneConfiguration());
            modelBuilder.Configurations.Add(new ProjectConfiguration());
            modelBuilder.Configurations.Add(new ProjectActivityConfiguration());
            modelBuilder.Configurations.Add(new ProjectActivityFileConfiguration());
            modelBuilder.Configurations.Add(new ProjectMovementConfiguration());
            modelBuilder.Configurations.Add(new ProjectMovementTypeConfiguration());
            modelBuilder.Configurations.Add(new RoleConfiguration());
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new DivisionConfiguration());
        }
    }
}
