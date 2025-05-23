﻿using Microsoft.Maui.Controls.Shapes;
using SeekerMAUI.Game;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SeekerMAUI.Gamebook.HostagesOfPirateAdmiral
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public int RoundsToWin { get; set; }
        public bool HeroWoundsLimit { get; set; }
        public bool EnemyWoundsLimit { get; set; }
        public int DevastatingAttack { get; set; }
        public bool SkillPenalty { get; set; }
        public bool HitPenalty { get; set; }

        public List<Character> Enemies { get; set; }

        public override List<string> Status() => new List<string>
        {
            $"Ловкость: {Character.Protagonist.Skill}",
            $"Сила: {Character.Protagonist.Strength}/{Character.Protagonist.MaxStrength}",
            $"Обаяние: {Character.Protagonist.Charm}",
        };

        public override bool GameOver(out int toEndParagraph, out string toEndText) =>
            GameOverBy(Character.Protagonist.Strength, out toEndParagraph, out toEndText);

        public override bool Availability(string option)
        {
            if (String.IsNullOrEmpty(option))
            {
                return true;
            }
            else if (Game.Services.AvailabilityByСomparison(option))
            {
                return Game.Services.AvailabilityByProperty(Character.Protagonist,
                    option, Constants.Availabilities);
            }
            else
            {
                return AvailabilityTrigger(option);
            }
        }

        public override List<string> Representer()
        {
            List<string> enemies = new List<string>();

            if (!String.IsNullOrEmpty(Head))
                return new List<string> { Head };

            if (Enemies == null)
                return enemies;

            foreach (Character enemy in Enemies)
                enemies.Add($"{enemy.Name}\nловкость {enemy.Skill}  сила {enemy.Strength}");

            return enemies;
        }


        private static string Numbers()
        {
            string luckListShow = String.Empty;

            for (int i = 1; i < 7; i++)
            {
                string luck = Constants.LuckList[Character.Protagonist.Luck[i] ? i : i + 10];
                luckListShow += $"{luck} ";
            }

            return luckListShow;
        }

        public List<string> Luck()
        {
            List<string> luckCheck = new List<string>
            {
                "Цифры удачи:",
                "BIG|" + Numbers()
            };

            int goodLuck = Game.Dice.Roll();
            bool isLuck = Character.Protagonist.Luck[goodLuck];
            string not = isLuck ? "не " : String.Empty;

            luckCheck.Add($"Проверка удачи: {Game.Dice.Symbol(goodLuck)} - {not}зачёркунтый");

            luckCheck.Add(Result(isLuck, "УСПЕХ", "НЕУДАЧА"));

            Game.Buttons.Disable(isLuck, "Повезло", "Нет");

            Character.Protagonist.Luck[goodLuck] = !Character.Protagonist.Luck[goodLuck];

            return luckCheck;
        }

        public List<string> LuckRecovery()
        {
            List<string> luckRecovery = new List<string> { "Восстановление удачи:" };

            bool success = false;

            for (int i = 1; i < 7; i++)
            {
                if (!Character.Protagonist.Luck[i])
                {
                    luckRecovery.Add($"GOOD|Цифра {i} восстановлена!");
                    Character.Protagonist.Luck[i] = true;
                    success = true;

                    break;
                }
            }

            if (!success)
                luckRecovery.Add("BAD|Все цифры и так счастливые!");

            luckRecovery.Add("Цифры удачи теперь:");
            luckRecovery.Add("BIG|" + Numbers());

            return luckRecovery;
        }

        public List<string> SimpleOneDice()
        {
            var dice = Game.Dice.Roll();
            var isOdd = dice % 2 == 0;
            var line = isOdd ? String.Empty : "НЕ";

            Buttons.Disable(isOdd, "Выпало четное число", "Выпало нечетное");

            return new List<string> {
                $"BIG|Кубик: {Dice.Symbol(dice)}",
                $"BIG|BOLD|Выпало {line}ЧЁТНОЕ число!"
            };
        }

        public List<string> SimpleTwoDice()
        {
            var game = new List<string> { "Игра в кости:" };

            Game.Dice.DoubleRoll(out int firstDice, out int secondDice);

            var protagonist = firstDice + secondDice;

            game.Add("Вы бросаете кубики: " +
                $"{Dice.Symbol(firstDice)} + {Dice.Symbol(secondDice)} = {protagonist}");

            Game.Dice.DoubleRoll(out firstDice, out secondDice);

            var enemy = firstDice + secondDice;

            game.Add("Противник бросает кубики: " +
                $"{Dice.Symbol(firstDice)} + {Dice.Symbol(secondDice)} = {enemy}");

            if (protagonist >= enemy)
            {
                var line = protagonist >= enemy ?
                    "столько же, но будем считать, что больше" : "больше";

                game.Add($"BIG|GOOD|У вас выпало {line} - вы победили! :)");
                Buttons.Disable("Противнику повезло больше");
            }
            else
            {
                game.Add("BIG|BAD|У вас выпало меньше - вы проиграли :(");
                Buttons.Disable("Вы эту партию выиграли");
            }

            return game;
        }

        public List<string> DiceWay()
        {
            var dice = Game.Dice.Roll();

            for (int i = 1; i <= 6; i++)
            {
                if (i != dice)
                    Buttons.Disable(i.ToString());
            }

            return new List<string> {
                $"BIG|BOLD|На кубике выпало: {Dice.Symbol(dice)}",
            };
        }

        public List<string> Break()
        {
            var breakingDoor = new List<string> { "Ломаете дверь:" };

            var succesBreaked = false;

            while (!succesBreaked && (Character.Protagonist.Strength > 0))
            {
                Game.Dice.DoubleRoll(out int firstDice, out int secondDice);

                if (firstDice == secondDice)
                {
                    succesBreaked = true;
                }
                else
                {
                    Character.Protagonist.Strength -= 1;
                }

                var result = succesBreaked ?
                    "удачный, дверь поддалась!" : "неудачный, -1 сила";

                breakingDoor.Add($"Удар: " +
                    $"{Game.Dice.Symbol(firstDice)} + " +
                    $"{Game.Dice.Symbol(secondDice)} = {result}");
            }

            breakingDoor.Add(Result(succesBreaked, "ДВЕРЬ ВЗЛОМАНА", "ВЫ УБИЛИСЬ ОБ ДВЕРЬ"));

            return breakingDoor;
        }

        public List<string> Charm()
        {
            Game.Dice.DoubleRoll(out int firstDice, out int secondDice);
            bool goodCharm = (firstDice + secondDice) <= Character.Protagonist.Charm;
            string charmLine = goodCharm ? "<=" : ">";

            List<string> luckCheck = new List<string> {
                $"Проверка обаяния: " +
                $"{Game.Dice.Symbol(firstDice)} + " +
                $"{Game.Dice.Symbol(secondDice)} " +
                $"{charmLine} {Character.Protagonist.Charm}" };

            if (goodCharm)
            {
                luckCheck.Add("BIG|GOOD|УСПЕХ :)");
                luckCheck.Add("Вы увеличили своё обаяние на единицу");

                Character.Protagonist.Charm += 1;
            }
            else
            {
                luckCheck.Add("BIG|BAD|НЕУДАЧА :(");

                if (Character.Protagonist.Charm > 2)
                {
                    luckCheck.Add("Вы уменьшили своё обаяние на единицу");
                    Character.Protagonist.Charm -= 1;
                }
            }

            return luckCheck;
        }

        public List<string> Skill()
        {
            Game.Dice.DoubleRoll(out int firstDice, out int secondDice);
            bool goodSkill = (firstDice + secondDice) <= Character.Protagonist.Skill;
            string skillLine = goodSkill ? "<=" : ">";

            List<string> luckCheck = new List<string> {
                $"Проверка ловкости: " +
                $"{Game.Dice.Symbol(firstDice)} + " +
                $"{Game.Dice.Symbol(secondDice)} " +
                $"{skillLine} {Character.Protagonist.Skill}" };

            luckCheck.Add(goodSkill ? "BIG|GOOD|УСПЕХ :)" : "BIG|BAD|НЕУДАЧА :(");

            return luckCheck;
        }

        public List<string> DiceWounds()
        {
            List<string> diceCheck = new List<string> { };

            int dices = 0;

            for (int i = 1; i <= 2; i++)
            {
                int dice = Game.Dice.Roll();
                dices += dice;
                diceCheck.Add($"На {i} выпало: {Game.Dice.Symbol(dice)}");
            }

            Character.Protagonist.Strength -= dices;

            diceCheck.Add($"BIG|BAD|Вы потеряли жизней: {dices}");

            return diceCheck;
        }

        private static bool NoMoreEnemies(List<Character> enemies, bool EnemyWoundsLimit) =>
            enemies.Where(x => x.Strength > (EnemyWoundsLimit ? 2 : 0)).Count() == 0;

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
                    if (enemy.Strength <= 0)
                        continue;

                    fight.Add($"{enemy.Name} (сила {enemy.Strength})");

                    if (!attackAlready)
                    {
                        Game.Dice.DoubleRoll(out int protagonistRollFirst, out int protagonistRollSecond);

                        int protagonistSkill = Character.Protagonist.Skill;
                        
                        if (SkillPenalty)
                        {
                            protagonistSkill -= 1;
                            fight.Add($"GRAY|Ваша ловкость снижена на 1 и равна {protagonistSkill}");
                        }

                        protagonistHitStrength = protagonistRollFirst + protagonistRollSecond + protagonistSkill;

                        fight.Add($"Мощность вашего удара: " +
                            $"{Game.Dice.Symbol(protagonistRollFirst)} + " +
                            $"{Game.Dice.Symbol(protagonistRollSecond)} + " +
                            $"{protagonistSkill} = {protagonistHitStrength}");

                        if (Game.Option.IsTriggered("без ножа"))
                        {
                            protagonistHitStrength -= 1;

                            fight.Add($"GRAY|Из-за отсутствия ножа, " +
                                $"мощность удара снижена на 1 и равна {protagonistHitStrength}");
                        }

                        if (HitPenalty)
                        {
                            protagonistHitStrength -= 1;
                            fight.Add($"GRAY|Ваша мощность удара снижена на 1 и равна {protagonistHitStrength}");
                        }
                    }

                    Game.Dice.DoubleRoll(out int enemyRollFirst, out int enemyRollSecond);
                    int enemyHitStrength = enemyRollFirst + enemyRollSecond + enemy.Skill;

                    fight.Add($"Мощность его удара: " +
                        $"{Game.Dice.Symbol(enemyRollFirst)} + " +
                        $"{Game.Dice.Symbol(enemyRollSecond)} + " +
                        $"{enemy.Skill} = {enemyHitStrength}");

                    if ((protagonistHitStrength > enemyHitStrength) && !attackAlready)
                    {
                        fight.Add($"GOOD|{enemy.Name} ранен");

                        enemy.Strength -= 2;

                        bool enemyLost = NoMoreEnemies(FightEnemies, EnemyWoundsLimit);

                        if (enemyLost)
                            return Win(fight);
                    }
                    else if (protagonistHitStrength > enemyHitStrength)
                    {
                        fight.Add($"BOLD|{enemy.Name} не смог вас ранить");
                    }
                    else if (protagonistHitStrength < enemyHitStrength)
                    {
                        fight.Add($"BAD|{enemy.Name} ранил вас");

                        Character.Protagonist.Strength -= (DevastatingAttack > 0 ? DevastatingAttack : 2);

                        var failByLimit = HeroWoundsLimit && (Character.Protagonist.Strength <= 2);

                        if ((Character.Protagonist.Strength <= 0) || failByLimit)
                            return Fail(fight);
                    }
                    else
                    {
                        fight.Add("BOLD|Ничья в раунде");
                    }

                    attackAlready = true;

                    if ((RoundsToWin > 0) && (RoundsToWin <= round))
                    {
                        fight.Add("BAD|Отведённые на победу раунды истекли.");
                        return Fail(fight);
                    }

                    fight.Add(String.Empty);
                }

                round += 1;
            }
        }

        public override bool IsHealingEnabled() =>
            Character.Protagonist.Strength < Character.Protagonist.MaxStrength;

        public override void UseHealing(int healingLevel)
        {
            if (healingLevel < 0)
                Character.Protagonist.Strength = Character.Protagonist.MaxStrength;
            else
                Character.Protagonist.Strength += healingLevel;
        }
    }
}
