using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace FiveM.Core.Ped_Handlers
{
	class PedSpawner : BaseScript
	{
		public static async void SpawnPed(float X = 196f, float Y = -933, float Z = 32f, float heading = 0, bool DoFadeIn = false)
		{
			int ped = Game.PlayerPed.Handle;

			ReviveInjuredPed(ped);
			Game.PlayerPed.ClearBloodDamage();

			Game.PlayerPed.Position = new Vector3(X, Y, Z);
			Game.PlayerPed.Heading = heading;

			FreezeEntityPosition(ped, false);
			SetEntityCollision(ped, true, true);
			Game.Player.CanControlCharacter = true;

			if (DoFadeIn)
				DoScreenFadeIn(500);
		}

		public static async void CreatePed(Player player, int InitialFatherFace, int InitialMotherFace, int InitialHairStyle, int InitialHairColour, int InitialEyeColour, string Model)
		{
			uint model = (uint)GetHashKey(Model);

			RequestModel(model);

			while (!HasModelLoaded(model))
			{
				RequestModel(model);
				await Delay(10);
			}


			while (!await player.ChangeModel(Model))
				await Delay(10);

			int ped = player.Character.Handle;

			SetPedHeadBlendData(ped, InitialFatherFace, InitialMotherFace, 0, InitialFatherFace, InitialMotherFace, 0, Menus.CharacterCreationMenu.mixValues[5], Menus.CharacterCreationMenu.mixValues[5], 0, false);

			SetPedComponentVariation(ped, 2, InitialHairStyle, 0, 1);
			SetPedHairColor(ped, InitialHairColour, InitialHairColour);

			if (Model == "mp_m_freemode_01")
			{
				SetPedComponentVariation(ped, 3, 15, 0, 1); // Arms
				SetPedComponentVariation(ped, 4, 14, 1, 1); // Pants
				SetPedComponentVariation(ped, 5, 9, 0, 1); // Bags
				SetPedComponentVariation(ped, 6, 34, 0, 1); // Feet
				SetPedComponentVariation(ped, 11, 15, 0, 1); // Top
				SetPedComponentVariation(ped, 8, 15, 0, 1); // Top
			}
			else if (Model == "mp_f_freemode_01")
			{
				SetPedComponentVariation(ped, 3, 4, 0, 1); // Arms
				SetPedComponentVariation(ped, 4, 14, 8, 1); // Pants
				SetPedComponentVariation(ped, 5, 9, 0, 1); // Bags
				SetPedComponentVariation(ped, 6, 35, 0, 1); // Feet
				SetPedComponentVariation(ped, 11, 5, 0, 1); // Top
				SetPedComponentVariation(ped, 8, 3, 0, 1); // Top
			}

			SetPedEyeColor(ped, InitialEyeColour);

			return;
		}
	}
}
