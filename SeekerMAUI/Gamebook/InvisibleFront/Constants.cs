using System.Collections.Generic;

namespace SeekerMAUI.Gamebook.InvisibleFront
{
    class Constants : Prototypes.Constants, Abstract.IConstants
    {
        public static List<string> GetApartments { get; set; }

        public static Dictionary<string, string> Availabilities { get; set; }
    }
}
