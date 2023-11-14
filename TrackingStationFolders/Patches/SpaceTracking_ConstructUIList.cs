using HarmonyLib;
using KSP.UI.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackingStationFolders.Extensions;
using TrackingStationFolders.Modules;
using TrackingStationFolders.UI.Screens;

namespace TrackingStationFolders.Patches
{
    [HarmonyPatch(nameof(SpaceTracking), "ConstructUIList")]
    public static class SpaceTracking_ConstructUIList
    {
        private static bool Prefix(SpaceTracking __instance)
        {
            TrackingStationFolders.Instance.ConstructUIList();

            return false;
        }

        private static void HandleListener(bool arg0)
        {
            //throw new NotImplementedException();
        }
    }
}
