using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;

namespace FiveM.Core.Ped_Handlers
{
	class HunterManager : BaseScript
	{
		Vehicle Hunter1 = null;
		Vehicle Hunter2 = null;

		public static int Notoriety = 0;
		private bool _notorietyTrigger = false;
		private DateTime LastUpdate = new DateTime(), LastCheck = new DateTime();

		string[] models = { "valkyrie2", "caracara" };

		bool areHuntersActive = false;

		int group;
		uint pedGroup;

		public HunterManager()
		{
			RequestModel((uint)GetHashKey("s_m_y_blackops_01"));
			foreach (string model in models)
				RequestModel((uint)GetHashKey(model));

			Tick += HunterTick;
		}

		private async Task HunterTick()
		{
			if (Notoriety < 15)
				_notorietyTrigger = false;

			TickLogic();

			if(Game.Player.WantedLevel < 1 && Notoriety > 15 && (DateTime.Now - LastCheck).Minutes > 1.5)
			{
				int Check = new System.Random().Next(20, 100);

				if(Check < Notoriety)
				{
					Game.Player.WantedLevel = 5;

					BeginTextCommandDisplayHelp("THREESTRINGS");
					AddTextComponentSubstringPlayerName($"Merryweather have found you, and have alerted the police to your location!");
					EndTextCommandDisplayHelp(0, false, false, 8000);
				}
				else
				{
					LastCheck = DateTime.Now;
				}
			}

			HandleNotoriety();
		}


		private async void TickLogic()
		{
			if (Game.Player.WantedLevel < 5)
			{
				if (areHuntersActive)
					areHuntersActive = false;

				if (Hunter1 != null)
				{
					foreach (Ped ped in Hunter1.Occupants)
						ped.Delete();

					Hunter1.Delete();
				}

				if (Hunter2 != null)
				{
					foreach (Ped ped in Hunter2.Occupants)
						ped.Delete();

					Hunter2.Delete();
				}

				Hunter1 = null;
				Hunter2 = null;

				return;
			}

			if (!areHuntersActive)
			{
				areHuntersActive = true;

				group = CreateGroup(8);
				pedGroup = (uint)GetPlayerGroup(PlayerPedId());
			}

			if (Hunter1 == null || !Hunter1.IsDriveable || Hunter1.Occupants.Length < 3 /*|| World.GetDistance(Hunter1.Position, Game.PlayerPed.Position) > 1000*/)
			{
				if (Hunter1 != null)
					foreach (var blip in Hunter1.AttachedBlips)
						blip.Delete();

				Hunter1 = await SpawnHunter(inAir: true);
			}

			if (Hunter2 == null || !Hunter2.IsDriveable || Hunter2.Occupants.Length < 3 /*|| World.GetDistance(Hunter2.Position, Game.PlayerPed.Position) > 1000*/)
			{
				if (Hunter2 != null)
					foreach (var blip in Hunter2.AttachedBlips)
						blip.Delete();


				Hunter2 = await SpawnHunter(inAir: true);
			}

			//List<Ped> deadPeds = new List<Ped>();

			//foreach(Ped ped in HunterPeds)
			//{
			//	if (!ped.IsInVehicle())
			//	{
			//		ped.AttachBlip();
			//		var blip = ped.AttachedBlips[0];

			//		SetBlipAsFriendly(blip.Handle, false);
			//		SetBlipSprite(blip.Handle, 84);
			//		SetBlipColour(blip.Handle, 75);
			//	}
			//	else if(ped.AttachedBlips.Length > 0 || ped.IsDead)
			//	{
			//		foreach (var blip in ped.AttachedBlips)
			//			blip.Delete();
			//	}

			//	if (ped.IsDead)
			//		deadPeds.Add(ped);
			//}

			//foreach (Ped ped in deadPeds)
			//	HunterPeds.Remove(ped);

			return;
		}

		private async void HandleNotoriety()
		{
			if(Game.Player.WantedLevel > 3 && (DateTime.Now - LastUpdate).TotalMinutes > 1)
			{
				int Increase = Game.Player.WantedLevel - 2;

				IncreaseNotoriety(Increase);

				LastUpdate = DateTime.Now;
			}
		}

