using System;
using System.Collections.Generic;

namespace SeekerMAUI.Gamebook.InvisibleFront
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
         public override List<string> Status() => new List<string> {
            $"Недовольство резидента: {Character.Protagonist.Dissatisfaction}",
            $"Вербовка: {Character.Protagonist.Recruitment}",
        };

        public override bool AvailabilityNode(string option)
        {
            if (Game.Services.AvailabilityByСomparison(option))
            {
                var fail = Game.Services.AvailabilityByProperty(Character.Protagonist,
                    option, Constants.Availabilities, onlyFailTrueReturn: true);

                return !fail;
            }
            else
            {
                return AvailabilityTrigger(option);
            }
        }
    }
}
