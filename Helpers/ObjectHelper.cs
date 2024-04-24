using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TrackingStationFolders.Helpers
{
    public static class ObjectHelper
    {
        public static void CopyFields(object objectA, object objectB)
        {
            BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            Dictionary<string, FieldInfo> objectAFields = objectA.GetType().GetFields(flags).ToDictionary(x => x.Name);
            Dictionary<string, FieldInfo> objectBFields = objectB.GetType().GetFields(flags).ToDictionary(x => x.Name);

            foreach(FieldInfo objectAField in objectAFields.Values)
            {
                if(objectBFields.TryGetValue(objectAField.Name, out FieldInfo objectBField) && objectBField.FieldType.IsAssignableFrom(objectAField.FieldType))
                {
                    object objectAFieldValue = objectAField.GetValue(objectA);
                    objectBField.SetValue(objectB, objectAFieldValue);
                }
            }
        }
    }
}
