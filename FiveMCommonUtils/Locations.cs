using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace FiveMCommonUtils
{
	public class BlipMarkerData
	{
		public BlipMarkerData(Vector3 location, int spriteId, string name, Marker marker = null)
		{
			Location = location;
			SpriteId = spriteId;
			BlipName = name;
			Marker = marker;
		}

		public Vector3 Location;
		public Marker Marker = null;

		public int SpriteId;
		public string BlipName;
	}

	public class Marker
	{
		public int Type = 1;
		public Vector3 Direction = Vector3.Zero;
		public Vector3 Rotation = Vector3.Zero;
		public Vector3 Scale = new Vector3(3, 3, 1);
		public int Red = 250;
		public int Green = 232;
		public int Blue = 117;
		public int Alpha = 255;
		public bool MoveUpDown = false;
		public bool FaceCamera = true;
		public readonly int p19 = 2;
		public bool Rotate = false;
		public string TextureDict = null;
		public string TextureName = null;
		public readonly bool DrawnOnEnts = false;
	}
}
