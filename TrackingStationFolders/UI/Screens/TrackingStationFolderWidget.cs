using KSP.UI.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackingStationFolders.Helpers;

namespace TrackingStationFolders.UI.Screens
{
    public class TrackingStationFolderWidget : TrackingStationWidget
    {
        public TrackingStationFolderWidget()
        {
            ObjectHelper.CopyFields(SpaceTracking.Instance.listItemPrefab, this);
        }
    }
}
