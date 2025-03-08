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

        public override bool AvailabilityNode(string option)
        {
            if (Game.Services.AvailabilityByСomparison(option))
            {
                var fail = Game.Services.AvailabilityByProperty(Character.Protagonist,
                    option, Constants.Availabilities, onlyFailTrueReturn: true);

                return !fail;
            }
            else if (option.Contains("ЕСТЬ ЛЕКАРСТВА"))
            {
                return (Game.Healing.List().Count > 0) || Game.Option.IsTriggered("обезболивающее");
            }
            else if (option.Contains("НЕТ ЛЕКАРСТВ"))
            {
                return (Game.Healing.List().Count <= 0) && !Game.Option.IsTriggered("обезболивающее");
            }
            else
            {
                return AvailabilityTrigger(option);
            }
        }

        public override bool IsHealingEnabled() =>
            Character.Protagonist.Hitpoints < 5;

        public override void UseHealing(int healingLevel) =>
            Character.Protagonist.Hitpoints = 5;
    }
}
