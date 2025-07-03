using System;

namespace SeekerMAUI.Gamebook.PrairieLaw
{
    class Games
    {
        public static List<string> Big()
        {
            List<string> gameReport = new List<string>();

            int dice = Game.Dice.Roll();
            bool even = (dice % 2 == 0);
            bool nuggetsGame = Game.Option.IsTriggered("Игра на самородок");
            string evelLine = even ? "чётное" : "нечётное";

            gameReport.Add($"На кубике выпало: {Game.Dice.Symbol(dice)} - {evelLine}");

            if (even)
            {
                gameReport.Add("GOOD|Вы ВЫИГРАЛИ :)");

                if (nuggetsGame)
                {
                    gameReport.Add("Самородок теперь ваш.");
                    Character.Protagonist.Nuggets += 1;
                }
                else
                {
                    gameReport.Add("Вы выиграли 3 доллара.");
                    Character.Protagonist.Cents += 300;
                }
            }
            else
            {
                gameReport.Add("BAD|Вы ПРОИГРАЛИ :(");

                if (nuggetsGame)
                {
                    gameReport.Add("Вы потеряли 1$");
                    Character.Protagonist.Cents -= 100;
                }
                else
                {
                    gameReport.Add("Вы потеряли 3$");
                    Character.Protagonist.Cents -= 300;
                }
            }

            return gameReport;
        }

        public static List<string> Ltl()
        {
            List<string> gameReport = new List<string>();

            int dice = Game.Dice.Roll();
            bool even = (dice % 2 == 0);
            string evelLine = even ? "чётное" : "нечётное";

            gameReport.Add($"На кубике выпало: {Game.Dice.Symbol(dice)} - {evelLine}");

            if (even)
            {
                gameReport.Add("GOOD|Вы ВЫИГРАЛИ и получаете 1$ :)");
                Character.Protagonist.Cents += 100;
            }
            else
            {
                gameReport.Add("BAD|Вы ПРОИГРАЛИ и потеряли 1$ :(");
                Character.Protagonist.Cents -= 100;
            }

            return gameReport;
        }

        public static List<string> Double()
        {
            List<string> gameReport = new List<string>();

            Game.Dice.DoubleRoll(out int firstDice, out int secondDice);

            gameReport.Add($"На рулетке выпали: " +
                $"{Game.Dice.Symbol(firstDice)} и {Game.Dice.Symbol(secondDice)}");

            if (firstDice == secondDice)
            {
                gameReport.Add("GOOD|Цифры совпали - вы ВЫИГРАЛИ и получили 5$ :)");
                Character.Protagonist.Cents += 500;
            }
            else
            {
                gameReport.Add("BAD|Вы ПРОИГРАЛИ и потеряли 1$ :(");
                Character.Protagonist.Cents -= 100;
            }

            return gameReport;
        }

        public static List<string> RedOrBlack()
        {
            List<string> gameReport = new List<string>();

            bool red = (Game.Dice.Roll() > 3);
            int dice = Game.Dice.Roll();
            bool even = (dice % 2 == 0);
            string redLine = red ? "красное (чёт)" : "чёрное (нечет)";
            string evenLine = even ? "красное" : "чёрное";

            gameReport.Add($"Вы поставили на {redLine}");
            gameReport.Add($"На рулетке выпало: {Game.Dice.Symbol(dice)} - {evenLine}");

            if (red == even)
            {
                gameReport.Add("GOOD|Вы ВЫИГРАЛИ и получили 1$ :)");
                Character.Protagonist.Cents += 100;
            }
            else
            {
                gameReport.Add("BAD|Вы ПРОИГРАЛИ и потеряли 1$ :(");
                Character.Protagonist.Cents -= 100;
            }

            return gameReport;
        }
    }
}
