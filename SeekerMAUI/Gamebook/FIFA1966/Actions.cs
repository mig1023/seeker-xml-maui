using System;
using System.Linq;

namespace SeekerMAUI.Gamebook.FIFA1966
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public override List<string> Status()
        {
            if (!String.IsNullOrEmpty(Character.Protagonist.Enemy))
            {
                var ussr = Character.Protagonist.Vars["ИГРА/СССР"];
                var enemy = Character.Protagonist.Vars["расходники/вороги"];
                var result = $"CCCР  {ussr} : {enemy}  {Character.Protagonist.Enemy}";
                return new List<string> { result };
            }
            else
            {
                return new List<string>();
            }
        }

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
                    var found = false;

                    foreach (var varName in Character.Protagonist.Vars.Keys())
                    {
                        if (!oneOption.Contains(varName))
                            continue;

                        found = true;

                        var value = Character.Protagonist.Vars[varName];

                        if (!Game.Services.LevelAvailability(varName, oneOption, value, level))
                            return false;
                    }

                    if (!found && !oneOption.Contains("!=")) 
                    {
                        return false;
                    }
                }

                return true;
            }
        }
    }
}
