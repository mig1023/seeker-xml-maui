﻿using System;

namespace SeekerMAUI.Gamebook.PrairieLaw
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public List<Character> Enemies { get; set; }
        public bool Firefight { get; set; }
        public bool HeroWoundsLimit { get; set; }
        public bool EnemyWoundsLimit { get; set; }

        public int Dices { get; set; }
        public bool Roulette { get; set; }
        public string SellPrices { get; set; }
        public string Untrigger { get; set; }

        public override List<string> Status() => new List<string>
        {
            $"Ловкость: {Character.Protagonist.Skill}",
            $"Сила: {Character.Protagonist.Strength}/{Character.Protagonist.MaxStrength}",
            $"Обаяние: {Character.Protagonist.Charm}",
        };

        public override List<string> AdditionalStatus() => new List<string>
        {
            $"Долларов: {ToDollars(Character.Protagonist.Cents)}",
            $"Патронов: {Character.Protagonist.Cartridges}",
        };

        private static string ToDollars(int cents)
        {
            double dollars = (double)cents / 100;
            return $"{dollars:f2}".Replace(',', '.');
        }

        public override bool GameOver(out int toEndParagraph, out string toEndText) =>
            GameOverBy(Character.Protagonist.Strength, out toEndParagraph, out toEndText);

        public override List<string> Representer()
        {
            List<string> enemies = new List<string>();

            if (Price > 0)
            {
                return new List<string> { $"{Head}\n{ToDollars(Price)}$" };
            }
            else if (!String.IsNullOrEmpty(Head))
            {
                return new List<string> { Head };
            }

            if (Enemies == null)
                return enemies;

            foreach (Character enemy in Enemies)
            {
                string line = $"{enemy.Name}\nловкость {enemy.Skill}  сила {enemy.Strength}";

                if (Firefight)
                    line += $"  патроны {enemy.Cartridges}";

                enemies.Add(line);
            }

            return enemies;
        }

        public override bool AvailabilityNode(string option)
        {
            if (Game.Services.AvailabilityByСomparison(option))
            {
                int level = Game.Services.LevelParse(option);

                if (option.Contains("ШКУР >=") && (level > Character.Protagonist.AnimalSkins.Count))
                {
                    return false;
                }
                else if (Game.Services.AvailabilityByСomparison(option))
                {
                    var fail = Game.Services.AvailabilityByProperty(Character.Protagonist,
                        option, Constants.Availabilities, onlyFailTrueReturn: true);

                    return !fail;
                }

                return true;
            }
            else
            {
                return AvailabilityTrigger(option);
            }
        }

        public List<string> Luck()
        {
            List<string> luckCheck = new List<string>
            {
                "Цифры удачи:",
                "BIG|" + Luckiness.Numbers()
            };

            int goodLuck = Game.Dice.Roll();
            bool isLuck = Character.Protagonist.Luck[goodLuck];
            string not = isLuck ? "не " : String.Empty;

            luckCheck.Add($"Проверка удачи: {Game.Dice.Symbol(goodLuck)} - {not}зачёркунтый");
            luckCheck.Add(Result(isLuck, "УСПЕХ", "НЕУДАЧА"));

            Game.Buttons.Disable(isLuck, "Повезло", "Не повезло");

            Character.Protagonist.Luck[goodLuck] = !Character.Protagonist.Luck[goodLuck];

            return luckCheck;
        }

        public List<string> LuckRecovery()
        {
            List<string> luckRecovery = new List<string> { "Восстановление удачи:" };

            bool success = Luckiness.Recovery(luckRecovery);

            if (!success)
            {
                luckRecovery.Add("BAD|Все цифры и так счастливые!");
            }

            luckRecovery.Add("Цифры удачи теперь:");
            luckRecovery.Add("BIG|" + Luckiness.Numbers());

            return luckRecovery;
        }

        public List<string> Charm()
        {
            Game.Dice.DoubleRoll(out int firstDice, out int secondDice);
            bool goodCharm = (firstDice + secondDice) <= Character.Protagonist.Charm;
            string charmLine = goodCharm ? "<=" : ">";

            List<string> luckCheck = new List<string> { $"Проверка обаяния: " +
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

            List<string> luckCheck = new List<string> { $"Проверка ловкости: " +
                $"{Game.Dice.Symbol(firstDice)} + " +
                $"{Game.Dice.Symbol(secondDice)} {skillLine} " +
                $"{Character.Protagonist.Skill}" };

            luckCheck.Add(Result(goodSkill, "УСПЕХ", "НЕУДАЧА"));

            return luckCheck;
        }

        public List<string> DiceWounds() =>
            Dice.Wounds(Dices);

        public override bool IsButtonEnabled(bool secondButton = false)
        {
            bool byUsed = (Price > 0) && Used;
            bool byPrice = (Price > 0) && (Character.Protagonist.Cents < Price);
            bool bySkins = (Type == "SellSkins") && (Character.Protagonist.AnimalSkins.Count == 0);
            bool byNuggets = (Type == "SellNuggets") && (Character.Protagonist.Nuggets == 0);
            bool byGame = Roulette && (Character.Protagonist.Cents < 100);

            return !(byUsed || byPrice || bySkins || byNuggets || byGame);
        }

        public List<string> Get()
        {
            if (!String.IsNullOrEmpty(Untrigger))
                Game.Option.Trigger(Untrigger, remove: true);

            if ((Price > 0) && (Character.Protagonist.Cents >= Price))
            {
                Character.Protagonist.Cents -= Price;

                if (!Multiple)
                    Used = true;

                if (Benefit != null)
                    Benefit.Do();
            }

            return new List<string> { "RELOAD" };
        }

        public List<string> SellSkins()
        {
            List<string> salesReport = new List<string>();

            int cents = 0, sold = 0, index = 0;

            Dictionary<string, int> prices = new Dictionary<string, int>();
            List<int> saledIndexes = new List<int>();

            foreach(string price in SellPrices.Split(',').ToList())
            {
                string[] valuePrice = price.Split('=');
                prices.Add(valuePrice[0].Trim(), int.Parse(valuePrice[1]));
            }

            bool anySkin = prices.ContainsKey("Любая шкура");

            foreach (string skin in Character.Protagonist.AnimalSkins)
            {
                if (prices.ContainsKey(skin) || anySkin)
                {
                    int price = (anySkin ? prices["Любая шкура"] : prices[skin]);

                    salesReport.Add($"{skin} - купил за {ToDollars(price)}$");
                    cents += price;
                    saledIndexes.Add(index);
                    sold += 1;
                }
                else
                {
                    salesReport.Add($"{skin} - её не купит");
                }

                index += 1;
            }

            saledIndexes.Reverse();

            foreach (int removeIndex in saledIndexes)
                Character.Protagonist.AnimalSkins.RemoveAt(removeIndex);

            salesReport.Add(String.Empty);
            salesReport.Add("BIG|ИТОГО:");
            salesReport.Add($"Вы продали шкур: {sold}");
            salesReport.Add($"GOOD|Вы получили: {ToDollars(cents)}$");

            Character.Protagonist.Cents += cents;

            return salesReport;
        }
        
        public List<string> SellNuggets()
        {
            List<string> salesReport = new List<string>();

            int price = int.Parse(SellPrices);
            int cents = Character.Protagonist.Nuggets * price;
            Character.Protagonist.Cents += cents;

            salesReport.Add($"Вы продали самородков: {Character.Protagonist.Nuggets}");
            salesReport.Add($"Цена за один: {ToDollars(price)}$");
            salesReport.Add($"GOOD|Вы получили: {ToDollars(cents)}$");

            Character.Protagonist.Nuggets = 0;

            return salesReport;
        }

        public List<string> RedOrBlackGame() =>
            Games.RedOrBlack();

        public List<string> DoubleGame() =>
            Games.Double();

        public List<string> DiceLtlGame() =>
            Games.Ltl();

        public List<string> DiceGame() =>
            Games.Big();

        public List<string> Fight()
        {
            List<string> fight = new List<string>();

            List<Character> FightEnemies = new List<Character>();

            foreach (Character enemy in Enemies)
                FightEnemies.Add(enemy.Clone());

            int round = 1;
            bool firefight = Firefight;

            while (true)
            {
                firefight = Fights.FirefightContinue(FightEnemies, ref fight, firefight);

                fight.Add($"HEAD|BOLD|Раунд: {round}");

                bool attackAlready = false;
                int protagonistHitStrength = 0, enemyHitStrength = 0;

                foreach (Character enemy in FightEnemies)
                {
                    if (enemy.Strength <= 0)
                        continue;

                    string cartLine = enemy.Cartridges > 0 ? $", патронов {enemy.Cartridges}" : String.Empty;
                    fight.Add($"{enemy.Name} (сила {enemy.Strength}{cartLine})");

                    bool noCartridges = Character.Protagonist.Cartridges <= 0;

                    if (!attackAlready && (!firefight || !noCartridges))
                    {
                        Game.Dice.DoubleRoll(out int protagonistRollFirst, out int protagonistRollSecond);
                        protagonistHitStrength = protagonistRollFirst + protagonistRollSecond + Character.Protagonist.Skill;

                        string protagonistHitLine = (firefight ? "Ваш выстрел" : "Мощность вашего удара");

                        fight.Add($"{protagonistHitLine}: " +
                            $"{Game.Dice.Symbol(protagonistRollFirst)} + " +
                            $"{Game.Dice.Symbol(protagonistRollSecond)} + " +
                            $"{Character.Protagonist.Skill} = {protagonistHitStrength}");

                        if (firefight)
                            Character.Protagonist.Cartridges -= 1;
                    }

                    if (!firefight || (enemy.Cartridges > 0))
                    {
                        Game.Dice.DoubleRoll(out int enemyRollFirst, out int enemyRollSecond);
                        enemyHitStrength = enemyRollFirst + enemyRollSecond + enemy.Skill;

                        string enemyHitLine = (firefight ? "Его выстрел" : "Мощность его удара");

                        fight.Add($"{enemyHitLine}: " +
                            $"{Game.Dice.Symbol(enemyRollFirst)} + " +
                            $"{Game.Dice.Symbol(enemyRollSecond)} + " +
                            $"{enemy.Skill} = {enemyHitStrength}");

                        if (firefight)
                            enemy.Cartridges -= 1;
                    }
                    else
                    {
                        enemyHitStrength = 0;
                    }

                    if ((protagonistHitStrength == 0) && (enemyHitStrength == 0))
                    { 
                        // nothing to do here
                    }
                    else if ((protagonistHitStrength > enemyHitStrength) && !attackAlready)
                    {
                        fight.Add($"GOOD|{enemy.Name} ранен");

                        enemy.Strength -= (firefight ? 3 : 2);

                        bool enemyLost = Fights.NoMoreEnemies(FightEnemies, EnemyWoundsLimit);

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

                        Character.Protagonist.Strength -= (firefight ? 3 : 2);

                        var failByLimit = HeroWoundsLimit && (Character.Protagonist.Strength <= 2);

                        if ((Character.Protagonist.Strength <= 0) || failByLimit)
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

        public override bool IsHealingEnabled() =>
            Character.Protagonist.Strength < Character.Protagonist.MaxStrength;

        public override void UseHealing(int healingLevel)
        {
            if (healingLevel == -1)
            {
                Character.Protagonist.Strength = Character.Protagonist.MaxStrength;
            }
            else
            {
                Character.Protagonist.Strength += healingLevel;
            }
        }
    }
}
