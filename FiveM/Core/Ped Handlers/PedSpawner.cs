using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace FiveM.Core.Ped_Handlers
{
	class PedSpawner : BaseScript
	{
		public static async void SpawnPed(float X = 196f, float Y = -933, float Z = 32f, bool DoFadeIn = false)
		{
			int ped = Game.PlayerPed.Handle;

			ReviveInjuredPed(ped);
			Game.PlayerPed.ClearBloodDamage();

			Game.PlayerPed.Position = new Vector3(X, Y, Z);

			FreezeEntityPosition(ped, false);
			SetEntityCollision(ped, true, true);
			Game.Player.CanControlCharacter = true;

			if (DoFadeIn)
				DoScreenFadeIn(500);
		}
	}
}
