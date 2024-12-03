using System.Collections.Generic;

namespace SeekerMAUI.Gamebook.Cyberpunk
{
    class Constants : Prototypes.Constants, Abstract.IConstants
    {
        public static Dictionary<string, string> CharactersParams { get; set; }

        public static Dictionary<string, string> Availabilities { get; set; }

        public static List<string> NormalizationParams { get; set; }
    }
}
