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
		public DbSet<Models.Usuario> Usuario { get; set; }
		public DbSet<Models.UsuariosProyectos> UsuariosProyectos { get; set; }
		public DbSet<Models.Proyecto> Proyectos { get; set; }
		public DbSet<Models.ProyectosObjetos> ProyectosObjetos { get; set; }
		public DbSet<Models.Objetos> Objetos { get; set; }
		public DbSet<Models.InvariantesHu> InvariantesHu { get; set; }
		public DbSet<Models.RedNeuronal> RedNeuronales { get; set; }
		public DbSet<Models.Capas> Capas { get; set; }
		public DbSet<Models.Neuronas> Neuronas { get; set; }
		public DbSet<Models.Umbrales> Umbrales { get; set; }
		public DbSet<Models.Pesos> Pesos { get; set; }
		public DbSet<Models.Rol> Roles { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
		}
	}
}
