using System.Collections.Generic;

namespace SeekerMAUI.Gamebook.BlackCastleDungeon
{
    class Constants : Prototypes.Constants, Abstract.IConstants
    {
        public static int SecondPartStartParagraph = 620;

        public static int ThirdPartStartParagraph = 752;

        public static List<string> StaticSpells { get; set; }

        public static Dictionary<string, string> Availabilities { get; set; }
    }
}
