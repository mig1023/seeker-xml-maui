using System;

namespace SeekerMAUI.Gamebook.Sheriff
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public override bool Availability(string option)
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
