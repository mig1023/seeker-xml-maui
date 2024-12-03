using System.Collections.Generic;

namespace SeekerMAUI.Gamebook.Catharsis
{
    class Constants : Prototypes.Constants, Abstract.IConstants
    {
        public static Dictionary<string, int> GetStartValues { get; set; }

        public static Dictionary<string, string> Availabilities { get; set; }
    }
}
