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
using CitizenFX.Core.UI;
using FiveM.Core.Misc;

namespace FiveM.Core.CharacterManagement
{
	class SaveHandler : BaseScript
	{
		private DateTime _lastSave = new DateTime();

		public static bool IsSavingAllowed = false, ForceSave = false;

		public static Character Character;

		public static int Money { get; private set; }

		public static void AddMoney(int Value) => Money += Value;
		public static void RemoveMoney(int Value) => Money = Misc.Extensions.Clamp(Money - Value, 0, int.MaxValue);

		public SaveHandler()
		{
			_lastSave = DateTime.Now;

			Tick += SaveHandler_Tick;
		}

		private async Task SaveHandler_Tick()
		{
			try
			{
				if (!IsSavingAllowed || Character == null || ((DateTime.Now - _lastSave).TotalSeconds < 90 && !ForceSave))
				{
					//Debug.WriteLine($"{IsSavingAllowed} {Character == null} {(DateTime.Now - LastSave).Seconds} {ForceSave}");
					return;
				}

				ShowLoadingPrompt((int)LoadingSpinnerType.SocialClubSaving);

				Debug.WriteLine("Saving data!");

				Character.Position = Game.PlayerPed.Position;
				Character.Money = Core.CharacterManagement.SaveHandler.Money;

				GetOverlayData();
				GetFaceData();
				GetComponentData();

				var json = JsonConvert.SerializeObject(Character);

				TriggerServerEvent("DSCP5M:SavePlayerData", JsonConvert.SerializeObject(Character), Character.CharacterID);

				_lastSave = DateTime.Now;
				ForceSave = false;

			}
			catch (Exception e)
			{
				Debug.WriteLine(e.ToString());
			}

			_lastSave = DateTime.Now;

			RemoveLoadingPrompt();
		}

