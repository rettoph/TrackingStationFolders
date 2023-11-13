using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TrackingStationFolders
{
    [KSPAddon(KSPAddon.Startup.TrackingStation, false)]
    public class TrackingStationFolders : MonoBehaviour
    {
        public TrackingStationFolders()
        {

        }

        public void Awake()
        {
            Console.WriteLine("Test123");
            var test = UnityEngine.Object.FindObjectOfType<GameObject>();
        }
    }
}
