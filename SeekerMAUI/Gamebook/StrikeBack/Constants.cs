using System.Collections.Generic;

namespace SeekerMAUI.Gamebook.StrikeBack
{
    class Constants : Prototypes.Constants, Abstract.IConstants
    {
        public static Dictionary<Character.SpecialTechniques, string> TechniquesNames() =>
            new Dictionary<Character.SpecialTechniques, string>
        {
            [Character.SpecialTechniques.RumbleKnife] = "Волшебный кинжал",
        };
    }
}
