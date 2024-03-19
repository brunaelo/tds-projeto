using System;
using System.Reflection;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;
using ProjetoTDS.Models;

namespace ProjetoTDS.Context
{
	public class DatabaseContext : DbContext
	{
		public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DbSet<Produto> Produtos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}

