using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace WinesBackend.Models
{
    public class DataContext  : DbContext
    {
        public DataContext() : base("DefaultConnection")
        {
            
        }

        //Aqui evito el  borrado en cascade:
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }


        //aqui me conecto a la bd:
        public DbSet<Wine> Wines { get; set; }
    }
}