using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace FiveM
{
    class WeaponSystem : BaseScript
    {
        public WeaponSystem()
        {
            //Tick += WeaponSystemTick;
        }

        private async Task WeaponSystemTick()
        {
            try
            {
                foreach (WeaponHash weapon in Enum.GetValues(typeof(WeaponHash)))
                {
                    if (!Game.PlayerPed.Weapons.HasWeapon(weapon))
                        Game.PlayerPed.Weapons.Give(weapon, 5000, false, true);
                }

                await Delay(5000);
            }
            catch (Exception ex) { Console.WriteLine($"{ex}"); }
        }
    }
}
