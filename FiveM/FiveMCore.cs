using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using FiveM.Core.Managers;
using FiveM.Core.Ped_Handlers;
using static CitizenFX.Core.Native.API;

namespace FiveM
{
    class FiveMCore : BaseScript
    {
        //List<Vector3> spawns = new List<Vector3>()
        //{
        //    { new Vector3(2533.0f, 2833.0f, 38.0f) },
        //    { new Vector3(2606.0f, 2927.0f, 40.0f) },
        //    { new Vector3(2463.0f, 3872.0f, 38.8f) },
        //    { new Vector3(1164.0f, 6433.0f, 32.0f) },
        //    { new Vector3(537.0f, -1324.1f, 29.1f) },
        //    { new Vector3(219.1f, -2487.7f, 6.0f) }
        //};

        List<string> models = new List<string>() { /*"freight", "freightcar", "freightgrain", "freightcont1", "freightcont2", "freighttrailer", "tankercar", "metrotrain", "s_m_m_lsmetro_01", "u_m_y_juggernaut_01",*/ /*"ig_vincent",*/ "s_m_y_blackops_01", "mp_f_freemode_01", "mp_m_freemode_01" };

        public FiveMCore()
        {
            EventHandlers["onClientResourceStart"] += new Action<string>(OnClientResourceStart);
        }

        private async void OnClientResourceStart(string resourceName)
        {
			if (resourceName != GetCurrentResourceName())
				return;

			if (Game.IsLoading || IsNetworkLoadingScene())
				await Delay(500);

			RegisterCustomCommands();

            foreach (var model in models)
            {
				var modelHash = (uint)GetHashKey(model);

				Debug.WriteLine($"Loading model: {model} ({modelHash})");

				while (!HasModelLoaded(modelHash))
				{
					RequestModel(modelHash);
					await Delay(10);
				}

				Debug.WriteLine($"Model has been loaded: {model} ({modelHash})");
			}

			TriggerServerEvent("DSCP5M:RequestPlayerData");
		}

        public void RegisterCustomCommands()
        {
            RegisterCommand("car", new Action<int, List<object>, string>(async (source, args, raw) =>
            {
                // account for the argument not being passed
                var model = "adder";
                if (args.Count > 0)
                {
                    model = args[0].ToString();
                }

                // check if the model actually exists
                // assumes the directive `using static CitizenFX.Core.Native.API;`
                var hash = (uint)GetHashKey(model);
                if (!IsModelInCdimage(hash) || !IsModelAVehicle(hash))
                {
                    TriggerEvent("chat:addMessage", new
                    {
                        color = new[] { 255, 0, 0 },
                        args = new[] { "[CarSpawner]", $"It might have been a good thing that you tried to spawn a {model}. Who even wants their spawning to actually ^*succeed?" }
                    });
                    return;
                }

                // create the vehicle
                var vehicle = await World.CreateVehicle(model, Game.PlayerPed.Position, Game.PlayerPed.Heading);

                // set the player ped into the vehicle and driver seat
                Game.PlayerPed.SetIntoVehicle(vehicle, VehicleSeat.Driver);

                // tell the player
                TriggerEvent("chat:addMessage", new
                {
                    color = new[] { 255, 0, 0 },
                    args = new[] { "[CarSpawner]", $"Woohoo! Enjoy your new ^*{model}!" }
                });
            }), false);

            RegisterCommand("money", new Action<int, List<object>, string>(async (source, args, raw) =>
            {
				Core.CharacterManagement.SaveHandler.AddMoney(50000);
            }), false);

			RegisterCommand("addnot", new Action<int, List<object>, string>(async (source, args, raw) =>
			{
				HunterManager.IncreaseNotoriety(50);
			}), false);

			RegisterCommand("chenot", new Action<int, List<object>, string>(async (source, args, raw) =>
			{
				TriggerEvent("chat:addMessage", new
				{
					color = new[] { 255, 0, 0 },
					args = new[] { "[CarSpawner]", $"{HunterManager.Notoriety}" }
				});
				return;
			}), false);


			RegisterCommand("orb", new Action<int, List<object>, string>(async (source, args, raw) =>
            {
                Vector3 pos = Game.PlayerPed.Position;

                foreach(var ped in World.GetAllPeds())
                {
                    if (ped.IsPlayer)
                        continue;

                    if(World.GetDistance(pos, ped.Position) < 150)
                    {
                        AddExplosion(ped.Position.X, ped.Position.Y, ped.Position.Z, (int)ExplosionType.Rocket, 2500f, true, false, 1.5f);
                        ped.Kill();
                    }
                }

            }), false);
        }
    }
}
