using CitizenFX.Core;
using MenuAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using static CitizenFX.Core.Native.API;
using FiveM.Core.Ped_Handlers;
using FiveM.Core.Menus;
using CCM = FiveM.Core.Menus.CharacterCreationMenu;
using System.Threading.Tasks;

namespace FiveM.Core.CharacterManagement
{
	class CharacterCreation : BaseScript
	{
		public CharacterCreation()
		{
			EventHandlers["DSCP5M:LoadCharacterMenu"] += new Action<string>(PlayerStart);
		}

		private async void PlayerStart(string characterJson)
		{
			Player player = Game.Player;

			SetEntityCoords(player.Character.Handle, 410, -999f, -100, true, false, false, true);
			player.Character.Heading = 180f;

			FreezeEntityPosition(player.Handle, true);
			SetEntityCollision(player.Handle, false, false);
			SetEntityInvincible(player.Handle, true);
			Game.Player.CanControlCharacter = false;

			int cam = CreateCam("DEFAULT_SCRIPTED_CAMERA", true);
			Camera MainCamera = new Camera(cam);
			MainCamera.Position = new Vector3(-306.7f, 202.5f, 200);
			MainCamera.PointAt(new Vector3(305.4f, -1816.3f, 110));
			Game.PlayerPed.Task.ClearAllImmediately();

			RenderScriptCams(true, false, 0, true, true);

			Menu CharacterMenu = new Menu("Character menu", "Select or create a character");

			List<Misc.Character> Characters = new List<Misc.Character>();

			if (!string.IsNullOrEmpty(characterJson))
			{
				try
				{
					foreach (string character in JsonConvert.DeserializeObject<List<string>>(characterJson))
						Characters.Add(JsonConvert.DeserializeObject<Misc.Character>(character));
					foreach(var character in Characters)
					{
						MenuItem CharacterItem = new MenuItem($"{character.Name}") { Label = $"({character.Gender})" };
						CharacterMenu.AddMenuItem(CharacterItem);
					}
				}
				catch(Exception e) { Debug.WriteLine(e.ToString());  }
			}

			MenuItem NewCharacter = new MenuItem($"New character", "Create a new character");
			CharacterMenu.AddMenuItem(NewCharacter);

			MenuController.AddMenu(CharacterMenu);

			await Delay(800);

			CharacterMenu.OnItemSelect += async (menu, item, index) =>
			{
				if (item == NewCharacter)
				{
					DoScreenFadeOut(500);
					await Delay(500);
					MenuController.CloseAllMenus();
					await Delay(500);

					MenuController.Menus.Remove(CharacterMenu);

					CreateCharacter();
				}
				else
				{
					MenuController.CloseAllMenus();

					MenuController.Menus.Clear();
					MenuController.DontOpenAnyMenu = true;

					SpawnPed(Characters.ElementAt(index));			
				}
			};

			CharacterMenu.OnMenuOpen += (menu) =>
			{
				MenuController.DisableBackButton = true;
				MenuController.PreventExitingMenu = true;
			};
			CharacterMenu.OnMenuClose += (menu) =>
			{
				MenuController.DisableBackButton = false;
				MenuController.PreventExitingMenu = false;
			};


			ShutdownLoadingScreen();

			DoScreenFadeIn(500);

			CharacterMenu.OpenMenu();
		}

		private async Task CreateCharacter()
		{
			if (Game.IsLoading || IsNetworkLoadingScene())
				await Delay(250);
			var random = new Random();

			while (Game.PlayerPed.Model == new Model())
				await Delay(10);

			SetCamera();
			CreateRandomPed();

			var ccm = new CCM().GetCCMenu();

			ccm.OnMenuOpen += (menu) =>
			{
				MenuController.DisableBackButton = true;
				MenuController.PreventExitingMenu = true;
			};
			ccm.OnMenuClose += (menu) =>
			{
				MenuController.DisableBackButton = false;
				MenuController.PreventExitingMenu = false;
			};

			MenuController.AddMenu(ccm);
			MenuController.MainMenu = ccm;

			await Delay(500);

			ccm.OpenMenu();

			ShutdownLoadingScreen();

			DoScreenFadeIn(500);
		}

