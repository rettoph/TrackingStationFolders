using KSP.UI.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using TrackingStationFolders.Components;
using TrackingStationFolders.Extensions;
using UnityEngine;

namespace TrackingStationFolders
{
    [KSPAddon(KSPAddon.Startup.TrackingStation, false)]
    public class TrackingStationFolders : MonoBehaviour
    {
        private Dictionary<string, TrackingStationFolder> _folders;
        private Dictionary<string, List<Vessel>> _vessels;
        private TrackingStationFolder _openSoon;
        private int _openSoonDelay;

        public static TrackingStationFolders Instance;

        public IEnumerable<string> Folders => _folders.Keys;

        private void Awake()
        {
            _folders = new Dictionary<string, TrackingStationFolder>();

            TrackingStationFolders.Instance = this;
        }

        public void ConstructUIList(string defaultOpen = null)
        {
            this.ClearUIList();

            ref var vesselWidgets = ref SpaceTracking.Instance.GetVesselWidgets();
            _vessels = SpaceTracking.Instance.GetTrackedVessles().GroupBy(x => x.GetTrackingStationFolderName())
                .OrderBy(x => x.Key)
                .ToDictionary(x => x.Key, x => x.ToList());

            foreach(KeyValuePair<string, List<Vessel>> folderData in _vessels.Where(x => x.Key.IsNullOrEmpty() == false))
            {
                var folder = this.CreateTrackingStationFolderWidget(folderData.Key);
                _folders.Add(folderData.Key, folder);
                vesselWidgets.Add(folder.GetComponent<TrackingStationWidget>());

                folder.AddVessels(folderData.Value);
            }

            if(_vessels.TryGetValue(string.Empty, out List<Vessel> folderlessVessels))
            {
                foreach(Vessel vessel in folderlessVessels)
                {
                    if(!MapViewFiltering.CheckAgainstFilter(vessel))
                    {
                        continue;
                    }

                    TrackingStationWidget widget = UnityEngine.Object.Instantiate(SpaceTracking.Instance.listItemPrefab).Setup(vessel);

                    vesselWidgets.Add(widget);
                }
            }

            if(defaultOpen != null && _folders.TryGetValue(defaultOpen, out TrackingStationFolder defaultOpenFolder))
            {
                defaultOpenFolder.Open();
            }
            else 
            {
                this.RefreshLayout();
            }
        }

        private void ClearUIList()
        {
            ref var vesselWidgets = ref SpaceTracking.Instance.GetVesselWidgets();
            int count = vesselWidgets.Count;
            while (count-- > 0)
            {
                vesselWidgets[count].Terminate();
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

        public void RefreshLayout()
        {
            SpaceTracking.Instance.RefreshLayout();

            foreach (TrackingStationFolder folder in _folders.Values)
            {
                folder.Indent();
            }
        }

        public void Update()
        {
            if(_openSoon != null)
            {
                if(_openSoonDelay++ >= 5)
                {
                    _openSoon.Open();
                    _openSoon = null;
                }
            }
        }

        internal void OpenSoon(TrackingStationFolder trackingStationFolder)
        {
            _openSoon = trackingStationFolder;
            _openSoonDelay = 0;
        }
    }
}
