﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smart_home_web.Models.Options
{
	public class OptionsViewModel
	{
		public int Id { get; set; }

		public int X { get; set; }
		public int Y { get; set; }
		public int Weight { get; set; }
		public int Height { get; set; }

		public int DashboardOptionsId { get; set; }
	}
}
