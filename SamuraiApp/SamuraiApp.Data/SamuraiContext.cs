using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SamuraiApp.Domain;
using System;

namespace SamuraiApp.Data
{
	public class SamuraiContext:DbContext
	{
		public SamuraiContext() 
		{

		}
		public SamuraiContext(DbContextOptions<SamuraiContext> options):base(options)
		{

		}

		public SamuraiContext(DbContextOptions opt) : base(opt)
		{

		}

		public DbSet<Samurai> Samurais { get; set; }
		public DbSet<Quote> Quotes { get; set; }
		public DbSet<Battle> Battles { get; set; }

		public DbSet<SamuraiBattleStat> SamuraiBattleStats { get; set; }


		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				//optionsBuilder.UseSqlServer("Data Source= (localdb)\\MSSQLLocalDB; Initial catalog=SamuraiTestData");
				optionsBuilder.UseSqlServer("Server=localhost, 1433; Database=SamuraiTestData; User=sa; Password=Windows2021!");
			}
			
				

			/*optionsBuilder.UseSqlServer("Data Source= (localdb)\\MSSQLLocalDB; Initial catalog=SamuraiAppData")
				.LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name },
								LogLevel.Information)
				.EnableSensitiveDataLogging();*/

		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<SamuraiBattleStat>().HasNoKey().ToView("SamuraiBattleStats");
			/*modelBuilder.Entity<Samurai>()
				.HasMany(s => s.Battles)
				.WithMany(b => b.Samurais)
				.UsingEntity<BattleSamurai>
					(bs => bs.HasOne<Battle>().WithMany(),
					bs=>bs.HasOne<Samurai>().WithMany())
				.Property(bs=>bs.DateJoined)
				.HasDefaultValueSql("getdate()");*/

			modelBuilder.Entity<Samurai>()
				.HasMany(s => s.Battles)
				.WithMany(b => b.Samurais)
				.UsingEntity<BattleSamurai>
					(bs => bs.HasOne<Battle>().WithMany(),
					bs => bs.HasOne<Samurai>().WithMany());
				

			modelBuilder.Entity<Horse>().ToTable("Horses");
		}
	}
}
