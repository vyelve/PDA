using Microsoft.EntityFrameworkCore;
using PDA.Entities;
using System;

namespace PDA.Repository
{
    public class PDADbContext : DbContext
    {
        public PDADbContext(DbContextOptions<PDADbContext> options) : base(options)
        { }

        public DbSet<Port> Ports { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Terminal> Terminals { get; set; }
        public DbSet<VesselType> VesselTypes { get; set; }
        public DbSet<Cargo> Cargos { get; set; }
        public DbSet<Pilotage> Pilotages { get; set; }

    }
}

