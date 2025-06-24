using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Reflection;

namespace WebService.Data
{
	public partial class Context : DbContext
	{
		public Context()
			: base("name=Context")
		{
		}
		public DbSet<Models.Usuarios> Usuario { get; set; }
		public DbSet<Models.Proyectos> Proyectos { get; set; }
		public DbSet<Models.Objetos> Objetos { get; set; }
		public DbSet<Models.InvariantesHu> InvariantesHu { get; set; }
		public DbSet<Models.RedesNeuronales> RedNeuronales { get; set; }
		public DbSet<Models.Umbrales> Umbrales { get; set; }
		public DbSet<Models.Pesos> Pesos { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
		}
	}
}
