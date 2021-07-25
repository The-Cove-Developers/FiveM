using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace FiveM.Core.Managers
{
	class VehicleManager : BaseScript
	{
		public VehicleManager()
		{
			Tick += OnTick;
		}

		public async Task OnTick()
		{
			VehicleBlip();
		}

		private Vehicle _lastVehicle;
		private void VehicleBlip()
		{
			if (Game.PlayerPed.IsInVehicle())
			{
				if (_lastVehicle != null && _lastVehicle.AttachedBlips.Length > 0)
				{
					foreach (var blip in _lastVehicle.AttachedBlips)
						blip.Delete();
				}

				_lastVehicle = null;

				return;
			}


			if (Game.PlayerPed.LastVehicle == null)
				return;

			if (_lastVehicle == null)
				_lastVehicle = Game.PlayerPed.LastVehicle;

			if (_lastVehicle.IsDriveable)
			{

				if (_lastVehicle.AttachedBlips.Length < 1)
				{
					_lastVehicle.AttachBlip();
					var blip = _lastVehicle.AttachedBlips[0];

					SetBlipSprite(blip.Handle, 225);
					SetBlipAsShortRange(blip.Handle, true);

					BeginTextCommandSetBlipName("STRING");
					AddTextComponentString("Vehicle");
					EndTextCommandSetBlipName(blip.Handle);
				}
			}
			else if (_lastVehicle.AttachedBlips.Length > 0)
			{
				foreach (var blip in _lastVehicle.AttachedBlips)
					blip.Delete();

				Player player = null;
				foreach (var plr in Players)
				{
					if (!_lastVehicle.HasBeenDamagedBy(plr.Character))
						continue;
					else
					{
						player = plr;
						break;
					}
				}

				BeginTextCommandDisplayHelp("THREESTRINGS");
				AddTextComponentSubstringPlayerName($"The vehicle you last used has been destroyed{(player == null ? "" : $" by {player.Name}")}!");
				EndTextCommandDisplayHelp(0, false, false, 6000);
			}














			//if (!Game.PlayerPed.IsInVehicle() && !Game.PlayerPed.IsGettingIntoAVehicle && Game.PlayerPed.LastVehicle != null)
			//{
			//	if (Game.PlayerPed.LastVehicle.AttachedBlips.Length < 1)
			//	{
			//		Game.PlayerPed.LastVehicle.AttachBlip();
			//		var blip = Game.PlayerPed.LastVehicle.AttachedBlips[0];

			//		SetBlipSprite(blip.Handle, 225);
			//		SetBlipAsShortRange(blip.Handle, true);

			//		if (_lastVehicle != null)
			//		{
			//			foreach (var oldBlip in _lastVehicle.AttachedBlips)
			//				oldBlip.Delete();
			//		}

			//		_lastVehicle = null;
			//	}
			//}
			//else if (Game.PlayerPed.IsInVehicle())
			//{
			//	Debug.WriteLine("AAA");

			//	if (_lastVehicle == null && Game.PlayerPed.LastVehicle != null)
			//		_lastVehicle = Game.PlayerPed.LastVehicle;

			//	if (_lastVehicle != null && !_lastVehicle.IsDriveable && _lastVehicle.AttachedBlips.Length > 0)
			//	{
			//		Debug.WriteLine("BB");
			//		foreach (var oldBlip in _lastVehicle.AttachedBlips)
			//		{
			//			Debug.WriteLine(oldBlip.Handle.ToString());
			//			oldBlip.Delete();
			//		}
			//	}
			//}
		}
	}
}
