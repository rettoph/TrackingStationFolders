using ClickThroughFix;
using KSP.UI.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using ToolbarControl_NS;
using TrackingStationFolders.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TrackingStationFolders.Components
{
    [KSPAddon(KSPAddon.Startup.TrackingStation, false)]
    public class SetTrackingStationFolderGUI : MonoBehaviour
    {
        public static SetTrackingStationFolderGUI Instance;
        public static string ModId = "Rettoph_TrackingStationFolders";
        public static string ModName = "TrackingStationFolders";
        public static string Lock = nameof(SetTrackingStationFolderGUI) + "_LOCK";
        private const int _windowId = 150374;
        private string _folder;

        private Vessel _vessel;
        private Rect _rect = new Rect((Screen.width - 250) / 2, (Screen.height - 480) / 2, 240, 1);
        private static ApplicationLauncherButton _launchButton;
        private Texture2D _launchButtonIcon;
        private bool _open;

        public void Awake()
        {
            GameEvents.onGUIApplicationLauncherReady.Add(OnGUIApplicationLauncherReady);
            GameEvents.OnMapFocusChange.Add(OnOnMapFocusChange);
            GameEvents.onHideUI.Add(OnHideUI);
            GameEvents.onShowUI.Add(OnShowUI);
            GameEvents.onSceneConfirmExit.Add(OnSceneConfirmExit);

            _launchButtonIcon = new Texture2D(512, 512);
            _launchButtonIcon.LoadImage(Icons.Folder);

            SetTrackingStationFolderGUI.Instance = this;
        }

        public void Destroy()
        {
            GameEvents.onGUIApplicationLauncherReady.Remove(OnGUIApplicationLauncherReady);
            GameEvents.OnMapFocusChange.Remove(OnOnMapFocusChange);
            GameEvents.onHideUI.Remove(OnHideUI);
            GameEvents.onShowUI.Remove(OnShowUI);
            GameEvents.onSceneConfirmExit.Remove(OnSceneConfirmExit);

            SetTrackingStationFolderGUI.Instance = null;
        }

        public void OnGUIApplicationLauncherReady()
        {
            if (ApplicationLauncher.Ready && HighLogic.LoadedScene == GameScenes.TRACKSTATION && _launchButton == null)
            {
                _launchButton = ApplicationLauncher.Instance.AddModApplication(
                    () => SetTrackingStationFolderGUI.Instance.ToggleWindow(),
                    () => SetTrackingStationFolderGUI.Instance.ToggleWindow(),
                    null,
                    null,
                    null,
                    null,
                    ApplicationLauncher.AppScenes.TRACKSTATION, _launchButtonIcon);

                _launchButton.Disable();
            }
        }

        public void OnGUI()
        {
            if(_open == false)
            {
                return;
            }

            _rect = ClickThruBlocker.GUILayoutWindow(_windowId, _rect, this.DrawGUI, "Tracking Station Folders", HighLogic.Skin.window);
        }

        private void Close(bool trySave)
        {
            if(trySave && _folder != SpaceTracking.Instance.SelectedVessel.GetTrackingStationFolderName())
            {
                SpaceTracking.Instance.SelectedVessel.SetTrackingStationFolderName(_folder);
                TrackingStationFolders.Instance.ConstructUIList(_folder);
            }

            _open = false;
        }

        private void ToggleWindow()
        {
            if(_open == true)
            {
                this.Close(true);
                return;
            }

            if(SpaceTracking.Instance.SelectedVessel == null)
            {
                return;
            }

            _open = true;
            _folder = SpaceTracking.Instance.SelectedVessel.GetTrackingStationFolderName();
        }

        private void DrawGUI(int id)
        {
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();

            GUILayout.Label(SpaceTracking.Instance.SelectedVessel?.GetDisplayName() ?? "");

            GUILayout.BeginHorizontal();
            GUILayout.Label("Folder:", GUILayout.Width(50));
            _folder = GUILayout.TextField(_folder, GUILayout.Width(200));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Save"))
            {
                this.Close(true);
            }

            if (GUILayout.Button("Cancel"))
            {
                this.Close(false);
            }
            GUILayout.EndHorizontal();

            if(TrackingStationFolders.Instance.Folders.Any())
            {
                GUIStyle horizontalLine;
                horizontalLine = new GUIStyle();
                horizontalLine.normal.background = Texture2D.whiteTexture;
                horizontalLine.margin = new RectOffset(0, 0, 4, 4);
                horizontalLine.fixedHeight = 1;
                GUILayout.Box(GUIContent.none, horizontalLine);

                bool folderPresetButtonClicked = false;
                foreach (string folder in TrackingStationFolders.Instance.Folders)
                {
                    if (GUILayout.Button(folder))
                    {
                        _folder = folder;
                        folderPresetButtonClicked |= true;
                    }
                }

                if (folderPresetButtonClicked)
                {
                    this.Close(true);
                }
            }

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUI.DragWindow();
        }

        private void OnOnMapFocusChange(MapObject data)
        {
            if(_launchButton is null)
            {
                return;
            }

            if (data?.vessel is null)
            {
                _launchButton.Disable();
            }
            else
            {
                _launchButton.Enable();
            }
        }

        private void OnShowUI()
        {
            _open = false;
        }

        private void OnHideUI()
        {
            _open = false;
        }

        private void OnSceneConfirmExit(GameScenes data)
        {
            _open = false;
        }
    }
}
