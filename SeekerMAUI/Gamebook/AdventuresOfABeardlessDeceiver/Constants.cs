using System.Collections.Generic;

namespace SeekerMAUI.Gamebook.AdventuresOfABeardlessDeceiver
{
    class Constants : Prototypes.Constants, Abstract.IConstants
    {
        public static Dictionary<string, string> StatNames { get; set; }

        public static Dictionary<string, string> Availabilities { get; set; }
    }
}
