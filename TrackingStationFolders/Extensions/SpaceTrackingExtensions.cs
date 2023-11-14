using HarmonyLib;
using KSP.UI.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace TrackingStationFolders.Extensions
{
    public static class SpaceTrackingExtensions
    {
        private static AccessTools.FieldRef<SpaceTracking, List<Vessel>> trackedVessels =
            AccessTools.FieldRefAccess<SpaceTracking, List<Vessel>>(nameof(trackedVessels));

        private static AccessTools.FieldRef<SpaceTracking, List<TrackingStationWidget>> vesselWidgets =
            AccessTools.FieldRefAccess<SpaceTracking, List<TrackingStationWidget>>(nameof(vesselWidgets));

        private static MethodInfo RequestVessel = typeof(SpaceTracking).GetMethod(nameof(RequestVessel), BindingFlags.Instance | BindingFlags.NonPublic);

        public static ref List<Vessel> GetTrackedVessles(this SpaceTracking spaceTracking)
        {
            return ref trackedVessels(spaceTracking);
        }

        public static ref List<TrackingStationWidget> GetVesselWidgets(this SpaceTracking spaceTracking)
        {
            return ref vesselWidgets(spaceTracking);
        }

        public static Callback<Vessel> GetRequestVesselCallback(this SpaceTracking spaceTracking)
        {
            return v => RequestVessel.Invoke(spaceTracking, new[] { v });
        }

        public static void RefreshLayout(this SpaceTracking spaceTracking)
        {
            var verticalLayoutGroup = spaceTracking.listContainer.GetComponent<VerticalLayoutGroup>();

            verticalLayoutGroup.CalculateLayoutInputHorizontal();
            verticalLayoutGroup.CalculateLayoutInputVertical();
            verticalLayoutGroup.SetLayoutHorizontal();
            verticalLayoutGroup.SetLayoutVertical();
        }
    }
}
