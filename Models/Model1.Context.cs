﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SewaKendaraan.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SewaKendaraanEntities : DbContext
    {
        public SewaKendaraanEntities()
            : base("name=SewaKendaraanEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Kendaraan> Kendaraans { get; set; }
        public virtual DbSet<LogAplikasi> LogAplikasis { get; set; }
        public virtual DbSet<Pemesanan> Pemesanans { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}
