using System;

namespace SeekerMAUI.Gamebook.Thanatos
{
    class Constants : Prototypes.Constants, Abstract.IConstants
    {
        public static List<string> EndPoints { get; set; }

        public static Dictionary<string, string> Availabilities { get; set; }
    }
}
