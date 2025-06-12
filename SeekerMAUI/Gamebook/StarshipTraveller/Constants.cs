using System.Collections.Generic;

namespace SeekerMAUI.Gamebook.StarshipTraveller
{
    class Constants : Prototypes.Constants, Abstract.IConstants
    {
        public static List<string> Team { get; set; }

        public static Dictionary<string,string> Names { get; set; }
    }
}
