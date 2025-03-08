using System;
using System.Collections.Generic;
using System.Linq;

namespace SeekerMAUI.Gamebook.Ants
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public override List<string> AdditionalStatus()
        {
            List<string> statusLines = new List<string>
            {
                $"Количество: {Character.Protagonist.Quantity}",
                $"Прирост: {Character.Protagonist.Increase}"
            };

            string government = String.Empty;

            foreach (string name in Constants.Government.Keys)
            {
                if (Game.Option.IsTriggered(name))
                    government = name;
            }

            if (!String.IsNullOrEmpty(government))
                statusLines.Insert(0, $"Правит: {government}");

            if (Character.Protagonist.Defence > 0)
                statusLines.Add($"Защита: {Character.Protagonist.Defence}");

            if (Character.Protagonist.EnemyHitpoints > 0)
                statusLines.Add($"{Character.Protagonist.EnemyName}: {Character.Protagonist.EnemyHitpoints}");

            return statusLines;
        }

        public override bool AvailabilityNode(string option)
        {
            if (Game.Services.AvailabilityByСomparison(option))
            {
                int level = Game.Services.LevelParse(option);

                if (option.Contains("ДАЙС =") && !Character.Protagonist.Dice[level])
                    return false;

                var fail = Game.Services.AvailabilityByProperty(Character.Protagonist,
                    option, Constants.Availabilities, onlyFailTrueReturn: true);

                if (fail)
                    return false;

                return true;
            }
            else
            {
                return AvailabilityTrigger(option);
            }
        }

        public List<string> Result()
        {
            List<string> results = new List<string>();

            bool queen = Game.Option.IsTriggered("Королева Антуанетта Плодовитая");
            bool prince = Game.Option.IsTriggered("Принц Мурадин Крылатый");
            bool soldier = Game.Option.IsTriggered("Солдат Руф Твердожвалый");

            List<string> ending = queen || prince || soldier ? Constants.EndingOne : Constants.EndingTwo;

            foreach (string endingLine in Constants.EndingOne)
                results.Add(endingLine.Replace(';', ','));

            results.Add(String.Empty);

            int speed = 300 - Character.Protagonist.Time;

            string line = String.Empty;

            foreach (KeyValuePair<string, int> timelist in Constants.Rating.OrderBy(x => x.Value))
            {
                if (speed < timelist.Value)
                {
                    break;
                }
                else
                {
                    line = timelist.Key;
                }
            }

            List<string> resultLines = line.Split('!').ToList();

            results.Add($"{resultLines[0]}!");
            results.Add($"BIG|BOLD|{resultLines[1].Trim()}");

            return results;
        }

        public List<string> Changes()
        {
            List<string> changes = new List<string>();

            foreach (string head in Constants.Government.Keys)
            {
                if (Game.Option.IsTriggered(head))
                    changes.Add(Constants.Government[head]);
            }

            return changes;
        }
    }
}
