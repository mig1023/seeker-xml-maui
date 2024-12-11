using System;

namespace SeekerMAUI.Gamebook.Diversant
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public override bool Availability(string option) =>
            AvailabilityTrigger(option);
    }
}
