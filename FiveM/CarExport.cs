using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using FiveM.Core.Managers;

namespace FiveM
{
	class CarExport : BaseScript
	{
		DateTime LastSale = new DateTime();
		Random random = new Random();
		bool IsPriceSet;
		int Price;
		bool IsSellable;

		public CarExport()
		{
			Tick += tCheckSale;

		}
		private async Task tCheckSale() => CheckSale();

		public void CheckSale()
		{
			if (!Game.PlayerPed.IsInVehicle()) return;
			CheckWarehouse();

			if (IsControlJustReleased(0, 46) && IsSellable) SellCar();
		}
		private void CheckWarehouse()
		{
			DrawMarker(27, 1204.50f, -3116.6f, 4.6f, 0, 0, 0, 0, 0, 0, 11, 11, 1, 250, 232, 117, 255, false, true, 2, true, null, null, false);

			if (World.GetDistance(Game.PlayerPed.Position, new Vector3(1204.50f, -3116.6f, 5)) < 6)
			{
				if (LastSale != null && (DateTime.Now - LastSale).TotalMinutes < 49)
				{
					IsSellable = false;
					AddTextEntry("Price", $"You have recently sold another vehicle");
					DisplayHelpTextThisFrame("Price", false);
				}
				else
				{
					if (Game.PlayerPed.LastVehicle.ClassType == VehicleClass.Compacts || Game.PlayerPed.LastVehicle.ClassType == VehicleClass.Sedans || Game.PlayerPed.LastVehicle.ClassType == VehicleClass.SUVs || Game.PlayerPed.LastVehicle.ClassType == VehicleClass.Coupes || Game.PlayerPed.LastVehicle.ClassType == VehicleClass.Muscle || Game.PlayerPed.LastVehicle.ClassType == VehicleClass.Motorcycles)
					{
						IsSellable = true;
						if (!IsPriceSet)
						{
							Price = random.Next(7, 14) * (Game.PlayerPed.LastVehicle.Health / 7);
							IsPriceSet = true;
						}
						AddTextEntry("Price", $"Price for this vehicle: ${((int)Price)}\nPress ~INPUT_CONTEXT~ to sell this vehicle!");
						DisplayHelpTextThisFrame("Price", false);
					}
					else
					{
						IsSellable = false;
						AddTextEntry("Price", $"You are unable to sell this vehicle!");
						DisplayHelpTextThisFrame("Price", false);
					}
				}
			}
			else if (World.GetDistance(Game.PlayerPed.Position, new Vector3(1204.50f, -3116.6f, 5)) > 5 && IsPriceSet) { IsSellable = false; IsPriceSet = false; }
		}
		private async void SellCar()
		{
			DoScreenFadeOut(500);
			await Delay(500);
			LastSale = DateTime.Now;
			Game.PlayerPed.CurrentVehicle.Delete();
			Game.PlayerPed.Position = new Vector3(1177.80f, -3113.80f, 5);

			MoneyHandler.AddMoney(Price);

			await Delay(500);

			DoScreenFadeIn(500);
		}
	}
}
