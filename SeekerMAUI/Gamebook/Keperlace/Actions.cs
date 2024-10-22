using System;

namespace SeekerMAUI.Gamebook.Keperlace
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public override bool Availability(string option) =>
            AvailabilityTrigger(option);
    }
}
