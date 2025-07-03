using System;

namespace SeekerMAUI.Gamebook.PrairieLaw
{
    class Dice
    {
        public static List<string> Wounds(int dices)
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

            Character.Protagonist.Strength -= dicesSum;

            diceCheck.Add($"BIG|BAD|Вы потеряли жизней: {dicesSum}");

            return diceCheck;
        }
    }
}
