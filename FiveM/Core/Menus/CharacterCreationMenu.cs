using CitizenFX.Core;
using MenuAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using static CitizenFX.Core.Native.API;

namespace FiveM.Core.Menus
{
	class CharacterCreationMenu : BaseScript
	{
		#region Components
		public static List<string> Opacity = new List<string>() { "0%", "10%", "20%", "30%", "40%", "50%", "60%", "70%", "80%", "90%", "100%" };
		public static List<string> Hair, MaleHair, FemaleHair, OverlayColours, Blemish, BeardList, EyebrowsList, AgeingList, MakeupList, BlushList, ComplexionList, SundamageList, LipstickList, MolesFrecklesList, ChestHairList, BodyBlemishList, EyeColours, Faces;
		#endregion


		public static int InitialFatherFace, InitialMotherFace, InitialHairStyle, InitialHairColour, InitialEyeColour;
		public Gender gender;

		Random random = new Random();
		public static List<float> mixValues = new List<float>() { 0.0f, 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1.0f };
		List<float> faceFeaturesValuesList = new List<float>()
			{
			   -1.0f,    // 0
        	   -0.9f,    // 1
        	   -0.8f,    // 2
        	   -0.7f,    // 3
        	   -0.6f,    // 4
        	   -0.5f,    // 5
        	   -0.4f,    // 6
        	   -0.3f,    // 7
        	   -0.2f,    // 8
        	   -0.1f,    // 9
        		0.0f,    // 10
        		0.1f,    // 11
        		0.2f,    // 12
        		0.3f,    // 13
        		0.4f,    // 14
        		0.5f,    // 15
        		0.6f,    // 16
        		0.7f,    // 17
        		0.8f,    // 18
        		0.9f,    // 19
        		1.0f     // 20
        	};
		string[] faceFeaturesNamesList = new string[20]
		{
				"Nose Width",               // 0
        		"Noes Peak Height",         // 1
        		"Nose Peak Length",         // 2
        		"Nose Bone Height",         // 3
        		"Nose Peak Lowering",       // 4
        		"Nose Bone Twist",          // 5
        		"Eyebrows Height",          // 6
        		"Eyebrows Depth",           // 7
        		"Cheekbones Height",        // 8
        		"Cheekbones Width",         // 9
        		"Cheeks Width",             // 10
        		"Eyes Opening",             // 11
        		"Lips Thickness",           // 12
        		"Jaw Bone Width",           // 13
        		"Jaw Bone Depth/Length",    // 14
        		"Chin Height",              // 15
        		"Chin Depth/Length",        // 16
        		"Chin Width",               // 17
        		"Chin Hole Size",           // 18
        		"Neck Thickness"            // 19
        };

		Menu CCMenu;
		public static Camera MainCamera, FaceCam, BodyCam;
		public static Vector3 BodyPos = new Vector3(402.85f, -997.85f, -98.5f), FullBodyLookAt = new Vector3(402.85f, -996.5f, -98.5f), FacePos = new Vector3(402.85f, -997f, -98.35f), FaceLookAt = new Vector3(402.85f, -996.5f, -98.35f);

