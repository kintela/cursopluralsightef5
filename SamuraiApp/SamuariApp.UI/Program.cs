using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SamuariApp.UI
{
	class Program
	{
		private static SamuraiContext _context = new SamuraiContext();

		static void Main(string[] args)
		{
			//_context.Database.EnsureCreated();

			ExecuteSomeRawSql();

			Console.WriteLine("Press any kay...");
			Console.ReadKey();

		}

		private static void ExecuteSomeRawSql()
		{
			var samuraiId = 1;
			var affected=_context.Database
				.ExecuteSqlInterpolated($"EXEC dbo.DeleteQuotesForSamurai {samuraiId}");
		}

		private static void QueryUsingFromSqlRawStoredProc()
		{
			var text = "dinner";
			var samurais = _context.Samurais.FromSqlInterpolated(
				$"EXEC dbo.SamuraisWhoSaidAWord {text}").ToList();
		}

		private static void QueryUsingRawSqlWithInterploation()
		{
			string name = "Okamoto";
			var samurais = _context.Samurais
				.FromSqlInterpolated($"select * from samurais where Name='{name}'").Include(s => s.Quotes)
				.ToList();
		}


		private static void QueryRelatedUsingRawSql()
		{
			var samurais = _context.Samurais.FromSqlRaw("select Id,Name from samurais").Include(s=>s.Quotes).ToList();
		}

		private static void QueryUsingRawSql()
		{
			//var samurais = _context.Samurais.FromSqlRaw("select name from samurais").ToList();
			var samurais = _context.Samurais.FromSqlRaw("select Id,Name, Quotes, Battles, Horse from samurais").ToList();
		}

		private static void QuerySamuraiBattleStats()
		{
			//var stats = _context.SamuraiBattleStats.ToList();
			var firstStat = _context.SamuraiBattleStats.FirstOrDefault();
			var sampsonState = _context.SamuraiBattleStats.FirstOrDefault(b => b.Name == "SampsonSan");
			var findOne = _context.SamuraiBattleStats.Find(2);
		}
		private static void AddSamuraiWithPayLoadToABattle()
		{
			var battle = _context.Battles.FirstOrDefault();
			battle.Samurais.Add(new Samurai { Name = "Ramon perez" });
			_context.SaveChanges();

			var b_s = _context.Set<BattleSamurai>()
				.SingleOrDefault(bs => bs.BattleId == 1 & bs.SamuraiId == 12);

			if (b_s!=null)
			{
				b_s.DateJoined = DateTime.Now;
				_context.SaveChanges();
			}
		}

		private static void AddSamuraiWithPayLoadToABattle(int battleId, int SamuraiId)
		{

			var b_s = _context.Set<BattleSamurai>().Add(new BattleSamurai { BattleId = battleId, SamuraiId = SamuraiId, DateJoined = DateTime.Now });

			_context.SaveChanges();
				

		}


		private static void GetHorsesWithSamurai()
		{
			var horseOnly = _context.Set<Horse>().Find(3);

			var horseWithSamurai = _context.Samurais.Include(s => s.Horse).FirstOrDefault(s => s.Horse.Id == 3);

			var horseSamuraiPairs = _context.Samurais
				.Where(s => s.Horse != null)
				.Select(s => new { Horse = s.Horse, Samurai = s })
				.ToList();

		}

		private static void GetSamuraiWithHorse()
		{
			var samurais = _context.Samurais.Include(s => s.Horse).ToList();
		}

		private static void ReplaceHorse()
		{
			var horse = _context.Set<Horse>().FirstOrDefault(h => h.Name == "Silver");
			horse.SamuraiId = 1;//Pertenece a trigger

			_context.SaveChanges();
		}

		private static void AddNewHorseToDisconnectedSamuraiObject()
		{
			var samurai = _context.Samurais.AsNoTracking().FirstOrDefault(s => s.Id == 5);

			samurai.Horse = new Horse { Name = "Mr. Ed" };

			using var newContext = new SamuraiContext();
			newContext.Samurais.Attach(samurai);

			_context.SaveChanges();

		}

		private static void AddNewHorseToSamuraiObject()
		{
			var samurai = _context.Samurais.Find(2);

			samurai.Horse = new Horse { Name = "Black beauty" };

			_context.SaveChanges();

		}

		private static void AddNewHorseToSamuraiUsingId()
		{
			var horse = new Horse { Name = "Scout", SamuraiId=2 };

			_context.Add(horse);

			_context.SaveChanges();

		}


		private static void AddNewSamuraiWithHorse()
		{
			var samurai = new Samurai { Name = "Jina Ujichika" };
			samurai.Horse = new Horse { Name = "Silver" };

			_context.Samurais.Add(samurai);

			_context.SaveChanges();
		
		}
		private static void EditPayLoadSamuraiFromABattleExplicit()
		{

			var b_s = _context.Set<BattleSamurai>()
				.SingleOrDefault(bs => bs.BattleId == 1 & bs.SamuraiId == 6);

			if (b_s!=null)
			{
				b_s.DateJoined = DateTime.Now;
				_context.SaveChanges();
			}
		}



		private static void RemoveSamuraiFromAattle()
		{
			var battleWithSamurai = _context.Battles
					.Include(b => b.Samurais.Where(s => s.Id == 6))
					.Single(s => s.BattleId == 1);

			var samurai = battleWithSamurai.Samurais[0];

			battleWithSamurai.Samurais.Remove(samurai);//la relacion no se trackea

			_context.SaveChanges();


		}

		private static void AddAllSamuraisToAllBatles()
		{
			var allBattles = _context.Battles.Include(b=>b.Samurais).ToList();
			var allSamurais = _context.Samurais.Where(s=>s.Id!=7).ToList();

			foreach (var battle in allBattles)
			{
				battle.Samurais.AddRange(allSamurais);
			}

			_context.SaveChanges();

		}

		private static void ReturnAllBattlesWithSamurais()
		{
			var battles = _context.Battles.Include(b => b.Samurais).ToList();

		}
		private static void ReturnBattleWithSamurais()
		{
			var battle = _context.Battles.Include(b => b.Samurais).FirstOrDefault();	
		
		}
		private static void AddingNewSamuraiToAnExistingBattle()
		{
			var battle = _context.Battles.FirstOrDefault();
			battle.Samurais.Add(new Samurai { Name = "Takeda Shingen" });
			_context.SaveChanges();
		}
		private static void ModifyingRelatedDataWhenNotTracked()
		{
			var samurai = _context.Samurais.Include(s => s.Quotes)
																		.FirstOrDefault(s => s.Id == 1);

			var quote = samurai.Quotes[0];
			quote.Text+= "Did you hear that?";

			using var newContext = new SamuraiContext();
			//newContext.Quotes.Update(quote);
			newContext.Entry(quote).State = EntityState.Modified;

			newContext.SaveChanges();
		}


		private static void ModifyingRelatedDataWhenTracked()
		{
			var samurai = _context.Samurais.Include(s => s.Quotes)
																		.FirstOrDefault(s => s.Id == 1);

			samurai.Quotes[0].Text = "Did you hear that?";
			_context.SaveChanges();
		}

		private static void FilteringWithRelatedData()
		{
			var samurais = _context.Samurais
										.Where(s => s.Quotes.Any(q => q.Text.Contains("happy")))
										.ToList();
		}
		private static void LazyLoadQuotes()
		{
			var samurai = _context.Samurais.Find(1);
			var quoteCount = samurai.Quotes.Count();//Esto no funciona sin LL activado
		
		}
		private static void ExplicitLoadQuotes()
		{
			//asegurate de que existe algun horse enla bbdd, luego limpia el trackeador del contexto
			_context.Set<Horse>().Add(new Horse { SamuraiId = 1, Name = "Mr. Ed" });
			_context.SaveChanges();
			_context.ChangeTracker.Clear();

			var samurai = _context.Samurais.Find(1);
			_context.Entry(samurai).Collection(s => s.Quotes).Load();
			_context.Entry(samurai).Reference(s => s.Horse).Load();
		}

		private static void ProjectSamuraisWithQuotes()
		{

			var samuraisAdQuotes = _context.Samurais
				.Select(s => new {Samurai=s,
							HappyQuotes=s.Quotes.Where(q=>q.Text.Contains("happy"))})
				.ToList();

			var firstSamurai = samuraisAdQuotes[0].Samurai.Name += " The Happiest";

		}


		private static void ProjectSomeProperties()
		{

			var someProperties = _context.Samurais.Select(s => new { s.Id, s.Name }).ToList();
			var idAndNames=_context.Samurais.Select(s => new IdAndName(s.Id, s.Name)).ToList();


		}
		public struct IdAndName
		{
			public IdAndName(int id, string name)
			{
				Id = id;
				Name = name;
			}

			public int Id;
			public string Name;
		}

		private static void EagerLoadingSamuraiWithQuotes()
		{
			//var samuraiWihQuotes = _context.Samurais.Include(s => s.Quotes).ToList();

			//var filteredInclude = _context.Samurais
			//	.Include(s => s.Quotes.Where(q => q.Text.Contains("Thanks"))).ToList();


			var filterPrimaryEntityWithInclude = _context.Samurais.Where(s => s.Name.Contains("Sampson"))
				.Include(s => s.Quotes).FirstOrDefault();
		}

		private static void AddQuoteToExistingSamuraiNotTracked(int samuriaId)
		{
			var quote = new Quote {Text="Thanks for dinner!", SamuraiId=samuriaId };

			using var newContext = new SamuraiContext();

			newContext.Quotes.Add(quote);

			newContext.SaveChanges();
		}

		private static void AddQuoteToExistingSamuraiNotTracked3(int samuriaId)
		{
			var samurai = _context.Samurais.Find(samuriaId);

			samurai.Quotes.Add(new Quote
			{
				Text = "I bet you're happy I've saved you!"
			});

			using (var newContext = new SamuraiContext())
			{
				newContext.Samurais.Attach(samurai);
				newContext.SaveChanges();
			}
		}

		private static void AddQuoteToExistingSamuraiNotTracked2(int samuriaId)
		{
			var samurai = _context.Samurais.Find(samuriaId);

			samurai.Quotes.Add(new Quote
			{
				Text = "I bet you're happy I've saved you!"
			});

			using (var newContext=new SamuraiContext())
			{
				newContext.Samurais.Attach(samurai);
				newContext.SaveChanges();
			}			
		}

		private static void InsertNewSamuraiWithAQuote()
		{
			var samurai = new Samurai
			{
				Name = "Kambei Shimada",
				Quotes = new List<Quote> {
					new Quote{ Text="I've come to save you"}
				}
			};

			_context.Samurais.Add(samurai);
			_context.SaveChanges();
		}

		private static void InsertNewSamuraiWithManyQuote()
		{
			var samurai = new Samurai
			{
				Name = "Kyuzo",
				Quotes = new List<Quote> {
					new Quote{ Text="Watch out form my sharp sword!"},
					new Quote{ Text="I told you to watch out for the sharp sword! Oh well"}
				}
			};

			_context.Samurais.Add(samurai);
			_context.SaveChanges();
		}

		private static void QueryAndUpdateBattles_Disconnected()
		{

			List<Battle> disconnectedBattles;

			using (var context1=new SamuraiContext())
			{
				disconnectedBattles = _context.Battles.ToList();
			}//context1 is disposed

			disconnectedBattles.ForEach(b=>
				{
					b.StartDate = new DateTime(1570, 01, 01);
					b.EndDate = new DateTime(1570, 01, 01);
				});

			using (var context2=new SamuraiContext())
			{
				context2.UpdateRange(disconnectedBattles);
				context2.SaveChanges();
			}

		}

		private static void RetrieveAndDeleteASamurai()
		{
			var samurai = _context.Samurais.Find(36);

			_context.Samurais.Remove(samurai);

			_context.SaveChanges();

		}

		private static void MultipleDatabaseOperations()
		{
			var samurai = _context.Samurais.FirstOrDefault();
			samurai.Name += "San";

			_context.Samurais.Add(new Samurai { Name="Shino"});

			_context.SaveChanges();

		}
		private static void RetrieveAndUpdateMultipleSamurais()
		{
			var samurais = _context.Samurais.Skip(1).Take(4).ToList();
			samurais.ForEach(s => s.Name += "San");

			_context.SaveChanges();

		}

		private static void RetrieveAndUpdateSamurai()
		{
			var samurai = _context.Samurais.FirstOrDefault();
			samurai.Name += "San";

			_context.SaveChanges();

		}


		private static void QueryAggregates()
		{
			var samurai = _context.Samurais.Find(2);
			
		}

		private static void QueryFilters()
		{
			//var name = "Sampson";
			//var samurais = _context.Samurais.Where(s => s.Name ==name).ToList();

			var filter = "J%";
			var samurais = _context.Samurais
				.Where(s=>EF.Functions.Like(s.Name,filter)).ToList();
			
		}

		private static void AddVariousTypes()
		{

			_context.AddRange(
				new Samurai { Name = "Shimada" },
				new Samurai { Name = "Okamoto" },
				new Battle { Name = "Battle of Anegawa" },
				new Battle { Name = "Battle of Nagashino" });

			_context.SaveChanges();
		}

		private static void AddSamurai()
		{
			var samurai = new Samurai { Name = "Sampson" };
			_context.Samurais.Add(samurai);
			_context.SaveChanges();
		}

		private static void AddSamuraisByName(params string[] names)
		{
			foreach (string name in names)
			{
				_context.Samurais.Add(new Samurai { Name = name });
			}
			_context.SaveChanges();
		}


		private static void GetSamurais(string text)
		{
			var samurais = _context.Samurais
				.TagWith("ConsoleApp.Program.getSamurais method")
				.ToList();

			Console.WriteLine($"{text}: Samurai count is {samurais.Count}");
			foreach (var samurai in samurais)
			{
				Console.WriteLine(samurai.Name);
			}
		}
	}
}