		private void GetOverlayData()
		{
			int junk = 0;
			Character.OverlayData = new Misc.PedOverlays();


			int blemish = 0; float blemishopacity = 0;
			GetPedHeadOverlayData(Game.PlayerPed.Handle, 0, ref blemish, ref junk, ref junk, ref junk, ref blemishopacity);
			Character.OverlayData.Blemish = blemish; Character.OverlayData.BlemishOpacity = blemishopacity;

			int facial = 0; int facialcolour = 0; float facialopacity = 0;
			GetPedHeadOverlayData(Game.PlayerPed.Handle, 1, ref facial, ref junk, ref facialcolour, ref junk, ref facialopacity);
			Character.OverlayData.FacialHair = facial; Character.OverlayData.FacialHairColour = facialcolour; Character.OverlayData.FacialHairOpacity = facialopacity;

			int eyebrow = 0; int eyebrowcolour = 0; float eyebrowopacity = 0;
			GetPedHeadOverlayData(Game.PlayerPed.Handle, 2, ref eyebrow, ref junk, ref eyebrowcolour, ref junk, ref eyebrowopacity);
			Character.OverlayData.Eyebrows = eyebrow; Character.OverlayData.EyebrowsColour = eyebrowcolour; Character.OverlayData.EyebrowsOpacity = eyebrowopacity;

			int ageing = 0; float ageingopacity = 0;
			GetPedHeadOverlayData(Game.PlayerPed.Handle, 3, ref ageing, ref junk, ref junk, ref junk, ref ageingopacity);
			Character.OverlayData.Ageing = ageing; Character.OverlayData.AgeingOpacity = ageingopacity;

			int makeup = 0; int makeupcolour = 0; float makeupopacity = 0;
			GetPedHeadOverlayData(Game.PlayerPed.Handle, 4, ref makeup, ref junk, ref makeupcolour, ref junk, ref makeupopacity);
			Character.OverlayData.Makeup = makeup; Character.OverlayData.MakeupColour = makeupcolour; Character.OverlayData.MakeupOpacity = makeupopacity;

			int blush = 0; int blushcolour = 0; float blushopacity = 0;
			GetPedHeadOverlayData(Game.PlayerPed.Handle, 5, ref blush, ref junk, ref blushcolour, ref junk, ref blushopacity);
			Character.OverlayData.Blush = blush; Character.OverlayData.BlushColour = blushcolour; Character.OverlayData.BlushOpacity = blushopacity;

			int complexion = 0; float complexionopacity = 0;
			GetPedHeadOverlayData(Game.PlayerPed.Handle, 6, ref complexion, ref junk, ref junk, ref junk, ref complexionopacity);
			Character.OverlayData.Complexion = complexion; Character.OverlayData.ComplexionOpacity = complexionopacity;

			int sundamage = 0; float sundamageopacity = 0;
			GetPedHeadOverlayData(Game.PlayerPed.Handle, 7, ref sundamage, ref junk, ref junk, ref junk, ref sundamageopacity);
			Character.OverlayData.Sundamage = sundamage; Character.OverlayData.SundamageOpacity = sundamageopacity;

			int lipstick = 0; int lipstickcolour = 0; float lipstickopacity = 0;
			GetPedHeadOverlayData(Game.PlayerPed.Handle, 8, ref lipstick, ref junk, ref lipstickcolour, ref junk, ref lipstickopacity);
			Character.OverlayData.Lipstick = lipstick; Character.OverlayData.LipstickColour = lipstickcolour; Character.OverlayData.LipstickOpacity = lipstickopacity;

			int molesfreckles = 0; float molesfrecklesopacity = 0;
			GetPedHeadOverlayData(Game.PlayerPed.Handle, 9, ref molesfreckles, ref junk, ref junk, ref junk, ref molesfrecklesopacity);
			Character.OverlayData.MolesFreckles = molesfreckles; Character.OverlayData.MolesFrecklesOpacity = molesfrecklesopacity;

			int chesthair = 0; int chesthaircolour = 0; float chesthairopacity = 0;
			GetPedHeadOverlayData(Game.PlayerPed.Handle, 10, ref chesthair, ref junk, ref chesthaircolour, ref junk, ref chesthairopacity);
			Character.OverlayData.ChestHair = chesthair; Character.OverlayData.ChestHairOpacity = chesthairopacity;

			int bodyblemish = 0; float bodyblemishopacity = 0;
			GetPedHeadOverlayData(Game.PlayerPed.Handle, 11, ref bodyblemish, ref junk, ref junk, ref junk, ref bodyblemishopacity);
			Character.OverlayData.BodyBlemish = bodyblemish; Character.OverlayData.BodyBlemishOpacity = bodyblemishopacity;

			int eyecolour = GetPedEyeColor(Game.PlayerPed.Handle);
			Character.OverlayData.EyeColour = eyecolour;
		}
		private void GetFaceData()
		{
			int ped = Game.PlayerPed.Handle;

			Character.FaceData = new Misc.Face();

			PedHeadBlendData a = Game.PlayerPed.GetHeadBlendData();
			Character.FaceData.FatherFace = a.FirstFaceShape;
			Character.FaceData.MotherFace = a.SecondFaceShape;
			Character.FaceData.FatherSkin = a.FirstSkinTone;
			Character.FaceData.MotherSkin = a.SecondSkinTone;
			Character.FaceData.ShapePercent = a.ParentFaceShapePercent;
			Character.FaceData.SkinPercent = a.ParentSkinTonePercent;

			Character.FaceData.NoseWidth = GetPedFaceFeature(ped, 0);
			Character.FaceData.NoesPeakHeight = GetPedFaceFeature(ped, 1);
			Character.FaceData.NosePeakLength = GetPedFaceFeature(ped, 2);
			Character.FaceData.NoseBoneHeight = GetPedFaceFeature(ped, 3);
			Character.FaceData.NosePeakLowering = GetPedFaceFeature(ped, 4);
			Character.FaceData.NoseBoneTwist = GetPedFaceFeature(ped, 5);
			Character.FaceData.EyebrowsHeight = GetPedFaceFeature(ped, 6);
			Character.FaceData.EyebrowsDepth = GetPedFaceFeature(ped, 7);
			Character.FaceData.CheekbonesHeight = GetPedFaceFeature(ped, 8);
			Character.FaceData.CheekbonesWidth = GetPedFaceFeature(ped, 9);
			Character.FaceData.CheeksWidth = GetPedFaceFeature(ped, 10);
			Character.FaceData.EyesOpening = GetPedFaceFeature(ped, 11);
			Character.FaceData.LipsThickness = GetPedFaceFeature(ped, 12);
			Character.FaceData.JawBoneWidth = GetPedFaceFeature(ped, 13);
			Character.FaceData.JawBoneDepthLength = GetPedFaceFeature(ped, 14);
			Character.FaceData.ChinHeight = GetPedFaceFeature(ped, 15);
			Character.FaceData.ChinDepthLength = GetPedFaceFeature(ped, 16);
			Character.FaceData.ChinWidth = GetPedFaceFeature(ped, 17);
			Character.FaceData.ChinHoleSize = GetPedFaceFeature(ped, 18);
			Character.FaceData.NeckThickness = GetPedFaceFeature(ped, 19);
		}
		private void GetComponentData()
		{
			int ped = Game.PlayerPed.Handle;

			Character.ComponentData = new Misc.Components();

			Character.ComponentData.Torso = GetPedDrawableVariation(ped, 0);
			Character.ComponentData.TorsoTexture = GetPedTextureVariation(ped, 0);
			Character.ComponentData.Pants = GetPedDrawableVariation(ped, 3);
			Character.ComponentData.PantsTexture = GetPedTextureVariation(ped, 0);
			Character.ComponentData.Shoes = GetPedDrawableVariation(ped, 6);
			Character.ComponentData.ShoesTexture = GetPedTextureVariation(ped, 0);
			Character.ComponentData.Undershirt = GetPedDrawableVariation(ped, 8);
			Character.ComponentData.UndershirtTexture = GetPedTextureVariation(ped, 0);
			Character.ComponentData.Shirt = GetPedDrawableVariation(ped, 11);
			Character.ComponentData.ShirtTexture = GetPedTextureVariation(ped, 0);
			Character.ComponentData.Hair = GetPedDrawableVariation(ped, 2);
			Character.ComponentData.HairColour = GetPedHairColor(ped);
			Character.ComponentData.HairHighlight = GetPedHairHighlightColor(ped);
		}
	}
}
