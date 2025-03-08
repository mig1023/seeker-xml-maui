using System;

namespace SeekerMAUI.Gamebook.PensionerSimulator
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public override bool AvailabilityNode(string option) =>
            AvailabilityTrigger(option);
    }
}
