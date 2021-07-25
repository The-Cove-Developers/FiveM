using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using System.Linq;

namespace FiveM.Core.Managers
{
	public static class MoneyHandler
	{
		public static int Money { get; private set; }

		public static void AddMoney(int Value) => Money += Value;
		public static void RemoveMoney(int Value) => Money = Extensions.Clamp(Money - Value, 0, int.MaxValue);
	}
}
