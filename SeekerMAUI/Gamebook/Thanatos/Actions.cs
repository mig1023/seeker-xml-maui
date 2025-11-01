using System;

namespace SeekerMAUI.Gamebook.Thanatos
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public override bool AvailabilityNode(string option)
        {
            if (Game.Services.AvailabilityByСomparison(option))
            {
                return Game.Services.AvailabilityByProperty(Character.Protagonist,
                     option, Constants.Availabilities);
            }
            else
            {
                return AvailabilityTrigger(option);
            }
        }
    }
}
