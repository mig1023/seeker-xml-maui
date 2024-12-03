using System;
using System.Collections.Generic;
using System.Linq;

namespace SeekerMAUI.Gamebook.RendezVous
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public int Dices { get; set; }

        public override List<string> Status() =>
            new List<string> { $"Осознание: {Character.Protagonist.Awareness}" };

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

        public List<string> DiceCheck()
        {
            List<string> diceCheck = new List<string> { };

            int firstDice = Game.Dice.Roll();
            int dicesResult = firstDice;

            if (Dices == 1)
            {
                diceCheck.Add($"На кубикe выпало: {Game.Dice.Symbol(firstDice)}");
            }
            else
            {
                int secondDice = Game.Dice.Roll();
                dicesResult += secondDice;

                diceCheck.Add($"На кубиках выпало:" +
                    $"{Game.Dice.Symbol(firstDice)} + " +
                    $"{Game.Dice.Symbol(secondDice)} = " +
                    $"{firstDice + secondDice}");
            }

            diceCheck.Add(dicesResult % 2 == 0 ? "BIG|ЧЁТНОЕ ЧИСЛО!" : "BIG|НЕЧЁТНОЕ ЧИСЛО!");

            return diceCheck;
        }
    }
}
