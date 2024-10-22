using System.Collections.Generic;

namespace SeekerMAUI.Gamebook.LastHokku
{
    class Constants : Prototypes.Constants, Abstract.IConstants
    {
        public static List<int> GetParagraphsWithoutHokkuCreation { get; set; }
    }
}
