using System;
using System.Collections.Generic;
using System.Linq;

namespace SeekerMAUI.Gamebook.PrisonerOfMoritaiCastle
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public override List<string> Status() => new List<string>
        {
            $"Жизненные силы: {Character.Protagonist.Hitpoints} / 5",
            $"Стрелы: {Character.Protagonist.Arrows}",
            $"Сюрикены: {Character.Protagonist.Shuriken}",
        };

        public override bool GameOver(out int toEndParagraph, out string toEndText) =>
            GameOverBy(Character.Protagonist.Hitpoints, out toEndParagraph, out toEndText);

        public override bool Availability(string option)
        {
            if (String.IsNullOrEmpty(option))
            {
                return true;
            }
            else if (option.Contains("|"))
            {
                return option.Split('|').Where(x => Game.Option.IsTriggered(x.Trim())).Count() > 0;
            }
            else
            {
                foreach (string oneOption in option.Split(','))
                {
                    if (Game.Services.AvailabilityByСomparison(oneOption))
                    {
                        var fail = Game.Services.AvailabilityByProperty(Character.Protagonist,
                            oneOption, Constants.Availabilities, onlyFailTrueReturn: true);

                        if (fail)
                            return false;
                    }
                    else if (oneOption.Contains("ЕСТЬ ЛЕКАРСТВА"))
                    {
                        return (Game.Healing.List().Count > 0) || Game.Option.IsTriggered("обезболивающее");
                    }
                    else if (oneOption.Contains("НЕТ ЛЕКАРСТВ"))
                    {
                        return (Game.Healing.List().Count <= 0) && !Game.Option.IsTriggered("обезболивающее");
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

        public override bool IsHealingEnabled() =>
            Character.Protagonist.Hitpoints < 5;

        public override void UseHealing(int healingLevel) =>
            Character.Protagonist.Hitpoints = 5;
    }
}
