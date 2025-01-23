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

        public override List<string> AdditionalStatus()
        {
            
            var match = Character.Protagonist.Vars["расходники/номер игры"];

            if (match <= 0)
            {
                return new List<string> { "Подготовка к Чемпионату мира" };
            }
            else if (match < 5)
            {
                var enemy = Constants.Matches[match];
                return new List<string> { $"Подготовка к Чемпионату мира: {enemy}" };
            }
            else if (match < 8)
            {
                var statuses = new List<string>();

                var group = Character.Protagonist.Vars
                    .ToDictionary()
                    .Where(x => x.Key.StartsWith("групповой этап/"))
                    .OrderByDescending(x => x.Value);

                int place = 0;

                foreach (var team in group)
                {
                    var name = team.Key.Replace("групповой этап/", "");
                    var value = Character.Protagonist.Vars[$"групповой этап/{name}"];

                    place += 1;

                    statuses.Add($"{place} место: {name} ({value} очков)");
                }

                return statuses;
            }
            else if (match < 10)
            {
                var enemy = Constants.Matches[match];
                var round = match == 8 ? "Четвертьфинал" : "Полуфинал";
                return new List<string> { $"{round}: {enemy}" };
            }
            else if ((match == 11) || (match == 81))
            {
                return new List<string> { $"Игра за бронзу: Португалия" };
            }
            else
            {
                return new List<string> { $"Финал: Англия" };
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