		public Menu GetCCMenu()
		{
			CCMenu = new Menu("Character Creation", "Create your character");

			MenuListItem GenderItem = new MenuListItem("Gender", new List<string> { "Male", "Female" }, 0);
			MenuItem SaveAndCreate = new MenuItem("Save and Create", "Saves your character, and spawns you in");

			CCMenu.AddMenuItem(GenderItem);

			GenerateComponentData();
			GenerateSubMenus();

			CCMenu.AddMenuItem(SaveAndCreate);

			CCMenu.OnMenuOpen += async (menu) => MenuController.DisableBackButton = true;
			CCMenu.OnMenuClose += async (menu) => MenuController.DisableBackButton = false;
			CCMenu.OnListIndexChange += async (menu, listItem, oldIndex, newIndex, itemIndex) =>
			{
				if (listItem == GenderItem)
				{
					if (listItem.ListIndex == 0)
					{
						DoScreenFadeOut(250);
						await Delay(250);

						gender = Gender.Male;

						Ped_Handlers.PedSpawner.CreatePed(Game.Player, InitialFatherFace = random.Next(0, 46), InitialMotherFace = random.Next(0, 46), InitialHairStyle = random.Next(0, 38),
							InitialHairColour = random.Next(0, GetNumHairColors() - 4), InitialEyeColour = random.Next(0, 31), "mp_m_freemode_01");

						CCMenu.ClearMenuItems();
						CCMenu.AddMenuItem(GenderItem);
						GenerateSubMenus();
						CCMenu.AddMenuItem(SaveAndCreate);

						await Delay(250);

						CCMenu.RefreshIndex();
						DoScreenFadeIn(500);
					}
					else if (listItem.ListIndex == 1)
					{
						DoScreenFadeOut(250);
						await Delay(250);

						gender = Gender.Female;

						Ped_Handlers.PedSpawner.CreatePed(Game.Player, InitialFatherFace = random.Next(0, 46), InitialMotherFace = random.Next(0, 46), InitialHairStyle = random.Next(0, 38),
							InitialHairColour = random.Next(0, GetNumHairColors() - 4), InitialEyeColour = random.Next(0, 31), "mp_f_freemode_01");

						CCMenu.ClearMenuItems();
						CCMenu.AddMenuItem(GenderItem);
						GenerateSubMenus();
						CCMenu.AddMenuItem(SaveAndCreate);

						await Delay(250);

						CCMenu.RefreshIndex();
						DoScreenFadeIn(500);
					}
				}
			};
			CCMenu.OnItemSelect += async (menu, listItem, index) =>
			{
				if (listItem == SaveAndCreate)
				{
					if (GenderItem.ListIndex == 0 && GetPedDrawableVariation(Game.PlayerPed.Handle, 2) == 23)
					{
						SetNotificationTextEntry("CELL_EMAIL_BCON"); // 10x ~a~
						AddTextComponentSubstringPlayerName($"~r~You are unable to create a character with that hair style");
						DrawNotification(false, false);
					}
					else if (GenderItem.ListIndex == 1 && GetPedDrawableVariation(Game.PlayerPed.Handle, 2) == 24)
					{
						SetNotificationTextEntry("CELL_EMAIL_BCON"); // 10x ~a~
						AddTextComponentSubstringPlayerName($"~r~You are unable to create a character with that hair style");
						DrawNotification(false, false);
					}
					else CreatePedName();
				}
			};
			return CCMenu;
		}

		public static void GenerateComponentData()
		{
			MaleHair = new List<string>();
			foreach (int number in Enumerable.Range(0, 36)) MaleHair.Add($"Style #{number + 1}");

			FemaleHair = new List<string>();
			foreach (int number in Enumerable.Range(0, 38)) FemaleHair.Add($"Style #{number + 1}");

			OverlayColours = new List<string>();
			foreach (int number in Enumerable.Range(0, GetNumHairColors())) OverlayColours.Add($"Colour #{number + 1}");

			Blemish = new List<string>();
			foreach (int number in Enumerable.Range(0, GetNumHeadOverlayValues(0))) Blemish.Add($"Style #{number + 1}");

			BeardList = new List<string>();
			foreach (int number in Enumerable.Range(0, GetNumHeadOverlayValues(1))) BeardList.Add($"Style #{number + 1}");

			EyebrowsList = new List<string>();
			foreach (int number in Enumerable.Range(0, GetNumHeadOverlayValues(2))) EyebrowsList.Add($"Style #{number + 1}");

			AgeingList = new List<string>();
			foreach (int number in Enumerable.Range(0, GetNumHeadOverlayValues(3))) AgeingList.Add($"Style #{number + 1}");

			MakeupList = new List<string>();
			foreach (int number in Enumerable.Range(0, GetNumHeadOverlayValues(4))) MakeupList.Add($"Style #{number + 1}");

			BlushList = new List<string>();
			foreach (int number in Enumerable.Range(0, GetNumHeadOverlayValues(5))) BlushList.Add($"Style #{number + 1}");

			ComplexionList = new List<string>();
			foreach (int number in Enumerable.Range(0, GetNumHeadOverlayValues(6))) ComplexionList.Add($"Style #{number + 1}");

			SundamageList = new List<string>();
			foreach (int number in Enumerable.Range(0, GetNumHeadOverlayValues(7))) SundamageList.Add($"Style #{number + 1}");

			LipstickList = new List<string>();
			foreach (int number in Enumerable.Range(0, GetNumHeadOverlayValues(8))) LipstickList.Add($"Style #{number + 1}");

			MolesFrecklesList = new List<string>();
			foreach (int number in Enumerable.Range(0, GetNumHeadOverlayValues(9))) MolesFrecklesList.Add($"Style #{number + 1}");

			ChestHairList = new List<string>();
			foreach (int number in Enumerable.Range(0, GetNumHeadOverlayValues(10))) ChestHairList.Add($"Style #{number + 1}");

			BodyBlemishList = new List<string>();
			foreach (int number in Enumerable.Range(0, GetNumHeadOverlayValues(11))) BodyBlemishList.Add($"Style #{number + 1}");

			EyeColours = new List<string>();
			foreach (int number in Enumerable.Range(0, 31)) EyeColours.Add($"Colour #{number + 1}");

			Faces = new List<string>();
			foreach (int number in Enumerable.Range(0, 45)) Faces.Add($"#{number + 1}");
		}