		private async void SetCamera()
		{
			DisplayRadar(false);

			Player player = Game.Player;

			SetEntityCoords(player.Character.Handle, 402.85f, -996.5f, -100, true, false, false, true);
			player.Character.Heading = 180f;

			FreezeEntityPosition(player.Handle, true);
			SetEntityCollision(player.Handle, false, false);
			SetEntityInvincible(player.Handle, true);
			Game.Player.CanControlCharacter = false;

			int cam = CreateCam("DEFAULT_SCRIPTED_CAMERA", true);
			CCM.MainCamera = new Camera(cam);
			CCM.MainCamera.Position = CCM.BodyPos;
			CCM.MainCamera.PointAt(CCM.FullBodyLookAt);

			Game.PlayerPed.Task.ClearAllImmediately();

			RenderScriptCams(true, false, 0, true, true);

			await Delay(0);

			return;
		}

		private void CreateRandomPed()
		{
			var random = new Random();
			PedSpawner.CreatePed(Game.Player, CCM.InitialFatherFace = random.Next(0, 46), CCM.InitialMotherFace = random.Next(0, 46), CCM.InitialHairStyle = random.Next(0, 36),
							CCM.InitialHairColour = random.Next(0, GetNumHairColors() - 4), CCM.InitialEyeColour = random.Next(0, 31), "mp_m_freemode_01");

		}

