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
