using System;

namespace SeekerMAUI.Gamebook.Hunt
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public override List<string> Status()
        {
            string zombies = new string('x', Character.Protagonist.Bitten).Replace("x", "🧟");
            string bitten = String.IsNullOrEmpty(zombies) ? "ни одного" : zombies;
            return new List<string>{ $"Укушенные: {bitten}" };
        }

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
