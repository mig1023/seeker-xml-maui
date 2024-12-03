using System.Collections.Generic;

namespace SeekerMAUI.Gamebook.BloodfeudOfAltheus
{
    class Constants : Prototypes.Constants, Abstract.IConstants
    {
        public static Dictionary<int, string> HealthLine { get; set; }

        public static Dictionary<string, string> Availabilities { get; set; }
    }
}
