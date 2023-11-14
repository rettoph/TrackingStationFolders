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
using UnityEngine.UI;

namespace TrackingStationFolders.Components
{
    public class TrackingStationFolder : MonoBehaviour
    {
        private static FieldInfo _imageInfo;
        private Sprite _folderIcon;
        private TrackingStationWidget _widget;
        private List<Vessel> _vessels;
        private List<TrackingStationFolderItem> _items;
        private GameObject _content;
        private VerticalLayoutGroup _verticalLayoutGroup;

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

            _verticalLayoutGroup = SpaceTracking.Instance.listContainer.gameObject.GetComponent<VerticalLayoutGroup>();
        }

        public void Terminate()
        {
            _widget.toggle.onValueChanged.RemoveListener(this.HandleToggle);
            _widget.Terminate();
        }

        public void Start()
        {
            var image = _imageInfo.GetValue(_widget.iconSprite) as Image;
            image.sprite = _folderIcon;

            _widget.textName.text = this.FolderName;
            _widget.textInfo.text = "Info";
            _widget.textStatus.text = "Status";
        }

        public void AddVessel(Vessel vessel)
        {
            _vessels.Add(vessel);

            var item = this.CreateTrackingStationFolderItemWidget(vessel);
            _items.Add(item);
        }

        private TrackingStationFolderItem CreateTrackingStationFolderItemWidget(Vessel vessel)
        {
            ref var vesselWidgets = ref SpaceTracking.Instance.GetVesselWidgets();

            TrackingStationWidget widget = GameObject.Instantiate(SpaceTracking.Instance.listItemPrefab).Setup(vessel, null);
            TrackingStationFolderItem item = widget.gameObject.AddComponent<TrackingStationFolderItem>();
            item.SetActive(_open);
            vesselWidgets.Add(widget);
            //item.transform.SetSiblingIndex(this.transform.GetSiblingIndex() + _items.Count + 1);

            return item;
        }

        private void Close()
        {
            foreach (TrackingStationFolderItem item in _items)
            {
                item.SetActive(false);
            }
        }

        private void HandleToggle(bool arg0)
        {
            if(arg0 == false)
            {
                return;
            }

            _open = !_open;
            SpaceTracking.Instance.SetVessel(null, true);

            if (_open)
            {
                foreach(TrackingStationFolderItem item in _items)
                {
                    item.SetActive(true);
                }

                SpaceTracking.Instance.RefreshLayout();

                foreach (TrackingStationFolderItem item in _items)
                {
                    item.Indent();
                }
            }
            else
            {
                this.Close();
            }
        }
    }
}
