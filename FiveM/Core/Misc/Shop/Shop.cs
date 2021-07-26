using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace FiveM.Core.Misc.Shop
{
	public enum ShopType
	{
		Barber = 0
	}

	public class Shop : BaseScript
	{
		public Shop(Vector3 location, int spriteID, string name, ShopType shopType, float distance = 2f, float scaleX = 1.5f, float scaleY = 1.5f, float scaleZ = 2.5f)
		{
			Location = location;
			SpriteID = spriteID;
			Name = name;
			Distance = distance;
			ScaleX = scaleX;
			ScaleY = scaleY;
			ScaleZ = scaleZ;
			ShopType = shopType;
		}

		public Vector3 Location { get; set; }
		public int SpriteID { get; set; }
		public Blip Blip { get; set; }
		public string Name { get; set; }

		public float Distance { get; set; }

		public float ScaleX { get; set; }
		public float ScaleY { get; set; }
		public float ScaleZ { get; set; }

		public ShopType ShopType { get; set; }
	}
}
