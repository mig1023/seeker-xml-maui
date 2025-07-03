using System;

namespace SeekerMAUI.Gamebook.LegendsAlwaysLie
{
    class Dice
    {
        public static List<string> Check()
        {
            List<string> diceCheck = new List<string> { };

            int dice = Game.Dice.Roll();

            diceCheck.Add($"На кубикe выпало: {Game.Dice.Symbol(dice)}");
            diceCheck.Add(dice % 2 == 0 ? "BIG|ЧЁТНОЕ ЧИСЛО!" : "BIG|НЕЧЁТНОЕ ЧИСЛО!");

            return diceCheck;
        }

        public static List<string> Wounds(int dices, int diceBonus)
        {
            List<string> diceCheck = new List<string> { };

            int dicesCount = (dices == 0 ? 1 : dices);
            int dicesSum = 0;

            for (int i = 1; i <= dicesCount; i++)
            {
                int dice = Game.Dice.Roll();
                dicesSum += dice;
                diceCheck.Add($"На {i} выпало: {Game.Dice.Symbol(dice)}");
            }

            if (diceBonus != 0)
            {
                dicesSum += diceBonus;
                diceCheck.Add($"Добавляем {diceBonus} по условию");
            }

            Character.Protagonist.Hitpoints -= dicesSum;

            diceCheck.Add($"BIG|BAD|Вы потеряли жизней: {dicesSum}");

            return diceCheck;
        }
    }
}
