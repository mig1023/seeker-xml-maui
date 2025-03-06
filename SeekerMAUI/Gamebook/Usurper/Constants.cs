using System.Collections.Generic;

namespace SeekerMAUI.Gamebook.Usurper
{
    class Constants : Prototypes.Constants, Abstract.IConstants
    {
        public static List<string> Random { get; set; }

        public static Dictionary<string, string> Availabilities { get; set; }
    }
}