		private void GenerateSubMenus()
		{
			#region Inhetirance menu
			Menu InheritanceMenu = new Menu("Inheritance", "Inheritance options");
			MenuItem InheritanceMenuItem = new MenuItem("Inheritance", "Change your characters inheritance");
			InheritanceMenu.ClearMenuItems();

			MenuListItem Father = new MenuListItem("Father", Faces, InitialFatherFace);
			MenuListItem Mother = new MenuListItem("Mother", Faces, InitialMotherFace);
			MenuSliderItem ShapeMix = new MenuSliderItem("Head Shape Mix", "Select how much of your head shape should be inherited from your father or mother.", 0, 10, 5, true)
			{ SliderLeftIcon = MenuItem.Icon.MALE, SliderRightIcon = MenuItem.Icon.FEMALE };
			MenuSliderItem SkinMix = new MenuSliderItem("Body Skin Mix", "Select how much of your body skin tone should be inherited from your father or mother.", 0, 10, 5, true)
			{ SliderLeftIcon = MenuItem.Icon.MALE, SliderRightIcon = MenuItem.Icon.FEMALE };

			InheritanceMenu.AddMenuItem(Father);
			InheritanceMenu.AddMenuItem(Mother);
			InheritanceMenu.AddMenuItem(ShapeMix);
			InheritanceMenu.AddMenuItem(SkinMix);

			CCMenu.AddMenuItem(InheritanceMenuItem);
			MenuController.BindMenuItem(CCMenu, InheritanceMenu, InheritanceMenuItem);

			InheritanceMenu.OnListIndexChange += (menu, listItem, oldIndex, newIndex, itemIndex) =>
			{
				SetPedHeadBlendData(GetPlayerPed(-1), Father.ListIndex, Mother.ListIndex, 0, Father.ListIndex, Mother.ListIndex, 0, mixValues[ShapeMix.Position], mixValues[SkinMix.Position], 0f, false);
			};
			InheritanceMenu.OnSliderPositionChange += (menu, listItem, oldIndex, newIndex, itemIndex) =>
			{
				SetPedHeadBlendData(GetPlayerPed(-1), Father.ListIndex, Mother.ListIndex, 0, Father.ListIndex, Mother.ListIndex, 0, mixValues[ShapeMix.Position], mixValues[SkinMix.Position], 0f, false);
			};
			InheritanceMenu.OnMenuOpen += (menu) => SetFaceCam();
			InheritanceMenu.OnMenuClose += (menu) => SetBodyCam();
			#endregion

			#region Face Shape
			Menu FaceShape = new Menu("Face shape", "Face shape options");
			MenuItem FaceShapeItem = new MenuItem("Face shape", "Tweak the details of your characters face");
			FaceShape.ClearMenuItems();

			for (int i = 0; i < 20; i++)
			{
				MenuSliderItem faceFeature = new MenuSliderItem(faceFeaturesNamesList[i], $"Set the {faceFeaturesNamesList[i]} face feature value.", 0, 20, 10, true);
				FaceShape.AddMenuItem(faceFeature);
			}

			FaceShape.OnSliderPositionChange += async (sender, sliderItem, oldPosition, newPosition, itemIndex) =>
			{
				float value = faceFeaturesValuesList[newPosition];
				SetPedFaceFeature(Game.PlayerPed.Handle, itemIndex, value);
			};

			FaceShape.OnMenuOpen += (menu) => SetFaceCam();
			FaceShape.OnMenuClose += (menu) => SetBodyCam();


			CCMenu.AddMenuItem(FaceShapeItem);
			MenuController.BindMenuItem(CCMenu, FaceShape, FaceShapeItem);

			#endregion

			#region Appearance
			Menu AppearanceMenu = new Menu("Appearance", "Appearance options");
			MenuItem AppearanceMenuItem = new MenuItem("Appearance", "Change your characters appearance");
			AppearanceMenu.ClearMenuItems();

			if (gender == Gender.Male) Hair = MaleHair;
			else Hair = FemaleHair;

			#region MenuList
			MenuListItem HairStyle = new MenuListItem("Hair", MaleHair, InitialHairStyle);
			MenuListItem HairColour = new MenuListItem("Hair Colour", OverlayColours, InitialHairColour) { ShowColorPanel = true, ColorPanelColorType = MenuListItem.ColorPanelType.Hair };
			MenuListItem HairHighlights = new MenuListItem("Hair Highlights", OverlayColours, InitialHairColour) { ShowColorPanel = true, ColorPanelColorType = MenuListItem.ColorPanelType.Hair };
			AppearanceMenu.AddMenuItem(HairStyle);
			AppearanceMenu.AddMenuItem(HairColour);
			AppearanceMenu.AddMenuItem(HairHighlights);

			MenuListItem BlemishStyle = new MenuListItem("Blemish", Blemish, random.Next(0, GetNumHeadOverlayValues(0) - 1));
			MenuListItem BlemishOpacity = new MenuListItem("Blemish Opacity", Opacity, 0) { ShowOpacityPanel = true };
			AppearanceMenu.AddMenuItem(BlemishStyle);
			AppearanceMenu.AddMenuItem(BlemishOpacity);


			MenuListItem BeardStyle = new MenuListItem("Beard", BeardList, 1);
			MenuListItem BeardOpacity = new MenuListItem("Beard Opacity", Opacity, 0) { ShowOpacityPanel = true };
			MenuListItem BeardColour = new MenuListItem("Beard Colour", OverlayColours, InitialHairColour) { ShowColorPanel = true, ColorPanelColorType = MenuListItem.ColorPanelType.Hair };
			if (gender == Gender.Male)
			{
				AppearanceMenu.AddMenuItem(BeardStyle);
				AppearanceMenu.AddMenuItem(BeardOpacity);
				AppearanceMenu.AddMenuItem(BeardColour);
			}

			MenuListItem EyebrowsStyle = new MenuListItem("Eyebrows", EyebrowsList, random.Next(0, EyebrowsList.Count()));
			MenuListItem EyebrowsOpacity = new MenuListItem("Eyebrow Opacity", Opacity, 8) { ShowOpacityPanel = true };
			MenuListItem EyebrowsColour = new MenuListItem("Eyebrow Colour", OverlayColours, 0) { ShowColorPanel = true, ColorPanelColorType = MenuListItem.ColorPanelType.Hair };
			AppearanceMenu.AddMenuItem(EyebrowsStyle);
			AppearanceMenu.AddMenuItem(EyebrowsOpacity);
			AppearanceMenu.AddMenuItem(EyebrowsColour);

			MenuListItem Ageing = new MenuListItem("Ageing", AgeingList, 4);
			MenuListItem AgeingOpacity = new MenuListItem("Ageing Opacity", Opacity, 0) { ShowOpacityPanel = true };
			AppearanceMenu.AddMenuItem(Ageing);
			AppearanceMenu.AddMenuItem(AgeingOpacity);

			MenuListItem MakeupStyle = new MenuListItem("Makeup", MakeupList, 4);
			MenuListItem MakeupOpacity = new MenuListItem("Makeup Opacity", Opacity, 8) { ShowOpacityPanel = true };
			MenuListItem MakeupColour = new MenuListItem("Makeup Colour", OverlayColours, 0) { ShowColorPanel = true, ColorPanelColorType = MenuListItem.ColorPanelType.Hair };
			AppearanceMenu.AddMenuItem(MakeupStyle);
			AppearanceMenu.AddMenuItem(MakeupOpacity);
			AppearanceMenu.AddMenuItem(MakeupColour);

			MenuListItem BlushStyle = new MenuListItem("Blush", BlushList, 4);
			MenuListItem BlushOpacity = new MenuListItem("Blush Opacity", Opacity, 8) { ShowOpacityPanel = true };
			MenuListItem BlushColour = new MenuListItem("Blush Colour", OverlayColours, 0) { ShowColorPanel = true, ColorPanelColorType = MenuListItem.ColorPanelType.Hair };
			AppearanceMenu.AddMenuItem(BlushStyle);
			AppearanceMenu.AddMenuItem(BlushOpacity);
			AppearanceMenu.AddMenuItem(BlushColour);

			MenuListItem ComplexionStyle = new MenuListItem("Complexion", ComplexionList, 4);
			MenuListItem ComplexionOpacity = new MenuListItem("Complexion Opacity", Opacity, 0) { ShowOpacityPanel = true };
			AppearanceMenu.AddMenuItem(ComplexionStyle);
			AppearanceMenu.AddMenuItem(ComplexionOpacity);

			MenuListItem SunDamageStyle = new MenuListItem("Sun Damage", SundamageList, 4);
			MenuListItem SunDamageOpacity = new MenuListItem("Sun Damage Opacity", Opacity, 0) { ShowOpacityPanel = true };
			AppearanceMenu.AddMenuItem(SunDamageStyle);
			AppearanceMenu.AddMenuItem(SunDamageOpacity);

			MenuListItem LipstickStyle = new MenuListItem("Lipstick", LipstickList, 4);
			MenuListItem LipstickOpacity = new MenuListItem("Lipstick Opacity", Opacity, 0) { ShowOpacityPanel = true };
			MenuListItem LipstickColour = new MenuListItem("Lipstick Colour", OverlayColours, 0) { ShowColorPanel = true, ColorPanelColorType = MenuListItem.ColorPanelType.Hair };
			AppearanceMenu.AddMenuItem(LipstickStyle);
			AppearanceMenu.AddMenuItem(LipstickOpacity);
			AppearanceMenu.AddMenuItem(LipstickColour);

			MenuListItem MolesFrecklesStyle = new MenuListItem("Moles and Freckles", MolesFrecklesList, 4);
			MenuListItem MolesFrecklesOpacity = new MenuListItem("Moles and Freckles Opacity", Opacity, 0) { ShowOpacityPanel = true };
			AppearanceMenu.AddMenuItem(MolesFrecklesStyle);
			AppearanceMenu.AddMenuItem(MolesFrecklesOpacity);


			MenuListItem ChestHairStyle = new MenuListItem("Chest Hair", ChestHairList, 1);
			MenuListItem ChestHairOpacity = new MenuListItem("Chest Hair Opacity", Opacity, 0) { ShowOpacityPanel = true };
			MenuListItem ChestHairColour = new MenuListItem("Chest Hair Colour", OverlayColours, 0) { ShowColorPanel = true, ColorPanelColorType = MenuListItem.ColorPanelType.Hair };
			if (gender == Gender.Male)
			{
				AppearanceMenu.AddMenuItem(ChestHairStyle);
				AppearanceMenu.AddMenuItem(ChestHairOpacity);
				AppearanceMenu.AddMenuItem(ChestHairColour);
			}

			MenuListItem BodyBlemishStyle = new MenuListItem("Body Blemish", BodyBlemishList, 4);
			MenuListItem BodyBlemishOpacity = new MenuListItem("Body Blemish Opacity", Opacity, 0) { ShowOpacityPanel = true };
			AppearanceMenu.AddMenuItem(BodyBlemishStyle);
			AppearanceMenu.AddMenuItem(BodyBlemishOpacity);

			MenuListItem EyeColour = new MenuListItem("Eye Colour", EyeColours, InitialEyeColour);
			AppearanceMenu.AddMenuItem(EyeColour);

			CCMenu.AddMenuItem(AppearanceMenuItem);
			MenuController.BindMenuItem(CCMenu, AppearanceMenu, AppearanceMenuItem);
			#endregion
			AppearanceMenu.OnListIndexChange += async (menu, listItem, oldIndex, newIndex, itemIndex) =>
			{
				switch (listItem.Text)
				{
					case "Hair":
						SetFaceCam();
						SetPedComponentVariation(Game.PlayerPed.Handle, 2, HairStyle.ListIndex, 0, 1);
						break;
					case "Hair Colour":
					case "Hair Highlights":
						SetFaceCam();
						SetPedHairColor(Game.PlayerPed.Handle, HairColour.ListIndex, HairHighlights.ListIndex);
						break;
					case "Blemish":
					case "Blemish Opacity":
						SetFaceCam();
						SetPedHeadOverlay(Game.PlayerPed.Handle, 0, BlemishStyle.ListIndex, (float)BlemishOpacity.ListIndex / 10);
						break;
					case "Beard":
					case "Beard Opacity":
					case "Beard Colour":
						SetFaceCam();
						SetPedHeadOverlay(Game.PlayerPed.Handle, 1, BeardStyle.ListIndex, (float)BeardOpacity.ListIndex / 10);
						SetPedHeadOverlayColor(Game.PlayerPed.Handle, 1, 1, BeardColour.ListIndex, BeardColour.ListIndex);
						break;
					case "Eyebrows":
					case "Eyebrow Opacity":
					case "Eyebrow Colour":
						SetFaceCam();
						SetPedHeadOverlay(Game.PlayerPed.Handle, 2, EyebrowsStyle.ListIndex, (float)EyebrowsOpacity.ListIndex / 10);
						SetPedHeadOverlayColor(Game.PlayerPed.Handle, 2, 1, EyebrowsColour.ListIndex, EyebrowsColour.ListIndex);
						break;
					case "Ageing":
					case "Ageing Opacity":
						SetFaceCam();
						SetPedHeadOverlay(Game.PlayerPed.Handle, 3, Ageing.ListIndex, (float)AgeingOpacity.ListIndex / 10);
						break;
					case "Makeup":
					case "Makeup Opacity":
					case "Makeup Colour":
						SetFaceCam();
						SetPedHeadOverlay(Game.PlayerPed.Handle, 4, MakeupStyle.ListIndex, (float)MakeupOpacity.ListIndex / 10);
						SetPedHeadOverlayColor(Game.PlayerPed.Handle, 4, 1, MakeupColour.ListIndex, MakeupColour.ListIndex);
						break;
					case "Blush":
					case "Blush Opacity":
					case "Blush Colour":
						SetFaceCam();
						SetPedHeadOverlay(Game.PlayerPed.Handle, 5, BlushStyle.ListIndex, (float)BlushOpacity.ListIndex / 10);
						SetPedHeadOverlayColor(Game.PlayerPed.Handle, 5, 1, BlushColour.ListIndex, BlushColour.ListIndex);
						break;
					case "Complexion":
					case "Complexion Opacity":
						SetFaceCam();
						SetPedHeadOverlay(Game.PlayerPed.Handle, 6, ComplexionStyle.ListIndex, (float)ComplexionOpacity.ListIndex / 10);
						break;
					case "Sun Damage":
					case "Sun Damage Opacity":
						SetFaceCam();
						SetPedHeadOverlay(Game.PlayerPed.Handle, 7, SunDamageStyle.ListIndex, (float)SunDamageOpacity.ListIndex / 10);
						break;
					case "Lipstick":
					case "Lipstick Opacity":
					case "Lipstick Colour":
						SetFaceCam();
						SetPedHeadOverlay(Game.PlayerPed.Handle, 8, LipstickStyle.ListIndex, (float)LipstickOpacity.ListIndex / 10);
						SetPedHeadOverlayColor(Game.PlayerPed.Handle, 8, 1, LipstickColour.ListIndex, LipstickColour.ListIndex);
						break;
					case "Moles and Freckles":
					case "Moles and Freckles Opacity":
						SetFaceCam();
						SetPedHeadOverlay(Game.PlayerPed.Handle, 9, MolesFrecklesStyle.ListIndex, (float)MolesFrecklesOpacity.ListIndex / 10);
						break;
					case "Chest Hair":
					case "Chest Hair Opacity":
					case "Chest Hair Colour":
						SetBodyCam();
						SetPedHeadOverlay(Game.PlayerPed.Handle, 10, ChestHairStyle.ListIndex, (float)ChestHairOpacity.ListIndex / 10);
						SetPedHeadOverlayColor(Game.PlayerPed.Handle, 10, 1, ChestHairColour.ListIndex, ChestHairColour.ListIndex);
						break;
					case "Body Blemish":
					case "Body Blemish Opacity":
						SetBodyCam();
						SetPedHeadOverlay(Game.PlayerPed.Handle, 11, BodyBlemishStyle.ListIndex, (float)BodyBlemishOpacity.ListIndex / 10);
						break;
					case "Eye Colour":
						SetFaceCam();
						SetPedEyeColor(Game.PlayerPed.Handle, EyeColour.ListIndex);
						break;
				}
			};
			AppearanceMenu.OnMenuClose += (menu) => SetBodyCam();
			#endregion

			return;
		}

