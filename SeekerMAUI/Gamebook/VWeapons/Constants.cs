using System.Collections.Generic;

namespace SeekerMAUI.Gamebook.VWeapons
{
    class Constants : Prototypes.Constants, Abstract.IConstants
    {
        public static Dictionary<string, string> HealingParts { get; set; }

        public static Dictionary<string, string> Availabilities { get; set; }
    }
}
