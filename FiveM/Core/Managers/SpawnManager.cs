using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace FiveM.Core.Managers
{
	class SpawnManager : BaseScript
	{
		Random random = new Random();
		int HospitalBill;
		bool isDead;

		public SpawnManager()
		{
			Tick += CheckForDeath;
		}
		async Task CheckForDeath()
		{
			if(Game.PlayerPed.IsDead && !isDead)
			{
				isDead = true;
				HandleDeath();
			}
		}

		public async void HandleDeath()
		{
			int MoneyTaken = 0;
			if (RobberyManager.IsWantedForRobbery)
			{
				MoneyHandler.RemoveMoney(RobberyManager.LastRobberyGain);
				MoneyTaken = RobberyManager.LastRobberyGain;
				RobberyManager.LastRobberyGain = 0;
				RobberyManager.IsWantedForRobbery = false;
			}

			SetAmbientPedsDropMoney(true);

			SetNotificationTextEntry("CELL_EMAIL_BCON");
			foreach (string s in CitizenFX.Core.UI.Screen.StringToArray("You died"))
			{
				AddTextComponentSubstringPlayerName(s);
			}
			DrawNotification(false, false);

			await Delay(5000);

			DoScreenFadeOut(2500);
			await Delay(2500);

			int ped = Game.PlayerPed.Handle;

			HunterManager.Notoriety = 0;

			NetworkResurrectLocalPlayer(random.Next(-460, -450), random.Next(-352, -327), 33, 0, true, false);

			DisplayHud(false);
			SpawnPed(random.Next(-460, -450), random.Next(-352, -327), 33);

			uint model = (uint)Game.PlayerPed.Model.Hash;
			RequestModel(model);

			ClearPedTasks(ped);
			ClearPlayerWantedLevel(ped);

			HospitalBill = random.Next(MoneyHandler.Money / 8, MoneyHandler.Money / 3);
			HospitalBill = HospitalBill.Clamp(500, MoneyHandler.Money);

			MoneyHandler.RemoveMoney(HospitalBill);

			DisplayHud(true);

			await Delay(2500);

			DisplayHud(true);

			isDead = false;

			await Delay(500);
			DoScreenFadeIn(500);

			while (!IsScreenFadedIn())
				await Delay(1);

			BeginTextCommandDisplayHelp("THREESTRINGS");
			AddTextComponentSubstringPlayerName($"${HospitalBill + MoneyTaken} has been deducted from your account due to medical costs{(MoneyTaken > 0 ? " and police seizures" : "")}.");
			EndTextCommandDisplayHelp(0, false, false, 6000);
		}

		public static async void SpawnPed(float X = 196f, float Y = -933, float Z = 30.7f, bool DoFadeIn = false)
		{
			int ped = Game.PlayerPed.Handle;

			Game.PlayerPed.ClearBloodDamage();
			Game.PlayerPed.Position = new Vector3(X, Y, Z);

			if (DoFadeIn)
				DoScreenFadeIn(500);
		}
	}
}
