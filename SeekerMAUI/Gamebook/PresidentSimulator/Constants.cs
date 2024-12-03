using System.Collections.Generic;

namespace SeekerMAUI.Gamebook.PresidentSimulator
{
    class Constants : Prototypes.Constants, Abstract.IConstants
    {
        public static Dictionary<string, string> TextByYears { get; set; }

        public static Dictionary<string, string> Availabilities { get; set; }
    }
}
