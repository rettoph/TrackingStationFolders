using KSP.UI.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TrackingStationFolders.Components;
using TrackingStationFolders.Extensions;
using TrackingStationFolders.Helpers;
using TrackingStationFolders.Modules;
using TrackingStationFolders.UI.Screens;
using UniLinq;
using UnityEngine;

namespace TrackingStationFolders
{
    [KSPAddon(KSPAddon.Startup.TrackingStation, false)]
    public class TrackingStationFolders : MonoBehaviour
    {
        private Dictionary<string, TrackingStationFolder> _folders;

        public static TrackingStationFolders Instance;

        private void Awake()
        {
            _folders = new Dictionary<string, TrackingStationFolder>();

            TrackingStationFolders.Instance = this;
        }

        public void ConstructUIList()
        {
            this.ClearUIList();

            ref var vesselWidgets = ref SpaceTracking.Instance.GetVesselWidgets();

            foreach (Vessel vessel in SpaceTracking.Instance.GetTrackedVessles())
            {
                string folderName = vessel.GetTrackingStationFolderName();

                if(folderName.IsNullOrEmpty())
                {
                    TrackingStationWidget widget = UnityEngine.Object.Instantiate(SpaceTracking.Instance.listItemPrefab).Setup(vessel);

                    vesselWidgets.Add(widget);

                    continue;
                }
                
                if(_folders.TryGetValue(folderName, out TrackingStationFolder folder) == false)
                {
                    folder = this.CreateTrackingStationFolderWidget(folderName);
                    _folders.Add(folderName, folder);
                }

                folder.AddVessel(vessel);
            }

            SpaceTracking.Instance.RefreshLayout();
        }

        private void ClearUIList()
        {
            ref var vesselWidgets = ref SpaceTracking.Instance.GetVesselWidgets();
            int count = vesselWidgets.Count;
            while (count-- > 0)
            {
                vesselWidgets[count].Terminate();
            }

            foreach(TrackingStationFolder folder in _folders.Values)
            {
                folder.Terminate();
            }

            vesselWidgets.Clear();
            _folders.Clear();
        }

        private TrackingStationFolder CreateTrackingStationFolderWidget(string folderName)
        {
            TrackingStationWidget widget = GameObject.Instantiate(SpaceTracking.Instance.listItemPrefab);
            TrackingStationFolder folder = widget.gameObject.AddComponent<TrackingStationFolder>();

            folder.FolderName = folderName;

            return folder;
        }

        private static string GetVesselFolderName(Vessel vessel)
        {
            return vessel.FindVesselModuleImplementing<TrackingStationFolderName>().Value;
        }

        private void HandleFolderOnClick(Vessel arg1)
        {
            throw new NotImplementedException();
        }
    }
}
