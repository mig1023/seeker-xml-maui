using System;

namespace SeekerMAUI.Gamebook.LordOfTheSteppes
{
    class Dice
    {
        public static List<string> Games()
        {
            if (Character.Protagonist.Coins < 5)
                return new List<string> { "BAD|У вас не достаточно денег, чтобы играть..." };

            List<string> diceGame = new List<string> { };

            int myResult, enemyResult;

            do
            {
                Game.Dice.DoubleRoll(out int hisFirstDice, out int hisSecondDice);
                enemyResult = hisFirstDice + hisSecondDice + 4;

                diceGame.Add($"Он бросил: " +
                    $"{Game.Dice.Symbol(hisFirstDice)} + " +
                    $"{Game.Dice.Symbol(hisSecondDice)} = {enemyResult}");

                Game.Dice.DoubleRoll(out int firstDice, out int secondDice);
                myResult = firstDice + secondDice;

                diceGame.Add($"Вы бросили: " +
                    $"{Game.Dice.Symbol(firstDice)} + " +
                    $"{Game.Dice.Symbol(secondDice)} = {myResult}");

                diceGame.Add(String.Empty);
            }
            while (myResult == enemyResult);

            if (myResult > enemyResult)
            {
                diceGame.Add("BIG|GOOD|ВЫ ВЫИГРАЛИ 5 МОНЕТ:)");
                Character.Protagonist.Coins += 5;
            }
            else
            {
                diceGame.Add("BIG|BAD|ПРОИГРАЛИ 5 МОНЕТ :(");
                Character.Protagonist.Coins -= 5;
            }

            return diceGame;
        }

        public static List<string> Check(bool odd, int dices, bool initiative)
        {
            List<string> diceCheck = new List<string> { };

            int firstDice = Game.Dice.Roll();
            int dicesResult = firstDice;

            string size = (odd ? String.Empty : "BIG|");

            if (dices == 1)
            {
                diceCheck.Add($"{size}На кубикe выпало: {Game.Dice.Symbol(firstDice)}");
            }
            else
            {
                int secondDice = Game.Dice.Roll();
                dicesResult += secondDice + (initiative ? Character.Protagonist.Initiative : 0);
                string initLine = (initiative ? $" + {Character.Protagonist.Initiative} Инициатива" : String.Empty);

                diceCheck.Add($"{size}На кубиках выпало: " +
                    $"{Game.Dice.Symbol(firstDice)} + " +
                    $"{Game.Dice.Symbol(secondDice)}{initLine} = {dicesResult}");
            }

            if (odd)
                diceCheck.Add(dicesResult % 2 == 0 ? "BIG|ЧЁТНОЕ ЧИСЛО!" : "BIG|НЕЧЁТНОЕ ЧИСЛО!");

            return diceCheck;
        }
    }
}
