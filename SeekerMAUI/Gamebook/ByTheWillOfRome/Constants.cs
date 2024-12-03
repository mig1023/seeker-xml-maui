using System.Collections.Generic;

namespace SeekerMAUI.Gamebook.ByTheWillOfRome
{
    class Constants : Prototypes.Constants, Abstract.IConstants
    {
        public static int AddonStartParagraph = 375;

        public static Dictionary<string, string> Availabilities { get; set; }
    }
}
