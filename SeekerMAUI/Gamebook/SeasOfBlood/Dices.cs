using System;

namespace SeekerMAUI.Gamebook.SeasOfBlood
{
    class Dices
    {
        private static int Roll(ref List<string> lines)
        {
            int first = Game.Dice.Roll();
            int second = Game.Dice.Roll();

            int summ = first + second;

            lines.Add($"Бросаем кубики: {Game.Dice.Symbol(first)} + " +
                $"{Game.Dice.Symbol(second)} = {first}");

            return summ;
        }

        public static List<string> Games()
        {
            List<string> game = new List<string>();

            int summ = Roll(ref game);

            if (summ == 7)
            {
                game.Add("BIG|GOOD|BOLD|Выпала семёрка!!");
                Game.Buttons.Disable("Выпало любое другое число");
            }
            else
            {
                game.Add("BIG|BAD|BOLD|Выпала не семёрка...");
                Game.Buttons.Disable("Выпала семёрка");
            }

            return game;
        }

        public static List<string> Dead()
        {
            List<string> dead = new List<string>();

            int summ = Roll(ref dead);
            string line = Game.Services.CoinsNoun(summ, "члена", "членов", "членов");

            dead.Add($"BIG|BAD|Ты потерял {summ} {line} экипажа!");

            Character.Protagonist.TeamSize -= summ;

            return dead;
        }

        public static List<string> Wounds()
        {
            List<string> wounds = new List<string>();

            int summ = Roll(ref wounds);
            string line = Game.Services.CoinsNoun(summ, "единицу", "единицы", "единиц");

            wounds.Add($"BIG|BAD|Ты потерял {summ} {line} выносливости!");

            Character.Protagonist.Endurance -= summ;

            return wounds;
        }

        public static List<string> Slaves()
        {
            List<string> slaves = new List<string>();

            Game.Dice.DoubleRoll(out int first, out int second);
            int count = first + second;

            slaves.Add($"Бросаем кубики: {Game.Dice.Symbol(first)} + " +
                $"{Game.Dice.Symbol(second)} = {count}");

            string line = Game.Services.CoinsNoun(count, "невольника", "невольников", "невольников");
            slaves.Add($"BIG|BAD|Ты захватил {count} {line}!");

            Character.Protagonist.Spoils -= count;

            return slaves;
        }

        public static List<string> Day()
        {
            List<string> test = new List<string>();

            int dice = Game.Dice.Roll();

            test.Add($"Бросаем кубик: {Game.Dice.Symbol(dice)}");

            int days = dice <= 3 ? 2 : 3;

            test.Add($"BIG|В судовой журнал пишем {days} дня!");
            Character.Protagonist.Logbook += days;

            return test;
        }

        public static List<string> Path()
        {
            List<string> test = new List<string>();

            int dice = Game.Dice.Roll();

            test.Add($"BIG|Бросаем кубик: {Game.Dice.Symbol(dice)}");

            if (dice < 3)
            {
                Game.Buttons.Disable("Выпало 3 или 4, Выпало 5 или 6");
            }
            else if (dice < 5)
            {
                Game.Buttons.Disable("Выпало 1 или 2, Выпало 5 или 6");
            }
            else
            {
                Game.Buttons.Disable("Выпало 1 или 2, Выпало 3 или 4");
            }

            return test;
        }
    }
}
