using System;
using System.Collections.Generic;
using System.Linq;

namespace SeekerMAUI.Gamebook.ThoseWhoAreAboutToDie
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public override List<string> Status() => new List<string>
        {
            $"Реакция: {Character.Protagonist.Reaction}/12",
            $"Сила: {Character.Protagonist.Strength}/12",
            $"Выносливость: {Character.Protagonist.Endurance}/12",
        };

        public override bool GameOver(out int toEndParagraph, out string toEndText) =>
            GameOverBy(Character.Protagonist.Endurance, out toEndParagraph, out toEndText);

        public override bool Availability(string option)
        {
            if (option.Contains(">") || option.Contains("<"))
            {
                if (Params.Fail(option))
                    return false;
            }

            return AvailabilityTrigger(option);
        }

        public List<string> TryToWound()
        {
            List<string> report = new List<string>();

            int dice = Game.Dice.Roll();
            report.Add($"На кубике выпало: {Game.Dice.Symbol(dice)}");

            if (dice > 4)
            {
                Character.Protagonist.Reaction += 3;
                report.Add("BIG|GOOD|+3 к Реакции! :)");
            }
            else
            {
                report.Add("BIG|BAD|Не повезло :(");
            }

            return report;
        }
    }
}
