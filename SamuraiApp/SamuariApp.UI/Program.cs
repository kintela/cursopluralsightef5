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
		static void Main(string[] args)
		{

			AddSamuraisByName("Shimada", "Okamoto", "Kikuchio");

		}

		private static void AddSamuraisByName(params string[] names)
		{
			var _bizdata = new BusinessDataLogic();
			var newSamuraisCreatedCount = _bizdata.AddSamuraisByName(names);
		}

		
	}
}
