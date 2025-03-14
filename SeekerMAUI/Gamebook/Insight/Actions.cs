﻿using System;
using System.Collections.Generic;

namespace SeekerMAUI.Gamebook.Insight
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public string Bonus { get; set; }

        public override List<string> Representer()
        {
            if (!String.IsNullOrEmpty(Bonus))
            {
                int diff = GetProperty(Character.Protagonist, Bonus) - Constants.GetStartValues[Bonus];
                string count = Game.Services.CoinsNoun(diff, "единица", "единицы", "единицы");
                string diffLine = diff > 0 ? $"\n+{diff} {count}" : String.Empty;

                return new List<string> { $"{Head}{diffLine}" };
            }

            return new List<string>();
        }

        public override List<string> Status()
        {
            List<string> statuses = new List<string>
            {
                $"Ячейки памяти: {Character.Protagonist.Memory} / 8",
            };

            if (Character.Protagonist.TimeStop <= 0)
                statuses.Add($"Время: {Character.Protagonist.Time} / 30");

            return statuses;
        }

        public override List<string> AdditionalStatus() => new List<string>
        {
            $"Здоровье: {Character.Protagonist.Life}",
            $"Аура: {Character.Protagonist.Aura}",
            $"Ловкость: {Character.Protagonist.Skill}",
            $"Меткость: {Character.Protagonist.Weapon}",
        };

        public override bool GameOver(out int toEndParagraph, out string toEndText)
        {
            if ((Character.Protagonist.Time >= 30) && (Character.Protagonist.TimeStop <= 0))
            {
                toEndParagraph = 112;
                toEndText = "Время вышло";
                Character.Protagonist.TimeStop = 1;

                return true;
            }
            else
            {
                return GameOverBy(Character.Protagonist.Life, out toEndParagraph, out toEndText);
            }
        }

        public override bool IsButtonEnabled(bool secondButton = false)
        {
            bool disabledByBonusesRemove = !String.IsNullOrEmpty(Bonus) &&
                ((GetProperty(Character.Protagonist, Bonus) - Constants.GetStartValues[Bonus]) <= 0) &&
                secondButton;

            bool disabledByBonusesAdd = (!String.IsNullOrEmpty(Bonus)) &&
                (Character.Protagonist.Bonuses <= 0) && !secondButton;

            return !(disabledByBonusesRemove || disabledByBonusesAdd);
        }

        public override bool AvailabilityNode(string option)
        {
            if (Game.Services.AvailabilityByСomparison(option))
            {
                var fail = Game.Services.AvailabilityByProperty(Character.Protagonist,
                    option, Constants.Availabilities, onlyFailTrueReturn: true);

                return !fail;
            }
            else
            {
                return AvailabilityTrigger(option);
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
            Character.Protagonist.Life < 22;

        public override void UseHealing(int healingLevel) =>
            Character.Protagonist.Life += healingLevel;
    }
}
