﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamuraiApp.Data
{
	public class BusinessDataLogic
	{
		private SamuraiContext _context;

		public BusinessDataLogic(SamuraiContext context)
		{
			_context = context;
		}

		public BusinessDataLogic()
		{
			_context = new SamuraiContext();
		}

		public int AddSamuraisByName(params string[] names)
		{
			foreach (string name in names)
			{
				_context.Samurais.Add(new Domain.Samurai { Name = name });
			}

			var dbResult = _context.SaveChanges();

			return dbResult;
		}

	}
}
