using HarmonyLib;
using KSP.UI;
using KSP.UI.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace TrackingStationFolders
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    public class Startup : MonoBehaviour
    {
        public void Awake()
        {
            Harmony harmony = new Harmony("TrackingStationFolders");
            harmony.PatchAll();
        }
    }
}
