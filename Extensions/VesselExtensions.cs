using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackingStationFolders.Modules;

namespace TrackingStationFolders.Extensions
{
    public static class VesselExtensions
    {
        public static string GetTrackingStationFolderName(this Vessel vessel)
        {
            try
            {
                return vessel.FindVesselModuleImplementing<TrackingStationFolderName>().Value;
            }
            catch(Exception e)
            {
                return string.Empty;
            }
        }

        public static void SetTrackingStationFolderName(this Vessel vessel, string name)
        {
            vessel.FindVesselModuleImplementing<TrackingStationFolderName>().Value = name;
        }
    }
}
