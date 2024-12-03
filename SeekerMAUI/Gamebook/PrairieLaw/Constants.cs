using System.Collections.Generic;

namespace SeekerMAUI.Gamebook.PrairieLaw
{
    class Constants : Prototypes.Constants, Abstract.IConstants
    {
        public static Dictionary<int, int> Skills { get; set; }

        public static Dictionary<int, int> Strengths { get; set; }

        public static Dictionary<int, int> Charms { get; set; }

        public static Dictionary<int, string> LuckList { get; set; }

        public static Dictionary<string, string> Availabilities { get; set; }
    }
}
