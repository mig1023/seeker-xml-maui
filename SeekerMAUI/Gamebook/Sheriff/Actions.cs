using System;

namespace SeekerMAUI.Gamebook.Sheriff
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public override bool Availability(string option)
        {
            if (String.IsNullOrEmpty(option))
                return true;

            foreach (string oneOption in option.Split(','))
            {
                if (Game.Services.AvailabilityByСomparison(oneOption))
                {
                    var fail = Game.Services.AvailabilityByProperty(Character.Protagonist,
                        oneOption, Constants.Availabilities, onlyFailTrueReturn: true);

                    if (fail)
                        return false;
                }
                else if (oneOption.Contains("!"))
                {
                    if (Game.Option.IsTriggered(oneOption.Replace("!", String.Empty).Trim()))
                        return false;
                }
                else if (!Game.Option.IsTriggered(oneOption.Trim()))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
