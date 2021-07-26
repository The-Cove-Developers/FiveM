using CitizenFX.Core;
using MenuAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using static CitizenFX.Core.Native.API;
using FiveM.Core.Ped_Handlers;
using FiveM.Core.Menus;
using CCM = FiveM.Core.Menus.CharacterCreationMenu;
using System.Threading.Tasks;

namespace FiveM.Core.Misc
{
	public class PedOverlays
	{
		public int Blemish { get; set; }
		public float BlemishOpacity { get; set; }
		public int FacialHair { get; set; }
		public int FacialHairColour { get; set; }
		public float FacialHairOpacity { get; set; }
		public int Eyebrows { get; set; }
		public int EyebrowsColour { get; set; }
		public float EyebrowsOpacity { get; set; }
		public int Ageing { get; set; }
		public float AgeingOpacity { get; set; }
		public int Makeup { get; set; }
		public int MakeupColour { get; set; }
		public float MakeupOpacity { get; set; }
		public int Blush { get; set; }
		public int BlushColour { get; set; }
		public float BlushOpacity { get; set; }
		public int Complexion { get; set; }
		public float ComplexionOpacity { get; set; }
		public int Sundamage { get; set; }
		public float SundamageOpacity { get; set; }
		public int Lipstick { get; set; }
		public int LipstickColour { get; set; }
		public float LipstickOpacity { get; set; }
		public int MolesFreckles { get; set; }
		public float MolesFrecklesOpacity { get; set; }
		public int ChestHair { get; set; }
		public int ChestHairColour { get; set; }
		public float ChestHairOpacity { get; set; }
		public int BodyBlemish { get; set; }
		public float BodyBlemishOpacity { get; set; }
		public int EyeColour { get; set; }
	}
	public class Face
	{
		public int FatherFace { get; set; }
		public int MotherFace { get; set; }
		public int FatherSkin { get; set; }
		public int MotherSkin { get; set; }
		public float ShapePercent { get; set; }
		public float SkinPercent { get; set; }
		public float NoseWidth { get; set; }
		public float NoesPeakHeight { get; set; }
		public float NosePeakLength { get; set; }
		public float NoseBoneHeight { get; set; }
		public float NosePeakLowering { get; set; }
		public float NoseBoneTwist { get; set; }
		public float EyebrowsHeight { get; set; }
		public float EyebrowsDepth { get; set; }
		public float CheekbonesHeight { get; set; }
		public float CheekbonesWidth { get; set; }
		public float CheeksWidth { get; set; }
		public float EyesOpening { get; set; }
		public float LipsThickness { get; set; }
		public float JawBoneWidth { get; set; }
		public float JawBoneDepthLength { get; set; }
		public float ChinHeight { get; set; }
		public float ChinDepthLength { get; set; }
		public float ChinWidth { get; set; }
		public float ChinHoleSize { get; set; }
		public float NeckThickness { get; set; }
	}
	public class Components
	{
		public int Torso { get; set; }
		public int TorsoTexture { get; set; }
		public int Pants { get; set; }
		public int PantsTexture { get; set; }
		public int Shoes { get; set; }
		public int ShoesTexture { get; set; }
		public int Undershirt { get; set; }
		public int UndershirtTexture { get; set; }
		public int Shirt { get; set; }
		public int ShirtTexture { get; set; }
		public int Hair { get; set; }
		public int HairColour { get; set; }
		public int HairHighlight { get; set; }
	}

	public class Character
	{
		public string Name { get; set; }
		public Vector3 Position { get; set; }
		public float Heading { get; set; }
		public Gender Gender { get; set; }
		public int Money { get; set; }
		public string CharacterID { get; set; }

		public PedOverlays OverlayData { get; set; }
		public Face FaceData { get; set; }
		public Components ComponentData { get; set; }

		public List<Vehicle> Vehicle = new List<Vehicle>();
	}

	public class Vehicle
	{
		public VehicleHash VehicleHash { get; set; }
		public VehicleColor VehicleColor { get; set; }
	}
}
