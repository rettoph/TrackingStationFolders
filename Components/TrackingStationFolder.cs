using KSP.UI.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TrackingStationFolders.Extensions;
using TrackingStationFolders.Modules;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TrackingStationFolders.Components
{
    public class TrackingStationFolder : MonoBehaviour
    {
        public static TrackingStationFolder CurrentOpenFolder;

        private static FieldInfo _imageInfo;
        private Sprite _folderIcon;
        private TrackingStationWidget _widget;
        private List<Vessel> _vessels;
        private List<TrackingStationFolderItem> _items;
        private static bool _ignoreToggle;
        private bool _hovered;
        private int _totalVesselsCount;

        private bool _open;

        public string FolderName;

        static TrackingStationFolder()
        {
            _imageInfo = typeof(VesselIconSprite).GetField("image", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        public void Awake()
        {
            _vessels = new List<Vessel>();
            _items = new List<TrackingStationFolderItem>();
            _widget = this.gameObject.GetComponent<TrackingStationWidget>();

            _widget.transform.SetParent(SpaceTracking.Instance.listContainer, worldPositionStays: false);
            _widget.SetVessel(new Vessel(), null, SpaceTracking.Instance.listToggleGroup);
            _widget.toggle.onValueChanged.AddListener(this.HandleToggle);

            var folderTexture = new Texture2D(512, 512);
            folderTexture.LoadImage(Icons.Folder);
            _folderIcon = Sprite.Create(folderTexture, new Rect(0, 0, 512, 512), Vector2.zero);

            this.UpdateStatusAndInfoText();

            EventTrigger eventTrigger = GetComponent<EventTrigger>();
            if (eventTrigger == null)
            {
                eventTrigger = base.gameObject.AddComponent<EventTrigger>();
                eventTrigger.triggers = new List<EventTrigger.Entry>();
            }
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerEnter;
            entry.callback.AddListener(OnMouseEnter);
            EventTrigger.Entry entry2 = new EventTrigger.Entry();
            entry2.eventID = EventTriggerType.PointerExit;
            entry2.callback.AddListener(OnMouseExit);
            eventTrigger.triggers.Add(entry);
            eventTrigger.triggers.Add(entry2);
        }

        public void OnDestroy()
        {
            _widget.toggle.onValueChanged.RemoveListener(this.HandleToggle);

            if(CurrentOpenFolder == this)
            {
                CurrentOpenFolder = null;
            }
        }

        public void Start()
        {
            var image = _imageInfo.GetValue(_widget.iconSprite) as Image;
            image.sprite = _folderIcon;

            _widget.textName.text = this.FolderName;

        }

        public void AddVessels(List<Vessel> vessels)
        {
            foreach(Vessel vessel in vessels)
            {
                _totalVesselsCount++;
                if (!MapViewFiltering.CheckAgainstFilter(vessel))
                {
                    continue;
                }

                _vessels.Add(vessel);

                var item = this.CreateTrackingStationFolderItemWidget(vessel);
            }

            this.UpdateStatusAndInfoText();
        }

        private TrackingStationFolderItem CreateTrackingStationFolderItemWidget(Vessel vessel)
        {
            ref var vesselWidgets = ref SpaceTracking.Instance.GetVesselWidgets();

            TrackingStationWidget widget = GameObject.Instantiate(SpaceTracking.Instance.listItemPrefab).Setup(vessel, null);
            TrackingStationFolderItem item = widget.gameObject.AddComponent<TrackingStationFolderItem>();
            item.SetActive(_open);
            vesselWidgets.Add(widget);

            _items.Add(item);
            this.UpdateStatusAndInfoText();

            return item;
        }

        public void Open()
        {
            if (CurrentOpenFolder != null)
            {
                CurrentOpenFolder.Close();
                TrackingStationFolders.Instance.RefreshLayout();
                CurrentOpenFolder = null;
                TrackingStationFolders.Instance.OpenSoon(this);

                return;
            }

            foreach (TrackingStationFolderItem item in _items)
            {
                item.SetActive(true);
            }

            _open = true;
            CurrentOpenFolder = this;

            TrackingStationFolders.Instance.RefreshLayout();
            this.UpdateStatusAndInfoText();
        }

        private void Close()
        {
            if (CurrentOpenFolder != this)
            {
                return;
            }

            foreach (TrackingStationFolderItem item in _items)
            {
                item.SetActive(false);
            }

            _open = false;
            CurrentOpenFolder = null;

            TrackingStationFolders.Instance.RefreshLayout();
            this.UpdateStatusAndInfoText();
        }

        public void RefreshItems()
        {
            if (_open == false)
            {
                return;
            }

            foreach (TrackingStationFolderItem item in _items)
            {
                item.SetActive(false);
            }

            foreach (TrackingStationFolderItem item in _items)
            {
                item.SetActive(true);
            }
        }
        public void Indent()
        {
            if(_open == false)
            {
                return;
            }

            foreach (TrackingStationFolderItem item in _items)
            {
                item.Indent();
            }
        }

        private void HandleToggle(bool arg0)
        {
            if(arg0 == false || _ignoreToggle == true || _hovered == false)
            {
                return;
            }

            _ignoreToggle = true;
            

            if (_open)
            {
                this.Close();
            }
            else
            {
                this.Open();
                
            }

            _ignoreToggle = false;
        }

        private void UpdateStatusAndInfoText()
        {
            _widget.textStatus.text = _open ? "Open" : "Closed";

            if(_items.Count == _totalVesselsCount)
            {
                _widget.textInfo.text = "Vessels: " + _totalVesselsCount.ToString("#,##0");
            }
            else
            {
                _widget.textInfo.text = "Vessels: " + _items.Count.ToString("#,##0") + "/" + _totalVesselsCount.ToString("#,##0");
            }
        }

        public void OnMouseEnter(BaseEventData data)
        {
            _hovered = true;
        }

        public void OnMouseExit(BaseEventData data)
        {
            _hovered = false;
        }
    }
}
