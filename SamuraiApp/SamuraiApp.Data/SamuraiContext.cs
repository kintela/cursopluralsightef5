using Microsoft.EntityFrameworkCore;
using SamuraiApp.Domain;

namespace SamuraiApp.Data
{
	public class SamuraiContext:DbContext
	{
		public DbSet<Samurai> Samurais { get; set; }
		public DbSet<Quote> Quotes { get; set; }
		public DbSet<Battle> Battles { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer("Data Source= (localdb)\\MSSQLLocalDB; Initial catalog=SamuraiAppData");
			
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Samurai>()
				.HasMany(s=>s.Battles)
				.WithMany(b=>b.Samurais)
				.UsingEntity<BattleSamurai>
				()

			modelBuilder.Entity<Quote>(entity=>
			{
				entity.HasIndex(e => e.SamuraiId, "IX_Quotes_SamuraiId");

				entity.HasOne(d => d.Samurai)
						.WithMany(p => p.Quotes)
						.HasForeignKey(d => d.SamuraiId);
			});
		}
	}
}
