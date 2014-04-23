﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FarmManager.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class FarmManagementDBEntities : DbContext
    {
        public FarmManagementDBEntities()
            : base("name=FarmManagementDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<AI> AIs { get; set; }
        public DbSet<AnimalNote> AnimalNotes { get; set; }
        public DbSet<Animal> Animals { get; set; }
        public DbSet<AnimalStatu> AnimalStatus { get; set; }
        public DbSet<Birth> Births { get; set; }
        public DbSet<Breed> Breeds { get; set; }
        public DbSet<DeathCaus> DeathCauses { get; set; }
        public DbSet<Death> Deaths { get; set; }
        public DbSet<DefaultMedicine> DefaultMedicines { get; set; }
        public DbSet<MartPrice> MartPrices { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SheepWeight> SheepWeights { get; set; }
        public DbSet<Species> Species { get; set; }
        public DbSet<sysdiagram> sysdiagrams { get; set; }
        public DbSet<Treatment> Treatments { get; set; }
        public DbSet<UserMedicine> UserMedicines { get; set; }
    }
}
