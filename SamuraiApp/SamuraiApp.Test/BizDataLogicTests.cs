using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SamuraiApp.Data;
using SamuraiApp.Domain;
using System.Diagnostics;

namespace SamuraiApp.Test
{
	[TestClass]
	public class BizDataLogicTests
	{
		[TestMethod]
		public void CanInsertSamuraiIntoDatabase()
		{
			var builder = new DbContextOptionsBuilder();
			builder.UseInMemoryDatabase("CanInsertSamurai");

			using var context = new SamuraiContext(builder.Options);
			var bizLogic = new BusinessDataLogic(context);

			var nameList = new string[] { "Kikuchio", "Kyuzo", "Rikchi" };

			var result = bizLogic.AddSamuraisByName(nameList);

			Assert.AreEqual(nameList.Length, result);
		}
	}
}
