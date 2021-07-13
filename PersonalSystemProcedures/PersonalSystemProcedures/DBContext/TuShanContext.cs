using Microsoft.EntityFrameworkCore;
using PersonalSystemProcedures.Models.MobileModels.DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalSystemProcedures.DBContext
{
    public class TuShanContext : DbContext
    {
        public TuShanContext(DbContextOptions<TuShanContext> options) : base(options)
        {

        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<LostInfo> LostInfors { get; set; }
    }
}
