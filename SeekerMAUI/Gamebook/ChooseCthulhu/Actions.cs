using System;
using System.Collections.Generic;

namespace SeekerMAUI.Gamebook.ChooseCthulhu
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public override List<string> Status()
        {
            string cursed = Character.Protagonist.IsCursed() ? " (проклят)" : String.Empty;
            return new List<string> { $"Посвящение: {Character.Protagonist.Initiation}{cursed}" };
        }

        public override bool Availability(string option)
        {
            if (String.IsNullOrEmpty(option))
            {
                return true;
            }
            else if (Game.Services.AvailabilityByСomparison(option))
            {
                return Game.Services.AvailabilityByProperty(Character.Protagonist,
                    option, Constants.Availabilities);
            }
            else
            {
                return AvailabilityTrigger(option.Trim());
            }
        }
    }
}
