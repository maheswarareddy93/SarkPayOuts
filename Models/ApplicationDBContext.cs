using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SarkPayOuts.Models.DbModels
{
    public class ApplicationDBContext:DbContext
    {
        public ApplicationDBContext() : base()
        {

        }
        public ApplicationDBContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<AgentRegistration> AgentRegistration { get; set; }
        public DbSet<AgentPayOuts> AgentPayOuts { get; set; }
        public DbSet<AdminDetails> AdminDetails { get; set; }
        public DbSet<ProjectsData > ProjectsData { get; set; }
        public DbSet<ProjectUnitsData > ProjectUnitsData { get; set; }

        public DbSet<AutharityData> AutharityData { get; set; }

        public DbSet<CommonSetting> CommonSetting { get; set; }
        public DbSet<MailTemplate> MailTemplate { get; set; }
    }
}
