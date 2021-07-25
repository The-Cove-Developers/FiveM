using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace FiveMCommonUtils
{
    public class UtilsCore
    {
		public class BlipMarkerData
		{
			public Vector3
			public float X;
			public float Y;
			public float Z;
			public int? SpriteID;
			public string Name;
			public int? Blip;
			public BlipMarkerData(float x, float y, float z, int? spriteid, string name)
			{
				X = x;
				Y = y;
				Z = z;
				SpriteID = spriteid;
				Name = name;
				Blip = null;
			}
		}
	}
}