		public static void IncreaseNotoriety(int Increase)
		{
			Debug.WriteLine($"Updating Notoriety: {Notoriety} -> {Notoriety + Increase}");

			Notoriety += Increase;
		}

		private bool Plus() => new System.Random().Next(0, 100) % 2 == 0;

		private async Task<Vehicle> SpawnHunter(string model = "valkyrie2", int people = 4, bool inAir = false)
		{
			System.Random rnd = new System.Random();

			Vector3 pos = Game.PlayerPed.Position;

			//Handle X
			if (Plus())
				pos.X += rnd.Next(250, 400);
			else
				pos.X -= rnd.Next(250, 400);

			//Handle Y
			if (Plus())
				pos.Y += rnd.Next(250, 400);
			else
				pos.Y -= rnd.Next(250, 400);

			if (!inAir)
			{
				pos.Z = World.GetGroundHeight(new Vector2(pos.X, pos.Y));

				pos = World.GetNextPositionOnStreet(pos, true);
			}
			else
				pos.Z += rnd.Next(50, 75);

			var vehicle = await World.CreateVehicle(new Model(GetHashKey(model)), pos, GetEntityHeading(Game.PlayerPed.Handle));

			vehicle.Speed = 15;
			vehicle.EnginePowerMultiplier = 4;

			//SetVehicleEngineOn(vehicle, true, true, true);
			//SetVehicleForwardSpeed(vehicle, 0); // Needed, so the heli doesn't fall down instantly
			//SetVehicleCheatPowerIncrease(vehicle, 2); // Make it easier to catch up

			for (int i = -1; i < people - 1; i++)
			{
				Ped ped = await vehicle.CreatePedOnSeat((VehicleSeat)i, new Model(GetHashKey("s_m_y_blackops_01")));

				int pedId = ped.Handle;

				SetBlockingOfNonTemporaryEvents(pedId, true);
				SetPedHearingRange(pedId, 9999);

				GiveWeaponToPed(pedId, (uint)WeaponHash.SpecialCarbine, 9999, true, true);
				SetPedAccuracy(pedId, 50);

				if (i < 1)
				{
					SetPedAsGroupLeader(pedId, group);

					SetPedMaxHealth(pedId, 800);
					SetEntityHealth(pedId, 800);
				}
				else
				{
					SetPedAsGroupMember(ped.Handle, group);
					SetPedMaxHealth(ped.Handle, 500);
					SetEntityHealth(pedId, 500);
				}

				SetPedAsCop(pedId, true);

				SetPedCombatAttributes(pedId, 0, true);
				SetPedCombatAttributes(pedId, 1, true);
				SetPedCombatAttributes(pedId, 2, true);
				SetPedCombatAttributes(pedId, 3, true);
				SetPedCombatAttributes(pedId, 5, true);
				SetPedCombatAttributes(pedId, 46, true);

				RegisterTarget(pedId, PlayerPedId());
				TaskCombatPed(pedId, PlayerPedId(), 0, 16);

			}



			vehicle.Health = vehicle.Health * 3;

			vehicle.AttachBlip();
			var blip = vehicle.AttachedBlips[0];

			SetBlipAsFriendly(blip.Handle, false);
			SetBlipSprite(blip.Handle, 84);
			SetBlipColour(blip.Handle, 75);

			BeginTextCommandSetBlipName("STRING");
			AddTextComponentString("Hunters");
			EndTextCommandSetBlipName(blip.Handle);

			SetRelationshipBetweenGroups((int)Relationship.Hate, (uint)group, pedGroup);
			SetRelationshipBetweenGroups((int)Relationship.Hate, pedGroup, (uint)group);
			SetRelationshipBetweenGroups((int)Relationship.Companion, (uint)group, (uint)group);

			SetRelationshipBetweenGroups((int)Relationship.Companion, (uint)group, (uint)group);

			SetRelationshipBetweenGroups((int)Relationship.Companion, (uint)group, (uint)GetHashKey("army"));
			SetRelationshipBetweenGroups((int)Relationship.Companion, (uint)GetHashKey("army"), (uint)group);

			return vehicle;
		}
	}
}
