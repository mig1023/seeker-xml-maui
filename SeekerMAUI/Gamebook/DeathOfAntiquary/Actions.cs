using System;

namespace SeekerMAUI.Gamebook.DeathOfAntiquary
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public override bool Availability(string option) =>
            AvailabilityTrigger(option);
    }
}
