﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiveM.Core
{
	static class Extensions
	{
		public static int Clamp(this int value, int min, int max)
		{
			return value > max ? max : value < min ? min : value;
		}
	}
}