		private void SetFaceCam()
		{
			if (MainCamera.Position == FacePos) return;
			int cam = CreateCam("DEFAULT_SCRIPTED_CAMERA", true);
			FaceCam = new Camera(cam);
			FaceCam.Position = FacePos;
			FaceCam.PointAt(FaceLookAt);

			MainCamera.InterpTo(FaceCam, 750, true, true);

			MainCamera = FaceCam;

			//BodyCam.InterpTo(FaceCam, 750, true, true);
		}
		private void SetBodyCam()
		{
			if (MainCamera.Position == BodyPos) return;

			int cam = CreateCam("DEFAULT_SCRIPTED_CAMERA", true);
			BodyCam = new Camera(cam);
			BodyCam.Position = BodyPos;
			BodyCam.PointAt(FullBodyLookAt);

			//FaceCam.InterpTo(BodyCam, 750, true, true);
			MainCamera.InterpTo(BodyCam, 750, true, true);

			MainCamera = BodyCam;
		}

		private async void ResetCamera()
		{
			await Delay(550);

			MenuController.CloseAllMenus();

			CCMenu.ClearMenuItems();

			DisplayRadar(true);
			DisplayHud(true);

			int ped = Game.PlayerPed.Handle;

			FreezeEntityPosition(ped, false);
			SetEntityCollision(ped, true, true);
			SetEntityInvincible(ped, false);
			Game.Player.CanControlCharacter = true;

			RenderScriptCams(false, false, 0, false, false);
			DestroyAllCams(true);

			await Delay(1000);

			DoScreenFadeIn(500);

			CharacterManagement.SaveHandler.IsSavingAllowed = true;

			return;
		}

