using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using System.Linq;

namespace FiveM.Core.Managers
{
	class RobberyManager : BaseScript
	{

		public static int LastRobberyGain = 0;
		public static bool IsWantedForRobbery = false;

		public class RobberyLocation
		{
			public RobberyLocation(Vector3 location, string name, int maxMoney, int minMoney, bool isAnyWeapon = false, WeaponHash weapon = WeaponHash.CarbineRifle, int wantedLevel = 3, int notorietyIncrease = 1)
			{
				Location = location;
				Name = name;
				MaxMoney = maxMoney;
				MinMoney = minMoney;
				RequiredWeapon = weapon;
				WantedLevel = wantedLevel;
				IsAnyWeapon = isAnyWeapon;
				NotorietyIncrease = notorietyIncrease;
			}

			public Vector3 Location { get; set; }
			public string Name { get; set; }
			public int MaxMoney { get; set; }
			public int MinMoney { get; set; }
			public bool IsAnyWeapon { get; set; }
			public WeaponHash RequiredWeapon = WeaponHash.CarbineRifle;
			public DateTime LastRobbed = new DateTime();
			public int WantedLevel = 3;
			public int NotorietyIncrease = 1;

		}

		public List<RobberyLocation> Locations = new List<RobberyLocation>
		{
			new RobberyLocation(new Vector3(150, -1040, 29.37f), "Fleeca", 15000, 7500),
			new RobberyLocation(new Vector3(-707.5f, -914, 19.2f), "Limited Gasoline", 5000, 1000, true, wantedLevel: 2),
			new RobberyLocation(new Vector3(-1222f, -907, 12.33f), "Rob's Liquor", 2500, 500, true, wantedLevel: 2),
			new RobberyLocation(new Vector3(-2962.5f, 483, 15.7f), "Fleeca", 25000, 12500),
			new RobberyLocation(new Vector3(-3243, 1001.35f, 12.83f), "24/7 Convenience", 5000, 500, true, wantedLevel: 2),
			new RobberyLocation(new Vector3(-113, 6470, 31.63f), "Blaine County Savings", 30000, 1500, false, WeaponHash.SpecialCarbine, wantedLevel: 4),
			new RobberyLocation(new Vector3(-105, 6476.5f, 31.63f), "Blaine County Savings Vault", 400000, 250000, false, WeaponHash.SpecialCarbine, wantedLevel: 5, notorietyIncrease: 15),
		};

		public RobberyManager()
		{
			Tick += OnTick;
		}
		public async Task OnTick()
		{
			CheckLocations();
			UpdateWantedStatus();
		}

		public void CheckLocations()
		{
			var cloLoc = Locations.Where(loc => World.GetDistance(Game.PlayerPed.Position, loc.Location) < 1.5).FirstOrDefault();

			if (cloLoc == null)
				return;

			if((DateTime.Now - cloLoc.LastRobbed).TotalSeconds < 30)
				return;
			
			if((DateTime.Now - cloLoc.LastRobbed).TotalMinutes < 48)
			{
				AddTextEntry("Price", $"You have recently robbed this building!");
				DisplayHelpTextThisFrame("Price", false);
				return;
			}
			else if (!cloLoc.IsAnyWeapon && Game.PlayerPed.Weapons.Current.Hash != cloLoc.RequiredWeapon)
			{
				AddTextEntry("Price", $"You lack an appropriate weapon to rob this building");
				DisplayHelpTextThisFrame("Price", false);
				return;
			}

			AddTextEntry("Price", $"Press ~INPUT_CONTEXT~ to rob this building!");
			DisplayHelpTextThisFrame("Price", false);

			if (IsControlJustReleased(0, 46))
			{
				int Gain = new Random().Next(cloLoc.MinMoney, cloLoc.MaxMoney);

				MoneyHandler.AddMoney(Gain);
				cloLoc.LastRobbed = DateTime.Now;

				BeginTextCommandDisplayHelp("THREESTRINGS");
				AddTextComponentSubstringPlayerName($"You have robbed {cloLoc.Name} for a total of ${Gain}");
				EndTextCommandDisplayHelp(0, false, false, 8000);

				Game.Player.WantedLevel = cloLoc.WantedLevel;
				IsWantedForRobbery = true;

				LastRobberyGain = Gain;

				HunterManager.IncreaseNotoriety(cloLoc.NotorietyIncrease);
			}
		}

		public static void UpdateWantedStatus()
		{
			if (Game.Player.WantedLevel < 1 && IsWantedForRobbery)
				IsWantedForRobbery = false;
		}
	}
}
