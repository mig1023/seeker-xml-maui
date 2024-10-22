using System.Collections.Generic;

namespace SeekerMAUI.Gamebook.LegendsAlwaysLie
{
    class Constants : Prototypes.Constants, Abstract.IConstants
    {
        public static Dictionary<string, Character.SpecializationType> GetSpecializationType() =>
            new Dictionary<string, Character.SpecializationType>
        {
            ["ВОИН"] = Character.SpecializationType.Warrior,
            ["МАГ"] = Character.SpecializationType.Wizard,
            ["МЕТАТЕЛЬ"] = Character.SpecializationType.Thrower,
        };
    }
}
