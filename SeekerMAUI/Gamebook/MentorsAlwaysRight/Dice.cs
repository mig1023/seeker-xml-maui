using System;

namespace SeekerMAUI.Gamebook.MentorsAlwaysRight
{
    class Dice
    {
        public static List<string> Roll()
        {
            int dice = Game.Dice.Roll();
            string odd = dice % 2 == 0 ? "чёт" : "нечет";
            return new List<string> { $"BIG|На кубике выпало: {Game.Dice.Symbol(dice)} - {odd}" };
        }

        public static List<string> Games()
        {
            List<string> game = new List<string> { };

            Game.Dice.DoubleRoll(out int firstDice, out int secondDice);

            game.Add($"На кубиках выпало: {Game.Dice.Symbol(firstDice)} и {Game.Dice.Symbol(secondDice)}");
            game.Add($"BIG|BOLD|Итого выпало: {firstDice + secondDice}");

            return game;
        }

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

            Character.Protagonist.Hitpoints -= dicesSum;

            diceCheck.Add($"BIG|BAD|Вы потеряли жизней: {dicesSum}");

            return diceCheck;
        }
    }
}
