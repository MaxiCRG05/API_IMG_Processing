using System.Data.Entity;
using WebService.Models;

namespace WebService.Data
{
	public partial class Context : DbContext
	{
		public Context()
			: base("name=Context")
		{
		}
		public DbSet<Usuario> Usuarios { get; set; }
		public DbSet<Proyecto> Proyectos { get; set; }
		public DbSet<ProyectoObjeto> ProyectosObjetos { get; set; }
		public DbSet<Objeto> Objetos { get; set; }
		public DbSet<InvariantesHu> InvariantesHu { get; set; }
		public DbSet<RedNeuronal> RedesNeuronales { get; set; }
		public DbSet<Umbral> Umbrales { get; set; }
		public DbSet<Peso> Pesos { get; set; }
		public DbSet<Categoria> Categorias { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
		}
	}
}