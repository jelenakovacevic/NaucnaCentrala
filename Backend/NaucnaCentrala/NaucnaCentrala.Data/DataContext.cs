using System;
using Microsoft.EntityFrameworkCore;
using NaucnaCentrala.Domain.Entities;

namespace NaucnaCentrala.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Korisnik> Korisnici { get; set; }
        public DbSet<Casopis> Casopisi { get; set; }
    }
}
