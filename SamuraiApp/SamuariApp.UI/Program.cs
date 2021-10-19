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
			_context.Database.EnsureCreated();
			AddSamurais("Julie2", "Sampson2");
			GetSamurais("After Add:");
			Console.WriteLine("Press any kay...");
			Console.ReadKey();

		}

		private static void AddSamurai()
		{
			var samurai = new Samurai { Name = "Sampson" };
			_context.Samurais.Add(samurai);
			_context.SaveChanges();
		}

		private static void AddSamurais(params string[] names)
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
