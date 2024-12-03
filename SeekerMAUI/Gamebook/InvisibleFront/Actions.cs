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

        public override bool Availability(string option)
        {
            if (String.IsNullOrEmpty(option))
            {
                return true;
            }
            else
            {
                string[] options = option.Split('|', ',');

                foreach (string oneOption in options)
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
                        return false;
                }

                return true;
            }
        }
    }
}
