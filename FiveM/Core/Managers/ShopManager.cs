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
using FiveM.Core.Misc.Shop;


namespace FiveM.Core.Managers
{
	class ShopManager : BaseScript
	{
		public List<Shop> Shops = new List<Shop>
		{
			new Shop(new Vector3(-813.71356201172f, -184.06265258789f, 36f), 71, "Barber", ShopType.Barber),
			new Shop(new Vector3(136.97842407227f, -1707.8671875f, 28f), 71, "Barber", ShopType.Barber),
			new Shop(new Vector3(-1282.8363037109f, -1116.9685058594f, 5.8f), 71, "Barber", ShopType.Barber),
			new Shop(new Vector3(1931.7169189453f, 3730.3142089844f, 31f), 71, "Barber", ShopType.Barber),
			new Shop(new Vector3(1212.4298095703f, -472.55453491211f, 65f), 71, "Barber", ShopType.Barber),
			new Shop(new Vector3(-32.703586578369f, -152.55470275879f, 56f), 71, "Barber", ShopType.Barber),
			new Shop(new Vector3(-278.02655029297f, 6228.3115234375f, 30f), 71, "Barber", ShopType.Barber)
		};

		public ShopManager()
		{
			Tick += ShopManager_Tick;
		}
		private async Task ShopManager_Tick() => HandleShops();

		private static Menu ShopMenu = null;
		
		private void HandleShops()
		{
			if (MenuController.IsAnyMenuOpen())
				return;

			Shop closestShop = null;

			foreach (var a in Shops)
			{
				if (!MenuController.IsAnyMenuOpen())
					DrawBlipAndMarker(a);

				if (World.GetDistance(Game.PlayerPed.Position, a.Location) < 5)
					closestShop = a;
			}

			if (closestShop == null)
			{
				if (ShopMenu != null)
				{
					MenuController.DontOpenAnyMenu = true;
					MenuController.Menus.Clear();

					ShopMenu = null;
				}

				return;
			}

			if (ShopMenu == null)
			{
				switch (closestShop.ShopType)
				{
					case ShopType.Barber:
						ShopMenu = new BarberShop().GetShopMenu();
						break;
				}

				MenuController.AddMenu(ShopMenu);
				MenuController.DontOpenAnyMenu = false;
			}

			if (World.GetDistance(Game.PlayerPed.Position, closestShop.Location) > closestShop.Distance)
				return;

			if (IsControlJustReleased(0, 46))
			{
				ShopMenu.OpenMenu();
			}
			else
			{
				AddTextEntry("Price", $"Press ~INPUT_CONTEXT~ to open shop menu!");
				DisplayHelpTextThisFrame("Price", false);
			}
		}

		private static void DrawBlipAndMarker(Shop shop)
		{
			DrawMarker(1, shop.Location.X, shop.Location.Y, shop.Location.Z, 0, 0, 0, 0, 0, 0, shop.ScaleX, shop.ScaleY, shop.ScaleZ, 250, 232, 117, 255, false, true, 2, false, null, null, false);

			if (shop.Blip == null)
			{
				int blipId = AddBlipForCoord(shop.Location.X, shop.Location.Y, shop.Location.Z);
				SetBlipSprite(blipId, (int)shop.SpriteID);
				SetBlipAsShortRange(blipId, true);
				shop.Blip = new Blip(blipId);
			}
		}
	}
}
