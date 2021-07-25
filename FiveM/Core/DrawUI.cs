using System;
using System.Collections.Generic;
using MenuAPI;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using static CitizenFX.Core.UI.Screen;
using static CitizenFX.Core.Native.API;
using System.Threading.Tasks;
using CitizenFX.Core.Native;
using FiveM.Core.Managers;

namespace FiveM.Core
{
	class DrawUI : BaseScript
	{
		public DrawUI()
		{
			Tick += OnTick;
		}

		private async Task OnTick()
		{
			if (Game.PlayerPed.IsDead)
			{
				HideHudAndRadarThisFrame();
				return;
			}

			if (!Hud.IsVisible || IsPauseMenuActive() || IsPauseMenuRestarting() || IsHudHidden() || IsPlayerSwitchInProgress())
			{
				BlockWeaponWheelThisFrame();
				return;
			}

			HideHudComponentThisFrame((int)HudComponent.StreetName);
			//ShowHudComponentThisFrame((int)HudComponent.WeaponIcon);

			bool isInVehicle = Game.PlayerPed.IsInVehicle();

			//if (!isInVehicle)
			//	DisplayRadar(false);
			//else
			//DisplayRadar(true);

			float xPos = (float)(0.208f + (1 / GetSafeZoneSize() / 3.1f) - /*(isInVehicle ?*/ 0.377f /*: /*0.52*/);
			float yPos = 0f;

			//Cash
			yPos = GetSafeZoneSize() - (isInVehicle ? GetTextScaleHeight(1.8f, 1) : GetTextScaleHeight(1.3f, 1));
			DrawText($"${MoneyHandler.Money}", xPos, yPos, 0.8f, 0.5f, (int)Alignment.Left, 4, 150, 255, 150, 255);

			//Time
			yPos = GetSafeZoneSize() - (isInVehicle ? GetTextScaleHeight(1.3f, 1) : GetTextScaleHeight(0.8f, 1));
			DrawText($"{World.CurrentDayTime.Hours.ToString("00")}:{World.CurrentDayTime.Minutes.ToString("00")}", xPos, yPos, 0.8f, 0.5f, (int)Alignment.Left, 4, 117, 117, 117, 255);

			//Speed
			yPos = GetSafeZoneSize() - GetTextScaleHeight(0.8f, 1);
			if (Game.PlayerPed.IsInVehicle()) DrawText($"{Math.Round(GetEntitySpeed(Game.PlayerPed.Handle) * 2.23694f)} MPH", xPos, yPos, 0.8f, 0.5f, (int)Alignment.Left, 4, 255, 255, 255, 255);

			//Street name and heading
			yPos = GetSafeZoneSize() - GetTextScaleHeight(0.3f, 1);
			DrawText($"{Direction(Game.PlayerPed.Heading)} | {World.GetStreetName(Game.PlayerPed.Position)}", xPos, yPos, 0.8f, 0.5f, (int)Alignment.Left, 4, 255, 255, 255, 255);
			
			return;
		}

		private void DrawText(string Text, float xPos, float yPos, float Scale, float size, int alignment, int font, int r, int g, int b, int alpha)
		{
			SetTextFont(font);
			SetTextScale(Scale, size);
			SetTextJustification(alignment);
			SetTextOutline();
			BeginTextCommandDisplayText("STRING");
			AddTextComponentSubstringPlayerName(Text);
			SetTextColour(r, g, b, alpha);
			EndTextCommandDisplayText(xPos, yPos);
		}

		private string Direction(float heading)
		{
			if (heading >= 22.5 && heading < 67.5) return "NE";
			else if (heading >= 67.5 && heading < 112.5) return "E";
			else if (heading >= 112.5 && heading < 157.5) return "SE";
			else if (heading >= 157.5 && heading < 202.5) return "S";
			else if (heading >= 202.5 && heading < 247.5) return "SW";
			else if (heading >= 247.5 && heading < 292.5) return "W";
			else if (heading >= 292.5 && heading < 337.5) return "NW";
			else if (heading >= 337.5 || heading < 22.5) return "N";

			else return "Oh no, how'd on earth did i get here? Do you know? Guess not...\nYou should probably go and tell someone that i'm here, perhaps they know";
		}
	}
}