		private async void CreatePedName()
		{
			//TODO: SAVE PED

			var spacer = "\t";
			AddTextEntry($"CHARACTERNAME_WINDOW_TITLE", $"Character Name (MAX 100 Characters)");

			// Display the input box.
			DisplayOnscreenKeyboard(1, $"CHARACTERNAME_WINDOW_TITLE", "", "Apples", "", "", "", 50);
			await Delay(0);
			// Wait for a result.
			while (true)
			{
				int keyboardStatus = UpdateOnscreenKeyboard();

				switch (keyboardStatus)
				{
					case 3: // not displaying input field anymore somehow
					case 2: // cancelled
						return;
					case 1: // finished editing
						FinishPedCreation(GetOnscreenKeyboardResult());
						return;
					default:
						await Delay(0);
						break;
				}
			}
		}

		#region Spawn points
		Tuple<float, float, float, float> StationOne = new Tuple<float, float, float, float>(428, -978.5f, 30, 90);
		Tuple<float, float, float, float> StationTwo = new Tuple<float, float, float, float>(341.5f, -1558f, 29, 320);
		Tuple<float, float, float, float> StationThree = new Tuple<float, float, float, float>(-1053, -816f, 19, 321);
		Tuple<float, float, float, float> StationFour = new Tuple<float, float, float, float>(-557.50f, -142, 38, 200);
		#endregion

