using System.Collections.Generic;

namespace SeekerMAUI.Gamebook.CaptainSheltonsSecret
{
    class Constants : Prototypes.Constants, Abstract.IConstants
    {
        public static Dictionary<int, int> Mastery { get; set; }

        public static Dictionary<int, int> Endurances { get; set; }

        public static Dictionary<int, string> LuckList { get; set; }

        public static Dictionary<string, string> Availabilities { get; set; }
    }
}
