using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace WpfApp3
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<Employee> Employee { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .Property(e => e.role)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.status)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.login)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.password)
                .IsUnicode(false);
        }
    }
}
