using HarmonyLib;
using KSP.UI.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackingStationFolders.Components;
using TrackingStationFolders.UI.Screens;

namespace TrackingStationFolders.Patches
{
    [HarmonyPatch(nameof(TrackingStationWidget), "Update")]
    public static class TrackingStationWidget_Update
    {
        private static bool Prefix(TrackingStationWidget __instance)
        {
            if(__instance.gameObject.TryGetComponent<TrackingStationFolder>(out TrackingStationFolder folder))
            {
                return false;
            }

            return true;
        }
    }
}
