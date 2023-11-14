using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingStationFolders.Modules
{
    public class TrackingStationFolderName : VesselModule
    {
        [KSPField(isPersistant = true)]
        public string Value;
    }
}
