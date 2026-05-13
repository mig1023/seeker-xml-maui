using System.Collections.Generic;

namespace SeekerMAUI.Gamebook.BehindTheThrone
{
    class Constants : Prototypes.Constants, Abstract.IConstants
    {
        public static Dictionary<string, string> CharactersParams { get; set; }

        public static int FirstPartSize = 84;
    }
}
