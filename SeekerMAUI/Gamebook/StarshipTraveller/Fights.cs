using System;

namespace SeekerMAUI.Gamebook.StarshipTraveller
{
    class Fights
    {
        private static bool NoMoreEnemies(List<Character> enemies) =>
            enemies.Where(x => (x.Stamina > 0) && (x.Shields > 0)).Count() == 0;

        public static List<string> SpaceCombat(Actions action, List<Character> enemies)
        {
            List<string> fight = new List<string>();

            var round = 1;

            while (true)
            {
                fight.Add($"HEAD|BOLD|Раунд: {round}");

                foreach (Character enemy in enemies)
                {
                    fight.Add($"{enemy.Name} (защита {enemy.Shields})");

                    Game.Dice.DoubleRoll(out int rollFirst, out int rollSecond);
                    var dices = rollFirst + rollSecond;

                    fight.Add($"BOLD|Ты стреляешь: {Game.Dice.Symbol(rollFirst)} + " +
                        $"{Game.Dice.Symbol(rollSecond)} = {dices}");

                    if (dices < Character.Protagonist.Weapons)
                    {
                        fight.Add($"Сумма в {dices} меньше показателя вооружения твоего корабля!");
                        fight.Add("BOLD|Твой выстрел достиг своей цели!");

                        Game.Dice.DoubleRoll(out rollFirst, out rollSecond);
                        dices = rollFirst + rollSecond;

                        fight.Add($"Определяем повреждения: {Game.Dice.Symbol(rollFirst)} + " +
                            $"{Game.Dice.Symbol(rollSecond)} = {dices}");

                        if ((rollFirst == 6) && (rollFirst == rollSecond))
                        {
                            fight.Add($"BOLD|Выпал дубль шестёрок!");
                            fight.Add("BOLD|GOOD|Твой выстрел нанёс противнику ущерб равный 6!");

                            enemy.Shields -= 6;
                        }
                        else if (dices > enemy.Shields)
                        {
                            fight.Add($"Сумма в {dices} больше показателя защиты корабля противника!");
                            fight.Add("BOLD|GOOD|Твой выстрел нанёс противнику ущерб равный 4!");

                            enemy.Shields -= 4;
                        }
                        else
                        {
                            fight.Add($"Сумма в {dices} не превышает показателя защиты корабля противника!");
                            fight.Add("BOLD|GOOD|Твой выстрел нанёс противнику ущерб равный 2!");

                            enemy.Shields -= 2;
                        }

                        fight.Add($"Теперь уровень защиты противника стал равен {enemy.Shields}!");

                        if (enemy.Shields <= 0)
                        {
                            return action.Win(fight, you: true);
                        }
                    }
                    else
                    {
                        fight.Add($"Сумма в {dices} превышает показатель вооружения твоего корабля.");
                        fight.Add("BOLD|BAD|Ты промахнулся...");
                    }

                    Game.Dice.DoubleRoll(out rollFirst, out rollSecond);
                    dices = rollFirst + rollSecond;

                    fight.Add($"BOLD|Противник стреляет: {Game.Dice.Symbol(rollFirst)} + " +
                        $"{Game.Dice.Symbol(rollSecond)} = {dices}");

                    if (dices < enemy.Weapons)
                    {
                        fight.Add($"Сумма в {dices} меньше показателя вооружения противника!");
                        fight.Add("BOLD|Его выстрел поразил твой корабль!");

                        Game.Dice.DoubleRoll(out rollFirst, out rollSecond);
                        dices = rollFirst + rollSecond;

                        fight.Add($"Определяем повреждения: {Game.Dice.Symbol(rollFirst)} + " +
                            $"{Game.Dice.Symbol(rollSecond)} = {dices}");

                        if ((rollFirst == 6) && (rollFirst == rollSecond))
                        {
                            fight.Add($"BOLD|Выпал дубль шестёрок!");
                            fight.Add("BOLD|BAD|Его выстрел нанёс твоему кораблю ущерб равный 6!");

                            Character.Protagonist.Shields -= 6;
                        }
                        else if (dices > enemy.Shields)
                        {
                            fight.Add($"Сумма в {dices} больше показателя защиты твоего противника!");
                            fight.Add("BOLD|BAD|Его выстрел нанёс твоему кораблю ущерб равный 4!");

                            Character.Protagonist.Shields -= 4;
                        }
                        else
                        {
                            fight.Add($"Сумма в {dices} меньше показателя защиты твоего противника!");
                            fight.Add("BOLD|BAD|Его выстрел нанёс твоему кораблю ущерб равный 2!");

                            enemy.Shields -= 2;
                        }

                        fight.Add($"Теперь твой уровень защиты стал равен {Character.Protagonist.Shields}!");

                        if (Character.Protagonist.Shields <= 0)
                        {
                            return action.Fail(fight, you: true);
                        }
                    }
                    else
                    {
                        fight.Add($"Сумма в {dices} превышает показатель вооружения корабля противника.");
                        fight.Add("BOLD|GOOD|Он промахнулся!");
                    }

                    fight.Add(String.Empty);
                }

                round += 1;
            }
        }

        public static List<string> HandToHandCombat(Actions action, List<Character> enemies)
        {
            List<string> fight = new List<string>();
            return fight;
        }

        public static List<string> BlasterCombat(Actions action, List<Character> enemies)
        {
            List<string> fight = new List<string>();
            return fight;
        }
    }
}