		private async void SpawnPed(Misc.Character Character)
		{
			DoScreenFadeOut(500);

			await Delay(500);

			MenuController.CloseAllMenus();

			RenderScriptCams(false, false, 0, false, false);

			Model model;
			if (Character.Gender == Gender.Male) model = new Model("mp_m_freemode_01");
			else model = new Model("mp_f_freemode_01");

			RequestModel((uint)model.Hash);

			while (!HasModelLoaded((uint)model.Hash)) await Delay(0);

			await Game.Player.ChangeModel(model);

			int ped = Game.PlayerPed.Handle;

			SetPlayerInvincible(ped, true);

			SetPedHeadBlendData(ped, Character.FaceData.FatherFace, Character.FaceData.MotherFace, 0, Character.FaceData.FatherSkin, Character.FaceData.MotherSkin, 0, Character.FaceData.ShapePercent, Character.FaceData.SkinPercent, 0, false);

			#region FaceFeatures
			SetPedFaceFeature(ped, 0, Character.FaceData.NoseWidth);
			SetPedFaceFeature(ped, 1, Character.FaceData.NoesPeakHeight);
			SetPedFaceFeature(ped, 2, Character.FaceData.NosePeakLength);
			SetPedFaceFeature(ped, 3, Character.FaceData.NoseBoneHeight);
			SetPedFaceFeature(ped, 4, Character.FaceData.NosePeakLowering);
			SetPedFaceFeature(ped, 5, Character.FaceData.NoseBoneTwist);
			SetPedFaceFeature(ped, 6, Character.FaceData.EyebrowsHeight);
			SetPedFaceFeature(ped, 7, Character.FaceData.EyebrowsDepth);
			SetPedFaceFeature(ped, 8, Character.FaceData.CheekbonesHeight);
			SetPedFaceFeature(ped, 9, Character.FaceData.CheekbonesWidth);
			SetPedFaceFeature(ped, 10, Character.FaceData.CheeksWidth);
			SetPedFaceFeature(ped, 11, Character.FaceData.EyesOpening);
			SetPedFaceFeature(ped, 12, Character.FaceData.LipsThickness);
			SetPedFaceFeature(ped, 13, Character.FaceData.JawBoneWidth);
			SetPedFaceFeature(ped, 14, Character.FaceData.JawBoneDepthLength);
			SetPedFaceFeature(ped, 15, Character.FaceData.ChinHeight);
			SetPedFaceFeature(ped, 16, Character.FaceData.ChinDepthLength);
			SetPedFaceFeature(ped, 17, Character.FaceData.ChinWidth);
			SetPedFaceFeature(ped, 18, Character.FaceData.ChinHoleSize);
			SetPedFaceFeature(ped, 19, Character.FaceData.NeckThickness);
			#endregion
			#region OverlayData
			SetPedComponentVariation(ped, 2, Character.ComponentData.Hair, 0, 1);
			SetPedHairColor(ped, Character.ComponentData.HairColour, Character.ComponentData.HairHighlight);

			SetPedHeadOverlay(ped, 0, Character.OverlayData.Blemish, Character.OverlayData.BlemishOpacity);

			SetPedHeadOverlay(ped, 1, Character.OverlayData.FacialHair, Character.OverlayData.FacialHairOpacity);
			SetPedHeadOverlayColor(ped, 1, 1, Character.OverlayData.FacialHairColour, Character.OverlayData.FacialHairColour);

			SetPedHeadOverlay(ped, 2, Character.OverlayData.Eyebrows, Character.OverlayData.EyebrowsOpacity);
			SetPedHeadOverlayColor(ped, 2, 1, Character.OverlayData.EyebrowsColour, Character.OverlayData.EyebrowsColour);

			SetPedHeadOverlay(ped, 3, Character.OverlayData.Ageing, Character.OverlayData.AgeingOpacity);

			SetPedHeadOverlay(ped, 4, Character.OverlayData.Makeup, Character.OverlayData.MakeupOpacity);
			SetPedHeadOverlayColor(ped, 4, 1, Character.OverlayData.MakeupColour, Character.OverlayData.MakeupColour);

			SetPedHeadOverlay(ped, 5, Character.OverlayData.Blush, Character.OverlayData.BlushOpacity);
			SetPedHeadOverlayColor(ped, 5, 1, Character.OverlayData.BlushColour, Character.OverlayData.BlushColour);

			SetPedHeadOverlay(ped, 6, Character.OverlayData.Complexion, Character.OverlayData.ComplexionOpacity);

			SetPedHeadOverlay(ped, 7, Character.OverlayData.Sundamage, Character.OverlayData.SundamageOpacity);

			SetPedHeadOverlay(ped, 8, Character.OverlayData.Lipstick, Character.OverlayData.LipstickOpacity);
			SetPedHeadOverlayColor(ped, 8, 1, Character.OverlayData.LipstickColour, Character.OverlayData.LipstickColour);

			SetPedHeadOverlay(ped, 9, Character.OverlayData.MolesFreckles, Character.OverlayData.MolesFrecklesOpacity);

			SetPedHeadOverlay(ped, 10, Character.OverlayData.ChestHair, Character.OverlayData.ChestHairOpacity);
			SetPedHeadOverlayColor(ped, 10, 1, Character.OverlayData.ChestHairColour, Character.OverlayData.ChestHairColour);

			SetPedHeadOverlay(ped, 11, Character.OverlayData.BodyBlemish, Character.OverlayData.BodyBlemishOpacity);

			SetPedEyeColor(ped, Character.OverlayData.EyeColour);
			#endregion
			#region Components
			SetPedComponentVariation(ped, 0, Character.ComponentData.Torso, 0, 1); // Torso
			SetPedComponentVariation(ped, 3, Character.ComponentData.Pants, Character.ComponentData.PantsTexture, 1); // Pants
			SetPedComponentVariation(ped, 6, Character.ComponentData.Shoes, Character.ComponentData.ShoesTexture, 1); // Shoes
			SetPedComponentVariation(ped, 8, Character.ComponentData.Undershirt, Character.ComponentData.UndershirtTexture, 1); // Undershirt
			SetPedComponentVariation(ped, 11, Character.ComponentData.Shirt, Character.ComponentData.ShirtTexture, 1); // Shirt
			#endregion

			Game.PlayerPed.Position = World.GetNextPositionOnStreet(Character.Position, true);
			Game.PlayerPed.Heading = Character.Heading;
			Core.CharacterManagement.SaveHandler.AddMoney(Character.Money);

			await Delay(1000);

			DoScreenFadeIn(500);

			FreezeEntityPosition(ped, false);
			SetEntityCollision(ped, true, true);
			Game.Player.CanControlCharacter = true;

			SaveHandler.Character = Character;
			SaveHandler.IsSavingAllowed = true;

			await Delay(5000);

			SetPlayerInvincible(ped, false);
		}
	}
}
