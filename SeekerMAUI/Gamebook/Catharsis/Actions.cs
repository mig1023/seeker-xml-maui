﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace SeekerMAUI.Gamebook.Catharsis
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public string Bonus { get; set; }

        public override List<string> Representer()
        {
            if (!String.IsNullOrEmpty(Bonus))
            {
                int diff = (GetProperty(Character.Protagonist, Bonus) - Constants.GetStartValues[Bonus]);
                string count = Game.Services.CoinsNoun(diff, "единица", "единицы", "единицы");
                string diffLine = (diff > 0 ? $"\n+{diff} {count}" : String.Empty);

                return new List<string> { $"{Head}{diffLine}" };
            }

            return new List<string>();
        }

        public override List<string> Status() => new List<string>
        {
            $"Здоровье: {Character.Protagonist.Life}/{Character.Protagonist.MaxLife}",
            $"Аура: {Character.Protagonist.Aura}",
        };

        public override List<string> AdditionalStatus() =>  new List<string>
        {
            $"Меткость: {Character.Protagonist.Accuracy}",
            $"Рукопашный бой: {Character.Protagonist.Fight}",
            $"Стелс: {Character.Protagonist.Stealth}",
        };

        public override bool GameOver(out int toEndParagraph, out string toEndText) =>
            GameOverBy(Character.Protagonist.Life, out toEndParagraph, out toEndText);

        public override bool IsButtonEnabled(bool secondButton = false)
        {
            bool disabledByBonusesRemove = !String.IsNullOrEmpty(Bonus) &&
                ((GetProperty(Character.Protagonist, Bonus) - Constants.GetStartValues[Bonus]) <= 0) && secondButton;

            bool disabledByBonusesAdd = (!String.IsNullOrEmpty(Bonus)) && (Character.Protagonist.Bonuses <= 0) && !secondButton;

            return !(disabledByBonusesRemove || disabledByBonusesAdd);
        }

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

                    else if (!Game.Option.IsTriggered(oneOption.Trim()))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public List<string> Get()
        {
            if (!String.IsNullOrEmpty(Bonus) && (Character.Protagonist.Bonuses >= 0))
                ChangeProtagonistParam(Bonus, Character.Protagonist, "Bonuses");

            return new List<string> { "RELOAD" };
        }

        public List<string> Decrease() =>
            ChangeProtagonistParam(Bonus, Character.Protagonist, "Bonuses", decrease: true);

        public override bool IsHealingEnabled() =>
            Character.Protagonist.Life < Character.Protagonist.MaxLife;

        public override void UseHealing(int healingLevel) =>
            Character.Protagonist.Life += healingLevel;
    }
}
