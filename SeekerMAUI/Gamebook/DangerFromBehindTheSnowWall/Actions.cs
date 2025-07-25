﻿using SeekerMAUI.Game;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SeekerMAUI.Gamebook.DangerFromBehindTheSnowWall
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public List<Character> Enemies { get; set; }
        public bool StrengthBonus { get; set; }
        public bool StrengthLossByDices { get; set; }
        public bool StrengthAlreadyLoss { get; set; }
        public bool Difference { get; set; }
        public bool Divided { get; set; }
        public bool Double { get; set; }
        public bool Triple { get; set; }
        public bool DayWounds { get; set; }
        public int DoorBreakNumber { get; set; }

        public override List<string> Status() => new List<string>
        {
            $"Ловкость: {Character.Protagonist.Skill}",
            $"Сила: {Character.Protagonist.Strength}/{Character.Protagonist.MaxStrength}",
            $"Удар: {Character.Protagonist.Damage}",
        };

        public override List<string> AdditionalStatus()
        {
            List<string> statuses = new List<string>
            {
                $"Наблюдательность: {Character.Protagonist.Skill}",
                $"Деньги: {MoneyFormat(Character.Protagonist.Money)}",
                $"Магия: {Character.Protagonist.Magic}",
            };

            if (Character.Protagonist.AthleticShape != null)
                statuses.Add($"Форма: {Character.Protagonist.AthleticShape}");

            return statuses;
        }
            
        private static string MoneyFormat(int ecu) =>
            String.Format("{0:f1}", (double)ecu / 10).TrimEnd('0').TrimEnd(',').Replace(',', '.');

        public override bool GameOver(out int toEndParagraph, out string toEndText) =>
                GameOverBy(Character.Protagonist.Strength, out toEndParagraph, out toEndText);

        public override List<string> Representer()
        {
            List<string> enemies = new List<string>();

            if (Enemies == null)
                return enemies;

            foreach (Character enemy in Enemies)
                enemies.Add($"{enemy.Name}\nловкость {enemy.Skill}  сила {enemy.Strength}  удар {enemy.Damage}");

            return enemies;
        }

        public override bool Availability(string option) =>
            AvailabilityTrigger(option);

        public static bool NoMoreEnemies(List<Character> enemies) =>
            enemies.Where(x => x.Strength > 0).Count() == 0;

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
                        int protagonistRoll = Game.Dice.Roll();
                        protagonistHitStrength = (protagonistRoll * 2) + Character.Protagonist.Skill;

                        fight.Add($"Сила вашей атаки: " +
                            $"{Game.Dice.Symbol(protagonistRoll)} x 2 + " +
                            $"{Character.Protagonist.Skill} = {protagonistHitStrength}");
                    }

                    int enemyRoll = Game.Dice.Roll();
                    int enemyHitStrength = (enemyRoll * 2) + enemy.Skill;

                    fight.Add($"Сила его атаки: " +
                        $"{Game.Dice.Symbol(enemyRoll)} x 2 + " +
                        $"{enemy.Skill} = {enemyHitStrength}");

                    if ((protagonistHitStrength > enemyHitStrength) && !attackAlready)
                    {
                        string points = Game.Services.CoinsNoun(Character.Protagonist.Damage, "очко", "очка", "очков");
                        fight.Add($"GOOD|{enemy.Name} ранен");
                        fight.Add($"Он теряет {Character.Protagonist.Damage} {points} Силы");

                        enemy.Strength -= Character.Protagonist.Damage;

                        if (NoMoreEnemies(FightEnemies))
                            return Win(fight);
                    }
                    else if (protagonistHitStrength > enemyHitStrength)
                    {
                        fight.Add($"BOLD|{enemy.Name} не смог вас ранить");
                    }
                    else if (protagonistHitStrength < enemyHitStrength)
                    {
                        string points = Game.Services.CoinsNoun(enemy.Damage, "очко", "очка", "очков");
                        fight.Add($"BAD|{enemy.Name} ранил вас");
                        fight.Add($"Вы теряете {enemy.Damage} {points} Силы");

                        Character.Protagonist.Strength -= enemy.Damage;

                        if (Character.Protagonist.Strength <= 0)
                            return Fail(fight);
                    }
                    else
                    {
                        fight.Add("BOLD|Ничья в раунде");
                    }

                    attackAlready = true;

                    fight.Add(String.Empty);
                }

                round += 1;
            }
        }

        public List<string> Observation()
        {
            Game.Dice.DoubleRoll(out int firstDice, out int secondDice);

            bool notice = (firstDice + secondDice + Character.Protagonist.Observation) > 10;
            string noticeLine = notice ? ">" : "<=";

            List<string> observationCheck = new List<string> {
                $"Проверка наблюдательности: {Game.Dice.Symbol(firstDice)} + " +
                $"{Game.Dice.Symbol(secondDice)} + {Character.Protagonist.Observation} " +
                $"{noticeLine} 10" };

            observationCheck.Add(notice ? "BIG|GOOD|УСПЕХ :)" : "BIG|BAD|НЕУДАЧА :(");

            Game.Buttons.Disable(notice, "Заметили", "Нет");

            return observationCheck;
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
                "Числа удачи:",
                "BIG|" + Numbers()
            };

            int goodLuck = Game.Dice.Roll();
            bool okResult = Character.Protagonist.Luck[goodLuck];
            string luckLine = okResult ? "не " : String.Empty;

            luckCheck.Add($"Проверка удачи: {Game.Dice.Symbol(goodLuck)} - {luckLine}является Числом Удачи");
            luckCheck.Add(Result(okResult, "УСПЕХ", "НЕУДАЧА"));

            Game.Buttons.Disable(okResult, "Удачливы, Вы удачливы все три попытки из трех", "Не повезло");

            return luckCheck;
        }

        public List<string> SimpleDoubleDice()
        {
            Game.Dice.DoubleRoll(out int diceFirst, out int diceSecond);
            
            int result = diceFirst + diceSecond;
            int diceThird = 0;
            string thirdLine = String.Empty;

            if (Triple)
            {
                diceThird = Game.Dice.Roll();
                result += diceThird;
                thirdLine = $" + {Game.Dice.Symbol(diceThird)}";
            }

            string line = $"{Game.Dice.Symbol(diceFirst)} + " +
                $"{Game.Dice.Symbol(diceSecond)}{thirdLine} = {result}";

            if (StrengthAlreadyLoss)
            {
                Character.Protagonist.Strength -= 2;
            }

            if (StrengthLossByDices)
            {
                Character.Protagonist.Strength -= result;
                string strength = Game.Services.CoinsNoun(result, "СИЛУ", "СИЛЫ", "СИЛ");

                line += $"\nBIG|BAD|BOLD|Вы потеряли {result} {strength}";
            }

            if (Difference)
            {
                if (diceFirst == diceSecond)
                {
                    line += "\nBIG|BOLD|Выпали два одинаковых числа!";
                }
                else if (Math.Abs(diceFirst - diceSecond) == 1)
                {
                    line += "\nBIG|BOLD|Разница между выпавшими числами составляет единицу!";
                }
                else
                {
                    line += "\nBIG|BOLD|Разница между выпавшими числами больше единицы!";
                }
            }

            if (Divided)
            {
                if ((result % 4) == 0)
                {
                    line += "\nBIG|BOLD|GOOD|Делится на 4 без остатка!";
                }
                else
                {
                    line += "\nBIG|BOLD|BAD|Не делится на 4 без остатка!";
                }
            }

            if (StrengthBonus)
            {
                line += $" + {Character.Protagonist.Strength} Сила";
                line += $"\nBIG|BOLD|Итого сумма: {result + Character.Protagonist.Strength}";
            }

            return new List<string> { $"BIG|Кубики: {line}" };
        }

        public List<string> DiceWound() =>
            Wounds.Dice();

        public List<string> ColdDiceWound() =>
            Wounds.ColdDice(DayWounds);

        public List<string> Break()
        {
            bool doorBreak = DoorBreakNumber > 0;
            List<string> breakingLock = new List<string>();
            breakingLock.Add(doorBreak ? "Ломаете дверь:" : "Сбиваете замок:");

            bool succesBreaked = false;

            while (!succesBreaked && (Character.Protagonist.Strength > 0))
            {
                Character.Protagonist.Strength -= 1;

                int dice = Game.Dice.Roll();
                string result = "не получилось...";
                bool lockSuccess = !doorBreak && (dice < 3);
                bool doorSuccess = doorBreak && (dice == DoorBreakNumber);

                if (lockSuccess || doorSuccess)
                {
                    result = "удачно!";
                    succesBreaked = true;
                    Game.Option.Trigger("ЗАМОК СБИТ");
                }

                breakingLock.Add($"Бьёте по замку: {Game.Dice.Symbol(dice)}  - {result}");
                breakingLock.Add("BAD|За эту попытку вы теряете 1 СИЛУ...");
            }

            if (doorBreak)
                breakingLock.Add(Result(succesBreaked, "ДВЕРЬ СЛОМАНА", "ВЫ УБИЛИСЬ ОБ ДВЕРЬ"));
            else
                breakingLock.Add(Result(succesBreaked, "ЗАМОК СБИТ", "ВЫ УБИЛИСЬ ОБ ЗАМОК"));

            return breakingLock;
        }

        public List<string> SkillSum()
        {
            List<string> skillSum = new List<string>();
            int result = 0;

            if (Double)
            {
                Game.Dice.DoubleRoll(out int diceFirst, out int diceSecond);

                result = diceFirst + diceSecond;

                skillSum.Add($"Кубики: {Game.Dice.Symbol(diceFirst)} + " +
                    $"{Game.Dice.Symbol(diceSecond)} = {result}");
            }
            else
            {
                result = Game.Dice.Roll();

                skillSum.Add($"Кубик: {Game.Dice.Symbol(result)}");
            }

            int fullResult = result + Character.Protagonist.Skill;
            skillSum.Add($"Прибавляем Ловкость: {result} + " +
                $"{Character.Protagonist.Skill} = {fullResult}");
            skillSum.Add($"BIG|BOLD|Итоговая сумма: {result}");

            return skillSum;
        }

        public List<string> Dispute()
        {
            List<string> dispute = new List<string>();

            int diceFirst, diceSecond;

            do
            {
                Game.Dice.DoubleRoll(out diceFirst, out diceSecond);

                dispute.Add($"Кубик Собеседника 1: {Game.Dice.Symbol(diceFirst)}");
                dispute.Add($"Кубик Собеседника 2: {Game.Dice.Symbol(diceSecond)}");

                if (diceFirst == diceSecond)
                    dispute.Add($"Спор продолжается...");
            }
            while (diceFirst == diceSecond);

            if (diceFirst > diceSecond)
            {
                dispute.Add("BIG|BOLD|В споре победил Собеседник 1");
                Game.Buttons.Disable("Победил Собеседник 2");
            }
            else
            {
                dispute.Add("BIG|BOLD|В споре победил Собеседник 2");
                Game.Buttons.Disable("Победил Собеседник 1");
            }

            
            return dispute;
        }

        public List<string> Skill()
        {
            int dice = Game.Dice.Roll();
            bool goodSkill = (dice * 3) <= Character.Protagonist.Skill;
            string skillLine = goodSkill ? "<=" : ">";

            List<string> skillCheck = new List<string> {
                $"Проверка ловкости: {Game.Dice.Symbol(dice)} х 3 = " +
                $"{dice * 3} {skillLine} {Character.Protagonist.Skill} ловкость" };

            skillCheck.Add(Result(goodSkill, "ЛОВКОСТИ ХВАТИЛО", "ЛОВКОСТИ НЕ ХВАТИЛО"));

            Game.Buttons.Disable(goodSkill,
                "Сумма оказалась меньше или равна вашей ЛОВКОСТИ", "Оказалась больше");

            return skillCheck;
        }

        public List<string> AthleticShape() =>
            Athletic.Shape();

        public List<string> AthleticBonus() =>
            Athletic.Bonus();

        public List<string> EscapeFromFire()
        {
            List<string> lines = new List<string> { "BIG|Уклоняемся от обстрела:" };
            List<int> inTarget = new List<int> { 1 };

            int shoot = 1;
            int hit = 0;

            while (shoot < 8)
            {
                int dice = Game.Dice.Roll();
                lines.Add($"BOLD|{shoot} выстрел");
                lines.Add($"На кубике выпало: {Game.Dice.Symbol(dice)}");

                shoot += 1;

                if (!inTarget.Contains(dice))
                {
                    lines.Add("GOOD|Промах!");
                }
                else
                {
                    hit += 1;

                    lines.Add("BAD|BIG|Попали!!");

                    if (hit == 1)
                    {
                        Character.Protagonist.Strength -= 3;
                        lines.Add("BAD|Вы теряелете 3 СИЛЫ!");

                        inTarget.Add(2);
                        lines.Add("GRAY|Далее торронги попадают при выпадении на кубике 1 или 2");
                    }
                    else if (hit == 2)
                    {
                        Character.Protagonist.Strength -= 7;
                        lines.Add("BAD|Вы теряелете 7 СИЛ!");

                        inTarget.Add(3);
                        lines.Add("GRAY|Далее торронги попадают при выпадении на кубике 1, 2 или 3");
                    }
                    else
                    {
                        Character.Protagonist.Strength = 0;
                        lines.Add("BAD|BIG|Третье попадание оказалось смертельным...");

                        break;
                    }
                }
            }

            if (hit < 3)
                lines.Add("GOOD|BIG|Вам успешно удалось уйти от стрельбы!");

            return lines;
        }
    }
}
