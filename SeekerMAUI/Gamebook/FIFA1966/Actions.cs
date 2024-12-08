using System;

namespace SeekerMAUI.Gamebook.FIFA1966
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public override bool Availability(string option)
        {
            if (String.IsNullOrEmpty(option))
            {
                return true;
            }
            else
            {
                foreach (var oneOption in option.Split(','))
                {
                    var level = Game.Services.LevelParse(oneOption);

                    foreach (var varName in Character.Protagonist.Vars.Keys)
                    {
                        if (!oneOption.Contains(varName))
                            continue;

                        var value = Character.Protagonist.Vars[varName];

                        if (!Game.Services.LevelAvailability(varName, oneOption, value, level))
                            return false;
                    }
                }

                return true;
            }
        }
    }
}
