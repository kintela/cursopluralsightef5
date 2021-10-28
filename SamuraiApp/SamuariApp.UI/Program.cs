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
			//QueryAndUpdateBattles_Disconnected();
			//InsertNewSamuraiWithAQuote();
			//InsertNewSamuraiWithManyQuote();
			//AddQuoteToExistingSamuraiWhileTracked();
			//AddQuoteToExistingSamuraiNotTracked(1);
			EagerLoadingSamuraiWithQuotes();
			Console.WriteLine("Press any kay...");
			Console.ReadKey();

		}

		private static void EagerLoadingSamuraiWithQuotes()
		{
			//var samuraiWihQuotes = _context.Samurais.Include(s => s.Quotes).ToList();

			var filteredInclude = _context.Samurais
				.Include(s => s.Quotes.Where(q => q.Text.Contains("Thanks"))).ToList();
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
