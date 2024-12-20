﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace SeekerMAUI.Gamebook.SeasOfBlood
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public int Less { get; set; }
        public int More { get; set; }
        public int MasteryPenalty { get; set; }
        public bool SilentMonk { get; set; }
        public bool Leech { get; set; }
        public bool FirstBlood { get; set; }

        public List<Character> Enemies { get; set; }

        public override List<string> Status()
        {
            if (Character.Protagonist.Cyclops == null)
            {
                return new List<string>
                {
                    $"Сила команды: {Character.Protagonist.TeamStrength}",
                    $"Численность: {Character.Protagonist.TeamSize}/{Character.Protagonist.MaxTeamSize}",
                    $"День: {Character.Protagonist.Logbook}/50",
                };
            }
            else
            {
                return new List<string>
                {
                    $"Выносливость циклопа: {Character.Protagonist.Cyclops}/16"
                };
            }
        }

        public override List<string> AdditionalStatus() => new List<string>
        {
            $"Мастерство: {Character.Protagonist.Mastery}",
            $"Выносливость: {Character.Protagonist.Endurance}/{Character.Protagonist.MaxEndurance}",
            $"Удачливость: {Character.Protagonist.Luck}/{Character.Protagonist.MaxLuck}",
            $"Золото: {Character.Protagonist.Coins}",
            $"Рабы: {Character.Protagonist.Spoils}",
        };

        public override List<string> Representer()
        {
            List<string> enemies = new List<string>();

            if (Type.StartsWith("HirePirates"))
            {
                string coins = Game.Services.CoinsNoun(Price, "монету", "монеты", "монет");
                string multi = Type.EndsWith("Random") ? "ОВ" : "А";
                return new List<string> { $"НАЙМ ПИРАТ{multi}\nза {Price} {coins}" };
            }

            if (Enemies == null)
            {
                return enemies;
            }

            if (Type == "TeamFight")
            {
                Character enemy = Enemies.First();
                enemies.Add($"{enemy.Name}\nсила {enemy.TeamStrength}  численность {enemy.TeamSize}");
            }
            else
            {
                foreach (Character enemy in Enemies)
                    enemies.Add($"{enemy.Name}\nмастерство {enemy.Mastery}  выносливость {enemy.Endurance}");
            }
            
            return enemies;
        }

        public override bool GameOver(out int toEndParagraph, out string toEndText)
        {
            if (Character.Protagonist.Cyclops == 0)
            {
                toEndParagraph = 311;
                toEndText = "Вы победили циклопа!";
                Character.Protagonist.Cyclops = null;

                return true;
            }
            else
            {
                toEndParagraph = 0;
                toEndText = Output.Constants.GAMEOVER_TEXT;

                bool byEndurance = Character.Protagonist.Endurance <= 0;
                bool byTeamSize = Character.Protagonist.TeamSize <= 0;

                return (byEndurance || byTeamSize);
            }
        }

        public override bool Availability(string option)
        {
            if (String.IsNullOrEmpty(option))
            {
                return true;
            }
            else if (option.StartsWith("СОКРОВИЩА"))
            {
                int level = Game.Services.LevelParse(option);
                int coins = (int)Math.Round((double)Character.Protagonist.Coins / 100) * 100;

                if (level == 100)
                {
                    return coins <= 100;
                }
                else if (level == 800)
                {
                    return coins >= 800;
                }
                else
                {
                    return coins == level;
                }
            }
            else if (option.StartsWith("ДЕНЬГИ >="))
            {
                int money = Game.Services.LevelParse(option);
                return Character.Protagonist.Coins >= money;
            }
            else if (option.EndsWith("ЧИСЛО ДНЕЙ"))
            {
                bool even = Character.Protagonist.Logbook % 2 == 0;
                return option.StartsWith("ЧЕТНОЕ") ? even : !even;
            }
            else
            {
                return AvailabilityTrigger(option);
            }
        }

        public override bool IsButtonEnabled(bool secondButton = false)
        {
            if (Type == "SellSlaves")
            {
                return Character.Protagonist.Spoils > 0;
            }
            else if (Type.StartsWith("HirePirates"))
            {
                bool coins = Character.Protagonist.Coins >= Price;
                bool team = Character.Protagonist.TeamSize < Character.Protagonist.MaxTeamSize;

                return coins && team;
            }
            else
            {
                return true;
            }
        }

        public static bool NoMoreEnemies(List<Character> enemies) =>
            enemies.Where(x => x.Endurance > 0).Count() == 0;

        public List<string> Fight()
        {
            List<string> fight = new List<string>();

            List<Character> FightEnemies = new List<Character>();

            foreach (Character enemy in Enemies)
                FightEnemies.Add(enemy.Clone());

            int round = 1;

            while (true)
            {
                fight.Add($"HEAD|BOLD|Раунд: {round}");

                bool attackAlready = false;
                int protagonistHitStrength = 0;

                foreach (Character enemy in FightEnemies)
                {
                    if (enemy.Endurance <= 0)
                        continue;

                    fight.Add($"{enemy.Name} (выносливость {enemy.Endurance})");

                    if (!attackAlready)
                    {
                        int mastery = Character.Protagonist.Mastery - MasteryPenalty;

                        Game.Dice.DoubleRoll(out int protagonistRollFirst, out int protagonistRollSecond);
                        protagonistHitStrength = protagonistRollFirst + protagonistRollSecond + mastery;

                        fight.Add($"Сила твоего удара: " +
                            $"{Game.Dice.Symbol(protagonistRollFirst)} + " +
                            $"{Game.Dice.Symbol(protagonistRollSecond)} + " +
                            $"{mastery} = {protagonistHitStrength}");
                    }

                    Game.Dice.DoubleRoll(out int enemyRollFirst, out int enemyRollSecond);
                    int enemyHitStrength = enemyRollFirst + enemyRollSecond + enemy.Mastery;

                    fight.Add($"Сила его удара: " +
                        $"{Game.Dice.Symbol(enemyRollFirst)} + " +
                        $"{Game.Dice.Symbol(enemyRollSecond)} + " +
                        $"{enemy.Mastery} = {enemyHitStrength}");

                    if ((protagonistHitStrength > enemyHitStrength) && !attackAlready)
                    {
                        fight.Add($"GOOD|{enemy.Name} ранен");

                        bool alreadyHit = false;

                        if (FirstBlood)
                        {
                            Game.Buttons.Disable("Проиграл");

                            fight.Add(String.Empty);
                            fight.Add("BIG|GOOD|Ты ПОБЕДИЛ великана :)");
                            return fight;
                        }

                        if (Game.Option.IsTriggered("Посох безмолвного монаха"))
                        {
                            int monkStaff = Game.Dice.Roll();

                            fight.Add("GRAY|Бросаем кубик ранений от посоха: " +
                                $"{Game.Dice.Symbol(monkStaff)}");

                            if (monkStaff < 3)
                            {
                                fight.Add("Противник теряет 1 очко Мастерства");
                                enemy.Mastery -= 1;
                                alreadyHit = true;
                            }
                        }

                        if (!alreadyHit)
                        {
                            fight.Add("Противник теряет 2 очка Выносливости");
                            enemy.Endurance -= 2;
                        }

                        if (NoMoreEnemies(FightEnemies))
                            return Win(fight, you: true);
                    }
                    else if ((protagonistHitStrength > enemyHitStrength) && !Leech)
                    {
                        fight.Add($"BOLD|{enemy.Name} не смог тебя ранить");
                    }
                    else if ((protagonistHitStrength < enemyHitStrength) && !Leech)
                    {
                        fight.Add($"BAD|{enemy.Name} ранил тебя");

                        if (FirstBlood)
                        {
                            Game.Buttons.Disable("Победил");

                            fight.Add(String.Empty);
                            fight.Add("BIG|BAD|Ты ПРОИГРАЛ великану :(");
                            return fight;
                        }

                        if (SilentMonk)
                        {
                            int monkDice = Game.Dice.Roll();

                            fight.Add("GRAY|Бросаем кубик ранений от посоха: " +
                                $"{Game.Dice.Symbol(monkDice)}");

                            if (monkDice < 3)
                            {
                                fight.Add("Ты теряешь 2 очка Выносливости обычным образом");
                                Character.Protagonist.Endurance -= 2;
                            }
                            else if (monkDice < 5)
                            {
                                fight.Add("Ты теряешь 1 очко Мастерства");
                                Character.Protagonist.Mastery -= 1;
                            }
                            else
                            {
                                fight.Add("Ты теряешь 1 очко Удачливости");
                                Character.Protagonist.Luck -= 1;
                            }
                        }
                        else
                        {
                            fight.Add("Ты теряешь 2 очка Выносливости");
                            Character.Protagonist.Endurance -= 2;
                        }

                        if (Character.Protagonist.Endurance <= 0)
                            return Fail(fight, you: true);
                    }
                    else if (!Leech)
                    {
                        fight.Add("BOLD|Ничья в раунде");
                    }

                    if (Leech)
                    {
                        fight.Add($"BAD|Пиявка высасывает силы");
                        fight.Add("Ты теряешь 2 очка Выносливости");
                        Character.Protagonist.Endurance -= 2;
                    }

                    attackAlready = true;

                    fight.Add(String.Empty);
                }

                round += 1;
            }
        }

        public List<string> TeamFight()
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
                        return Fail(fight, you: true);
                }
                else
                {
                    fight.Add("BOLD|Ничья в раунде");
                }

                fight.Add(String.Empty);

                round += 1;
            }
        }

        private int TeamSizeBonus(ref List<string> test, string line, int summ, int bonus)
        {
            int newSumm = summ - bonus;

            if (newSumm <= 0)
                newSumm = 0;

            test.Add($"GOOD|Благодаря {line}, " +
                $"выпавшая сумма уменьшается на {bonus} и теперь равна {newSumm}");

            return newSumm;
        }

        public List<string> TeamSizeTest()
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
                dicesSumm = TeamSizeBonus(ref test, "благословению призрака", dicesSumm, 2);
            }

            if (!curse && Game.Option.IsTriggered("Мешки"))
            {
                dicesSumm = TeamSizeBonus(ref test, "мешкам Короля Четырех Ветров", dicesSumm, 4);
            }

            int days = 0;

            if ((dicesSumm < size) && !curse)
            {
                test.Add("BIG|BOLD|Выпало МЕНЬШЕ численности!");

                if (Less > 0)
                    days = Less;

                Game.Buttons.Disable("Больше или равно ЧИСЛЕННОСТИ твоего экипажа");
            }
            else
            {
                test.Add("BIG|BOLD|Выпало БОЛЬШЕ или равно численности!");

                if (More > 0)
                    days = More;

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

        public List<string> Luck()
        {
            Game.Dice.DoubleRoll(out int firstDice, out int secondDice);

            bool goodLuck = (firstDice + secondDice) <= Character.Protagonist.Luck;
            string luckLine = goodLuck ? "<=" : ">";

            List<string> luckCheck = new List<string> {
                $"Проверка удачи: {Game.Dice.Symbol(firstDice)} + " +
                $"{Game.Dice.Symbol(secondDice)} {luckLine} {Character.Protagonist.Luck}" };

            luckCheck.Add(goodLuck ? "BIG|GOOD|УСПЕХ :)" : "BIG|BAD|НЕУДАЧА :(");

            if (Character.Protagonist.Luck > 2)
            {
                Character.Protagonist.Luck -= 1;
                luckCheck.Add("Уровень удачи снижен на единицу");
            }

            Game.Buttons.Disable(goodLuck, "Повезло", $"Не повезло");

            return luckCheck;
        }

        public List<string> SellSlaves()
        {
            List<string> sell = new List<string>();

            int spoils = Character.Protagonist.Spoils;

            sell.Add($"Число невольников: {spoils}");
            sell.Add($"Продажная цена: {Price}");

            int summ = spoils * Price;

            Character.Protagonist.Spoils = 0;
            Character.Protagonist.Coins += summ;

            string coins = Game.Services.CoinsNoun(summ, "монета", "монеты", "монет");
            sell.Add($"BIG|Выручка: {spoils} x {Price} = {summ} {coins}");

            return sell;
        }

        private int Dices(ref List<string> lines)
        {
            int first = Game.Dice.Roll();
            int second = Game.Dice.Roll();

            int summ = first + second;

            lines.Add($"Бросаем кубики: {Game.Dice.Symbol(first)} + " +
                $"{Game.Dice.Symbol(second)} = {first}");

            return summ;
        }

        public List<string> DebtGame()
        {
            List<string> game = new List<string>();

            int summ = Dices(ref game);
            
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

        public List<string> DeadByDice()
        {
            List<string> dead = new List<string>();

            int summ = Dices(ref dead);
            string line = Game.Services.CoinsNoun(summ, "члена", "членов", "членов");

            dead.Add($"BIG|BAD|Ты потерял {summ} {line} экипажа!");

            Character.Protagonist.TeamSize -= summ;

            return dead;
        }

        public List<string> WoundsByDice()
        {
            List<string> wounds = new List<string>();

            int summ = Dices(ref wounds);
            string line = Game.Services.CoinsNoun(summ, "единицу", "единицы", "единиц");

            wounds.Add($"BIG|BAD|Ты потерял {summ} {line} выносливости!");

            Character.Protagonist.Endurance -= summ;

            return wounds;
        }

        private List<string> Hire(int count)
        {
            Character.Protagonist.Coins -= Price;
            Character.Protagonist.TeamSize += count;

            return new List<string> { "RELOAD" };
        }

        public List<string> HirePirates() =>
            Hire(1);

        public List<string> HirePiratesRandom() =>
            Hire(Game.Dice.Roll());

        public List<string> MasteryTest()
        {
            List<string> test = new List<string>();

            Game.Dice.DoubleRoll(out int first, out int second);

            int dicesSumm = first + second;

            test.Add($"Бросаем кубики: {Game.Dice.Symbol(first)} + " +
                $"{Game.Dice.Symbol(second)} = {dicesSumm}");

            if (dicesSumm < Character.Protagonist.Mastery)
            {
                test.Add("BIG|BOLD|Выпало МЕНЬШЕ твоего мастерства!");
                Game.Buttons.Disable("Больше или равно уровню твоего МАСТЕРСТВА");
            }
            else
            {
                test.Add("BIG|BOLD|Выпало БОЛЬШЕ или равно твоего мастерства!");
                Game.Buttons.Disable("Меньше уровня твоего МАСТЕРСТВА");
            }

            return test;
        }

        public List<string> SlavesByDice()
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

        public List<string> DayByDice()
        {
            List<string> test = new List<string>();

            int dice = Game.Dice.Roll();

            test.Add($"Бросаем кубик: {Game.Dice.Symbol(dice)}");

            int days = dice <= 3 ? 2 : 3;

            test.Add($"BIG|В судовой журнал пишем {days} дня!");
            Character.Protagonist.Logbook += days;

            return test;
        }

        public List<string> PathByDice()
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