		private async void FinishPedCreation(string Name)
		{
			DoScreenFadeOut(500);

			await Delay(500);

			Ped player = Game.PlayerPed;
			int ped = Game.PlayerPed.Handle;

			if (Game.PlayerPed.Model == "mp_m_freemode_01")
			{
				SetPedComponentVariation(ped, 3, 0, 0, 1); // Arms
				SetPedComponentVariation(ped, 4, 13, 1, 1); // Pants
				SetPedComponentVariation(ped, 5, 9, 0, 1); // Bags
				SetPedComponentVariation(ped, 6, 4, 0, 1); // Feet
				SetPedComponentVariation(ped, 8, 2, 0, 1); // Top
				SetPedComponentVariation(ped, 11, 0, 0, 1); // Top

			}
			else if (Game.PlayerPed.Model == "mp_f_freemode_01")
			{
				SetPedComponentVariation(ped, 3, 14, 0, 1); // Arms
				SetPedComponentVariation(ped, 4, 45, 1, 1); // Pants
				SetPedComponentVariation(ped, 5, 9, 0, 1); // Bags
				SetPedComponentVariation(ped, 6, 33, 1, 1); // Feet
				SetPedComponentVariation(ped, 8, 3, 0, 1); // Top
				SetPedComponentVariation(ped, 11, 73, 0, 1); // Top
			}

			await Delay(50);

			int SelectSpawn = random.Next(0, 5);

			if (SelectSpawn == 1) { SetEntityCoords(ped, StationOne.Item1, StationOne.Item2, StationOne.Item3, true, false, false, true); player.Heading = StationOne.Item4; }
			else if (SelectSpawn == 2) { SetEntityCoords(ped, StationTwo.Item1, StationTwo.Item2, StationTwo.Item3, true, false, false, true); player.Heading = StationTwo.Item4; }
			else if (SelectSpawn == 3) { SetEntityCoords(ped, StationThree.Item1, StationThree.Item2, StationThree.Item3, true, false, false, true); player.Heading = StationThree.Item4; }
			else { SetEntityCoords(ped, StationFour.Item1, StationFour.Item2, StationFour.Item3, true, false, false, true); player.Heading = StationFour.Item4; }

			CharacterManagement.SaveHandler.Character = new Misc.Character();

			CharacterManagement.SaveHandler.Character.Name = Name;
			CharacterManagement.SaveHandler.Character.Position = Game.PlayerPed.Position;

			CharacterManagement.SaveHandler.Character.Gender = Game.PlayerPed.Model == "mp_m_freemode_01" ? CitizenFX.Core.Gender.Male : CitizenFX.Core.Gender.Female;
			Core.CharacterManagement.SaveHandler.AddMoney(500);
			CharacterManagement.SaveHandler.Character.Money = Core.CharacterManagement.SaveHandler.Money;
			CharacterManagement.SaveHandler.Character.CharacterID = $"{BitConverter.ToString(BitConverter.GetBytes(DateTime.Now.Ticks)).Replace("-", string.Empty)}{BitConverter.ToString(BitConverter.GetBytes(new Random().Next(0, 50))).Replace("-", string.Empty)}";

			ResetCamera();

			MenuController.Menus.Remove(CCMenu);

			CharacterManagement.SaveHandler.ForceSave = true;
		}
	}
}
