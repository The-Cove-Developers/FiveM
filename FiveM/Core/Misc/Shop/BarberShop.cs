using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenuAPI;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using static CitizenFX.Core.UI.Screen;
using static CitizenFX.Core.Native.API;
using System.Drawing;

namespace FiveM.Core.Misc.Shop
{
	class BarberShop : BaseScript
	{
		public Menu GetShopMenu()
		{
			int ped = Game.PlayerPed.Handle;

			Menu BarberMenu = new Menu("Barbers", null);
			MenuListItem HairStyle;

			Menus.CharacterCreationMenu.GenerateComponentData();

			#region Ref ints/Floats
			int junk = 0;

			int intBeardStyle = 0;
			int intBeardColour = 0;
			float floatBeardOpacity = 0;
			GetPedHeadOverlayData(ped, 1, ref intBeardStyle, ref junk, ref intBeardColour, ref junk, ref floatBeardOpacity);

			int intEyebrowsStyle = 0;
			int intEyebrowsColour = 0;
			float floatEyebrowsOpacity = 0;
			GetPedHeadOverlayData(ped, 1, ref intEyebrowsStyle, ref junk, ref intEyebrowsColour, ref junk, ref floatEyebrowsOpacity);

			int intMakeupStyle = 0;
			int intMakeupColour = 0;
			float floatMakeupOpacity = 0;
			GetPedHeadOverlayData(ped, 1, ref intMakeupStyle, ref junk, ref intMakeupColour, ref junk, ref floatMakeupOpacity);

			int intBlushStyle = 0;
			int intBlushColour = 0;
			float floatBlushOpacity = 0;
			GetPedHeadOverlayData(ped, 1, ref intBlushStyle, ref junk, ref intBlushColour, ref junk, ref floatBlushOpacity);

			int intLipstickStyle = 0;
			int intLipstickColour = 0;
			float floatLipstickOpacity = 0;
			GetPedHeadOverlayData(ped, 1, ref intLipstickStyle, ref junk, ref intLipstickColour, ref junk, ref floatLipstickOpacity);
			#endregion

			#region MenuItems
			if (Game.PlayerPed.Model == "mp_m_freemode_01") HairStyle = new MenuListItem("Hair", Menus.CharacterCreationMenu.MaleHair, GetPedDrawableVariation(ped, 2));
			else HairStyle = new MenuListItem("Hair", Menus.CharacterCreationMenu.FemaleHair, GetPedDrawableVariation(ped, 2));

			MenuListItem HairColour = new MenuListItem("Hair Colour",  Menus.CharacterCreationMenu.OverlayColours, GetPedHairColor(ped)) { ShowColorPanel = true, ColorPanelColorType = MenuListItem.ColorPanelType.Hair };
			MenuListItem HairHighlights = new MenuListItem("Hair Highlights", Menus.CharacterCreationMenu.OverlayColours, GetPedHairHighlightColor(ped)) { ShowColorPanel = true, ColorPanelColorType = MenuListItem.ColorPanelType.Hair };

			MenuListItem BeardStyle = new MenuListItem("Beard", Menus.CharacterCreationMenu.BeardList, intBeardStyle);
			MenuListItem BeardOpacity = new MenuListItem("Beard Opacity", Menus.CharacterCreationMenu.Opacity, 5) { ShowOpacityPanel = true };
			MenuListItem BeardColour = new MenuListItem("Beard Colour", Menus.CharacterCreationMenu.OverlayColours, intBeardColour) { ShowColorPanel = true, ColorPanelColorType = MenuListItem.ColorPanelType.Hair };

			MenuListItem EyebrowsStyle = new MenuListItem("Eyebrows", Menus.CharacterCreationMenu.EyebrowsList, intEyebrowsStyle);
			MenuListItem EyebrowsOpacity = new MenuListItem("Eyebrow Opacity", Menus.CharacterCreationMenu.Opacity, 5) { ShowOpacityPanel = true };
			MenuListItem EyebrowsColour = new MenuListItem("Eyebrow Colour", Menus.CharacterCreationMenu.OverlayColours, intEyebrowsColour) { ShowColorPanel = true, ColorPanelColorType = MenuListItem.ColorPanelType.Hair };

			MenuListItem MakeupStyle = new MenuListItem("Makeup", Menus.CharacterCreationMenu.MakeupList, intMakeupStyle);
			MenuListItem MakeupOpacity = new MenuListItem("Makeup Opacity", Menus.CharacterCreationMenu.Opacity, 5) { ShowOpacityPanel = true };
			MenuListItem MakeupColour = new MenuListItem("Makeup Colour", Menus.CharacterCreationMenu.OverlayColours, intMakeupColour) { ShowColorPanel = true, ColorPanelColorType = MenuListItem.ColorPanelType.Makeup };

			MenuListItem BlushStyle = new MenuListItem("Blush", Menus.CharacterCreationMenu.BlushList, intBlushStyle);
			MenuListItem BlushOpacity = new MenuListItem("Blush Opacity", Menus.CharacterCreationMenu.Opacity, 5) { ShowOpacityPanel = true };
			MenuListItem BlushColour = new MenuListItem("Blush Colour", Menus.CharacterCreationMenu.OverlayColours, intBlushColour) { ShowColorPanel = true, ColorPanelColorType = MenuListItem.ColorPanelType.Makeup };

			MenuListItem LipstickStyle = new MenuListItem("Lipstick", Menus.CharacterCreationMenu.LipstickList, intLipstickStyle);
			MenuListItem LipstickOpacity = new MenuListItem("Lipstick Opacity", Menus.CharacterCreationMenu.Opacity, 5) { ShowOpacityPanel = true };
			MenuListItem LipstickColour = new MenuListItem("Lipstick Colour", Menus.CharacterCreationMenu.OverlayColours, intLipstickColour) { ShowColorPanel = true, ColorPanelColorType = MenuListItem.ColorPanelType.Makeup };

			MenuListItem EyeColour = new MenuListItem("Eye Colour", Menus.CharacterCreationMenu.EyeColours, GetPedEyeColor(ped));

			BarberMenu.AddMenuItem(HairStyle);
			BarberMenu.AddMenuItem(HairColour);
			BarberMenu.AddMenuItem(HairHighlights);
			if (CharacterManagement.SaveHandler.Character.Gender == Gender.Male)
			{
				BarberMenu.AddMenuItem(BeardStyle);
				BarberMenu.AddMenuItem(BeardOpacity);
				BarberMenu.AddMenuItem(BeardColour);
			}
			BarberMenu.AddMenuItem(EyebrowsStyle);
			BarberMenu.AddMenuItem(EyebrowsOpacity);
			BarberMenu.AddMenuItem(EyebrowsColour);
			BarberMenu.AddMenuItem(MakeupStyle);
			BarberMenu.AddMenuItem(MakeupOpacity);
			BarberMenu.AddMenuItem(MakeupColour);
			BarberMenu.AddMenuItem(BlushStyle);
			BarberMenu.AddMenuItem(BlushOpacity);
			BarberMenu.AddMenuItem(BlushColour);
			BarberMenu.AddMenuItem(LipstickStyle);
			BarberMenu.AddMenuItem(LipstickOpacity);
			BarberMenu.AddMenuItem(LipstickColour);
			BarberMenu.AddMenuItem(EyeColour);
			#endregion

			BarberMenu.OnListIndexChange += async (menu, listItem, oldIndex, newIndex, itemIndex) =>
			{
				switch (listItem.Text)
				{
					case "Hair":
						SetPedComponentVariation(Game.PlayerPed.Handle, 2, HairStyle.ListIndex, 0, 1);
						break;
					case "Hair Colour":
					case "Hair Highlights":
						SetPedHairColor(Game.PlayerPed.Handle, HairColour.ListIndex, HairHighlights.ListIndex);
						break;
					case "Beard":
					case "Beard Opacity":
					case "Beard Colour":
						SetPedHeadOverlay(Game.PlayerPed.Handle, 1, BeardStyle.ListIndex, (float)BeardOpacity.ListIndex / 10);
						SetPedHeadOverlayColor(Game.PlayerPed.Handle, 1, 1, BeardColour.ListIndex, BeardColour.ListIndex);
						break;
					case "Eyebrows":
					case "Eyebrow Opacity":
					case "Eyebrow Colour":
						SetPedHeadOverlay(Game.PlayerPed.Handle, 2, EyebrowsStyle.ListIndex, (float)EyebrowsOpacity.ListIndex / 10);
						SetPedHeadOverlayColor(Game.PlayerPed.Handle, 2, 1, EyebrowsColour.ListIndex, EyebrowsColour.ListIndex);
						break;
					case "Makeup":
					case "Makeup Opacity":
					case "Makeup Colour":
						SetPedHeadOverlay(Game.PlayerPed.Handle, 4, MakeupStyle.ListIndex, (float)MakeupOpacity.ListIndex / 10);
						SetPedHeadOverlayColor(Game.PlayerPed.Handle, 4, 1, MakeupColour.ListIndex, MakeupColour.ListIndex);
						break;
					case "Blush":
					case "Blush Opacity":
					case "Blush Colour":
						SetPedHeadOverlay(Game.PlayerPed.Handle, 5, BlushStyle.ListIndex, (float)BlushOpacity.ListIndex / 10);
						SetPedHeadOverlayColor(Game.PlayerPed.Handle, 5, 1, BlushColour.ListIndex, BlushColour.ListIndex);
						break;
					case "Lipstick":
					case "Lipstick Opacity":
					case "Lipstick Colour":
						SetPedHeadOverlay(Game.PlayerPed.Handle, 8, LipstickStyle.ListIndex, (float)LipstickOpacity.ListIndex / 10);
						SetPedHeadOverlayColor(Game.PlayerPed.Handle, 8, 1, LipstickColour.ListIndex, LipstickColour.ListIndex);
						break;
					case "Eye Colour":
						SetPedEyeColor(Game.PlayerPed.Handle, EyeColour.ListIndex);
						break;
				}
			};
			BarberMenu.OnMenuOpen += async (menu) => {
				FreezeEntityPosition(Game.PlayerPed.Handle, true);
				SetEntityCollision(Game.PlayerPed.Handle, false, false);
				SetEntityInvincible(Game.PlayerPed.Handle, true);
				Game.Player.CanControlCharacter = false;
			};
			BarberMenu.OnMenuClose += async (menu) => {
				FreezeEntityPosition(Game.PlayerPed.Handle, false);
				SetEntityCollision(Game.PlayerPed.Handle, true, true);
				SetEntityInvincible(Game.PlayerPed.Handle, false);
				Game.Player.CanControlCharacter = true;
			};

			return BarberMenu;
		}
	}
}
