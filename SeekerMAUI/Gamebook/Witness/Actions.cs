using System;

namespace SeekerMAUI.Gamebook.Witness
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public override bool AvailabilityNode(string option) =>
            AvailabilityTrigger(option);
    }
}
