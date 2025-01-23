using System.Collections.Generic;

namespace SeekerMAUI.Gamebook.FIFA1966
{
    class Constants : Prototypes.Constants, Abstract.IConstants
    {
        public static Dictionary<string, int> Teams { get; set; }
        public static Dictionary<int, string> Matches { get; set; }
    }
}
