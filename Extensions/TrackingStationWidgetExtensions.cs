using HarmonyLib;
using KSP.UI.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace TrackingStationFolders.Extensions
{
    public static class TrackingStationWidgetExtensions
    {
        public static TrackingStationWidget Setup(this TrackingStationWidget widget, Vessel vessel = null, Callback<Vessel> onClick = null, Transform parent = null, bool worldPositionStays = false)
        {
            vessel = vessel ?? new Vessel();
            onClick = onClick ?? SpaceTracking.Instance.GetRequestVesselCallback();
            parent = parent ?? SpaceTracking.Instance.listContainer;

            widget.transform.SetParent(parent, worldPositionStays: worldPositionStays);
            widget.SetVessel(vessel, onClick, SpaceTracking.Instance.listToggleGroup);

            return widget;
        }
    }
}
