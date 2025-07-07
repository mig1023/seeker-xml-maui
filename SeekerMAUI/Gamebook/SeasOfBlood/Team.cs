using System;

namespace SeekerMAUI.Gamebook.SeasOfBlood
{
    class Team
    {
        private static int SizeBonus(ref List<string> test, string line, int summ, int bonus)
        {
            int newSumm = summ - bonus;

            if (newSumm <= 0)
                newSumm = 0;

            test.Add($"GOOD|Благодаря {line}, " +
                $"выпавшая сумма уменьшается на {bonus} и теперь равна {newSumm}");

            return newSumm;
        }

        public static List<string> SizeTest(int more, int less)
        {
            List<string> test = new List<string>();

            int size = Character.Protagonist.TeamSize;
            string line = Game.Services.CoinsNoun(size, "пират", "пирата", "пиратов");
            test.Add($"Численность команды: {size} {line}");

            int dicesSumm = 0;
            bool curse = Game.Option.IsTriggered("Проклятие морских эльфов");
            int first = Game.Dice.Roll();
            int second = Game.Dice.Roll();

            if (curse)
            {
                test.Add("BAD|Из-за проклятие морских эльфов нет нужды " +
                    "в броске кубиков: выпавший результат всегда будет " +
                    "больше численности команды!");
            }
            else if (Game.Option.IsTriggered("Благословление морских эльфов"))
            {
                test.Add("GOOD|Благодаря благословению морских эльфов, " +
                    "нужно кидать только два кубика вместо трёх!");

                dicesSumm = first + second;

                test.Add($"Бросаем кубики: {Game.Dice.Symbol(first)} + " +
                    $"{Game.Dice.Symbol(second)} = {dicesSumm}");
            }
            else
            {
                int third = Game.Dice.Roll();
                dicesSumm = first + second + third;

                test.Add($"Бросаем кубики: {Game.Dice.Symbol(first)} + " +
                    $"{Game.Dice.Symbol(second)} + {Game.Dice.Symbol(third)} = {dicesSumm}");
            }

            if (!curse && Game.Option.IsTriggered("Благословление"))
            {
                dicesSumm = SizeBonus(ref test, "благословению призрака", dicesSumm, 2);
            }

            if (!curse && Game.Option.IsTriggered("Мешки"))
            {
                dicesSumm = SizeBonus(ref test, "мешкам Короля Четырех Ветров", dicesSumm, 4);
            }

            int days = 0;

            if ((dicesSumm < size) && !curse)
            {
                test.Add("BIG|BOLD|Выпало МЕНЬШЕ численности!");

                if (less > 0)
                    days = less;

                Game.Buttons.Disable("Больше или равно ЧИСЛЕННОСТИ твоего экипажа");
            }
            else
            {
                test.Add("BIG|BOLD|Выпало БОЛЬШЕ или равно численности!");

                if (more > 0)
                    days = more;

                Game.Buttons.Disable("Меньше ЧИСЛЕННОСТИ твоего экипажа");
            }

            if (days > 0)
            {
                string count = Game.Services.CoinsNoun(days, "день", "дня", "дней");
                test.Add($"В судовой журнал пишем {days} {count}");
                Character.Protagonist.Logbook += days;

                if (Character.Protagonist.Endurance < Character.Protagonist.MaxEndurance)
                {
                    Character.Protagonist.Endurance += 1;
                    test.Add("GRAY|Ты восстанавливаешь 1 единицу выносливости");
                }
            }

            return test;
        }

        public static List<string> Fight(Actions actions, List<Character> Enemies)
        {
            List<string> fight = new List<string>();

            Character enemyTeam = Enemies.First().Clone();

            int round = 1;

            while (true)
            {
                fight.Add($"HEAD|BOLD|Раунд: {round}");

                fight.Add($"{enemyTeam.Name} (численность {enemyTeam.TeamSize})");

                Game.Dice.DoubleRoll(out int teamRollFirst, out int teamRollSecond);
                int teamStrength = teamRollFirst + teamRollSecond + Character.Protagonist.TeamStrength;

                fight.Add($"Сила удара твоей команды: " +
                    $"{Game.Dice.Symbol(teamRollFirst)} + " +
                    $"{Game.Dice.Symbol(teamRollSecond)} + " +
                    $"{Character.Protagonist.TeamStrength} = {teamStrength}");

                Game.Dice.DoubleRoll(out int enemyRollFirst, out int enemyRollSecond);
                int enemyStrength = enemyRollFirst + enemyRollSecond + enemyTeam.TeamStrength;

                fight.Add($"Сила его удара: " +
                    $"{Game.Dice.Symbol(enemyRollFirst)} + " +
                    $"{Game.Dice.Symbol(enemyRollSecond)} + " +
                    $"{enemyTeam.TeamStrength} = {enemyStrength}");

                if (teamStrength > enemyStrength)
                {
                    fight.Add("GOOD|Противник проиграл раунд");
                    fight.Add("Его численность уменьшилась на 2");

                    enemyTeam.TeamSize -= 2;

                    if (enemyTeam.TeamSize <= 0)
                    {
                        fight.Add(String.Empty);
                        fight.Add("BIG|GOOD|Вы ПОБЕДИЛИ :)");
                        return fight;
                    }
                }
                else if (teamStrength < enemyStrength)
                {
                    fight.Add($"BAD|Противник выиграл раунд");
                    fight.Add("Численность твоей команды уменьшилась на 2");

                    Character.Protagonist.TeamSize -= 2;

                    if (Character.Protagonist.TeamSize <= 0)
                        return actions.Fail(fight, you: true);
                }
                else
                {
                    fight.Add("BOLD|Ничья в раунде");
                }

                fight.Add(String.Empty);

                round += 1;
            }
        }
    }
}
