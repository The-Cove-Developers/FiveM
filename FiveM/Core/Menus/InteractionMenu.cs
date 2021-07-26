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

namespace FiveM.Core.Menus
{
	class InteractionMenu : BaseScript
	{
		//PrepareServerEvents PSE = new PrepareServerEvents();

		public Menu MainInteractionMenu;

		Menu AdminMenu;
		Menu CashMenu;
		Menu InventoryMenu;
		Menu WeaponMenu;
		Menu VehicleMenu;
		Menu ServicesMenu;
		Menu ServerMenu;

		Menu PlayerSubMenu;
		MenuItem PlayerItem;

		public Menu CreateInteractionMenu()
		{
			//if (MainInteractionMenu != null) return MainInteractionMenu;

			MainInteractionMenu = new Menu(Game.Player.Name, "Interaction Menu");

			AddMenu(MainInteractionMenu, CreateAdminMenu(), AddAdminMenu());
			AddMenu(MainInteractionMenu, CreateCashMenu(), AddCashMenu());
			AddMenu(MainInteractionMenu, CreateInventoryMenu(), AddInventoryMenu());
			AddMenu(MainInteractionMenu, CreateWeaponMenu(), AddWeaponMenu());
			AddMenu(MainInteractionMenu, CreateVehicleMenu(), AddVehicleMenu());
			AddMenu(MainInteractionMenu, CreateServicesMenu(), AddServicesMenu());
			AddMenu(MainInteractionMenu, CreateServerMenu(), AddServerMenu());

			MenuController.AddMenu(MainInteractionMenu);

			MenuController.MainMenu = MainInteractionMenu;

			return MainInteractionMenu;
		}

		#region AdminMenu
		private Menu CreateAdminMenu()
		{
			PlayerSubMenu = new Menu("Player tools", null);

			PlayerSubMenu.ClearMenuItems();

			AdminMenu = new Menu("Admin tools", "Useful tools for server admins");

			foreach (Player player in Players)
			{
				PlayerItem = new MenuItem(player.Name, $"Player options for {player.Name}") { Label = $"#{player.ServerId} →→→" };
				AdminMenu.AddMenuItem(PlayerItem);
				MenuController.BindMenuItem(AdminMenu, PlayerSubMenu, PlayerItem);
			}

			PlayerSubMenu.AddMenuItem(AddKickOption());
			PlayerSubMenu.AddMenuItem(AddBanOption());
			PlayerSubMenu.AddMenuItem(AddCoordOption());

			MenuController.AddMenu(PlayerSubMenu);

			return AdminMenu;
		}
		private MenuItem AddAdminMenu() => new MenuItem("Admin tools", "Tools for server moderators/administrators") { Label = "→→→" };
		private MenuItem AddKickOption() => new MenuItem("Kick", "Kicks the player from the server");
		private MenuItem AddBanOption() => new MenuItem("Ban", "Bans the player from the server");
		private MenuItem AddCoordOption() => new MenuItem("Co-ords", "Prints the co-ords for the player");
		#endregion

		#region Cash Menu
		private Menu CreateCashMenu()
		{
			//if (CashMenu != null) return CashMenu;

			CashMenu = new Menu("Cash", "Cash management options") { Visible = false };

			MenuController.AddMenu(CashMenu);

			return CashMenu;
		}
		private MenuItem AddCashMenu() => new MenuItem("Cash", "Money options") { Label = "→→→" };
		#endregion

		#region Inventory Menu
		private Menu CreateInventoryMenu()
		{
			//if (InventoryMenu != null) return InventoryMenu;

			InventoryMenu = new Menu("Inventory", "Inventory management options") { Visible = false };

			MenuController.AddMenu(InventoryMenu);

			return InventoryMenu;
		}
		private MenuItem AddInventoryMenu() => new MenuItem("Inventory", "Inventory options") { Label = "→→→" };
		#endregion

		#region Weapon Menu
		private Menu CreateWeaponMenu()
		{
			//if (WeaponMenu != null) return WeaponMenu;

			WeaponMenu = new Menu("Weapons", "Weapon management options") { Visible = false };

			MenuController.AddMenu(WeaponMenu);

			return WeaponMenu;
		}
		private MenuItem AddWeaponMenu() => new MenuItem("Weapons", "Weapon options") { Label = "→→→" };
		#endregion

		#region Vehicle Menu
		private Menu CreateVehicleMenu()
		{
			//if (VehicleMenu != null) return VehicleMenu;

			VehicleMenu = new Menu("Vehicles", "Vehicle management options") { Visible = false };

			MenuController.AddMenu(VehicleMenu);

			return VehicleMenu;
		}
		private MenuItem AddVehicleMenu() => new MenuItem("Vehicle", "Vehicle options") { Label = "→→→" };
		#endregion

		#region Services Menu
		private Menu CreateServicesMenu()
		{
			//if (ServicesMenu != null) return ServicesMenu;

			ServicesMenu = new Menu("Services", "Services management options") { Visible = false };

			MenuController.AddMenu(ServicesMenu);

			return ServicesMenu;
		}
		private MenuItem AddServicesMenu() => new MenuItem("Services", "World service options") { Label = "→→→" };
		#endregion

		#region Server Menu
		private Menu CreateServerMenu()
		{
			//if (ServerMenu != null) return ServerMenu;

			ServerMenu = new Menu("Server", "Server management options") { Visible = false };

			MenuController.AddMenu(ServerMenu);

			ServerMenu.AddMenuItem(DisconnectMenuItem());
			ServerMenu.AddMenuItem(ScuicideMenuItem());

			ServerMenu.OnItemSelect += async (sender, item, index) => {
				Debug.WriteLine(item.Text);
				if (item.Text == "Disconnect") TriggerServerEvent("dFrame:DisconnectPlayer", Game.Player.ServerId);
				else if (item.Text == "~r~Commit Scuicide") Game.PlayerPed.Kill();
			};

			return ServerMenu;
		}
		private MenuItem DisconnectMenuItem() => new MenuItem("Disconnect", "Disconnects you from the server");
		private MenuItem ScuicideMenuItem() => new MenuItem("~r~Commit Scuicide", "Kills your player and respawns you");
		private MenuItem AddServerMenu() => new MenuItem("Server", "Misc server settings (Such as reports)") { Label = "→→→" };
		#endregion

		public void AddMenu(Menu parentMenu, Menu subMenu, MenuItem menuItem)
		{
			parentMenu.AddMenuItem(menuItem);
			MenuController.AddSubmenu(parentMenu, subMenu);
			MenuController.BindMenuItem(parentMenu, subMenu, menuItem);
			subMenu.RefreshIndex();
		}
	}
}
