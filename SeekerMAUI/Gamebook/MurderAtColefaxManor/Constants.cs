using System.Collections.Generic;

namespace SeekerMAUI.Gamebook.MurderAtColefaxManor
{
    class Constants : Prototypes.Constants, Abstract.IConstants
    {
        public static List<string> Active { get; set; }
        public static List<string> Passive { get; set; }
    }
}
