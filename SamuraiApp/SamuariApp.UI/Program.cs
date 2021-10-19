using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;
using System;
using System.Linq;

namespace SamuariApp.UI
{
	class Program
	{
		private static SamuraiContext _context = new SamuraiContext();

		static void Main(string[] args)
		{
			//_context.Database.EnsureCreated();
			//AddSamuraisByName("shimada", "Okamoto","Kikuchio","Hayashida");
			//AddVariousTypes();
			//GetSamurais("After Add:");
			//QueryFilters();
			QueryAggregates();
			Console.WriteLine("Press any kay...");
			Console.ReadKey();

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
