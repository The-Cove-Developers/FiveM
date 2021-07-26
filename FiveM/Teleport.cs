using System;
using System.Collections.Generic;
using MenuAPI;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using static CitizenFX.Core.UI.Screen;
using static CitizenFX.Core.Native.API;
using System.Threading.Tasks;

namespace FiveM
{
    class Teleport : BaseScript
    {
        private List<TeleportData> Teleports = new List<TeleportData>();
        private bool AreTeleportsMade;

        private class TeleportData
        {
            public Vector3 TriggerPoint;
            public Vector3 SendPoint;
            public float Range;
            public TeleportData(Vector3 triggerpoint, Vector3 sendpoint, float range)
            {
                TriggerPoint = triggerpoint;
                SendPoint = sendpoint;
                Range = range;
            }
        }

        public Teleport()
        {
            Tick += TeleportTickCheck;

        }
        private async Task TeleportTickCheck() { await Delay(0); TeleportCheck(); }

        public void TeleportCheck()
        {
            if (!AreTeleportsMade) { LoadTeleportData(); return; }

            foreach (TeleportData data in Teleports)
                if (World.GetDistance(Game.PlayerPed.Position, data.TriggerPoint) < data.Range) Game.PlayerPed.Position = data.SendPoint;
        }

        private void LoadTeleportData()
        {
            AreTeleportsMade = false;

            //Humane
            Teleports.Add(new TeleportData(new Vector3(3540.75f, 3675.86f, 28f), new Vector3(3540.2f, 3671.6f, 20f), 1.5f));
            Teleports.Add(new TeleportData(new Vector3(3540.75f, 3675.86f, 21f), new Vector3(3540.2f, 3671.6f, 27f), 1.5f));

            //Facility
            Teleports.Add(new TeleportData(new Vector3(-2051.5f, 3237.1f, 31f), new Vector3(2150f, 2921f, -63f), 1.5f));
            Teleports.Add(new TeleportData(new Vector3(2155.12f, 2920.95f, -63f), new Vector3(-2055.5f, 3239.2f, 31f), 1.5f));
            Teleports.Add(new TeleportData(new Vector3(2033.73f, 2942.2f, -63f), new Vector3(2158f, 2921.05f, -82.1f), 1.5f));
            Teleports.Add(new TeleportData(new Vector3(2155f, 2921.05f, -82.1f), new Vector3(2037f, 2941.7f, -63f), 1.5f));

            AreTeleportsMade = true;
        }
    }
}
