using System;

namespace SeekerMAUI.Gamebook.OminousPaths
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public List<Character> Enemies { get; set; }

        public bool HeroWoundsLimit { get; set; }
        public bool EnemyWoundsLimit { get; set; }

        public override List<string> Status() => new List<string>
        {
            $"Ловкость: {Character.Protagonist.Skill}",
            $"Сила: {Character.Protagonist.Strength}",
            $"Обаяние: {Character.Protagonist.Charm}",
        };

        public override bool GameOver(out int toEndParagraph, out string toEndText) =>
            GameOverBy(Character.Protagonist.Strength, out toEndParagraph, out toEndText);

        public override List<string> Representer()
        {
            var enemies = new List<string>();

            if (!String.IsNullOrEmpty(Head))
            {
                return new List<string> { Head };
            }

            if (Enemies == null)
                return enemies;

            foreach (Character enemy in Enemies)
            {
                var line = $"{enemy.Name}\nловкость {enemy.Skill}  сила {enemy.Strength}";
                enemies.Add(line);
            }

            return enemies;
        }

        public List<string> Luck()
        {
            var luckCheck = new List<string>
            {
                "Цифры удачи:",
                "BIG|" + Luckiness.Numbers()
            };

            var goodLuck = Game.Dice.Roll();
            var isLuck = Character.Protagonist.Luck[goodLuck];
            var not = isLuck ? "не " : String.Empty;

            luckCheck.Add($"Проверка удачи: {Game.Dice.Symbol(goodLuck)} - {not}зачёркунтый");
            luckCheck.Add(Result(isLuck, "УСПЕХ", "НЕУДАЧА"));

            Game.Buttons.Disable(isLuck, "Повезло", "Не повезло");

            Character.Protagonist.Luck[goodLuck] = !Character.Protagonist.Luck[goodLuck];

            return luckCheck;
        }

        public List<string> LuckRecovery()
        {
            var luckRecovery = new List<string> { "Восстановление удачи:" };

            var success = Luckiness.Recovery(luckRecovery);

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
            var goodCharm = (firstDice + secondDice) <= Character.Protagonist.Charm;
            var charmLine = goodCharm ? "<=" : ">";

            var luckCheck = new List<string> { $"Проверка обаяния: " +
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
            var goodSkill = (firstDice + secondDice) <= Character.Protagonist.Skill;
            var skillLine = goodSkill ? "<=" : ">";

            var skillCheck = new List<string> { $"Проверка ловкости: " +
                $"{Game.Dice.Symbol(firstDice)} + " +
                $"{Game.Dice.Symbol(secondDice)} {skillLine} " +
                $"{Character.Protagonist.Skill}" };

            skillCheck.Add(Result(goodSkill, "УСПЕХ", "НЕУДАЧА"));

            return skillCheck;
        }

        public List<string> Strength()
        {
            Game.Dice.DoubleRoll(out int firstDice, out int secondDice);
            var goodStrength = (firstDice + secondDice) <= Character.Protagonist.Strength;
            var skillLine = goodStrength ? "<=" : ">";

            var strengthCheck = new List<string> { $"Проверка силы: " +
                $"{Game.Dice.Symbol(firstDice)} + " +
                $"{Game.Dice.Symbol(secondDice)} {skillLine} " +
                $"{Character.Protagonist.Strength}" };

            strengthCheck.Add(Result(goodStrength, "УСПЕХ", "НЕУДАЧА"));

            return strengthCheck;
        }

        public List<string> Accuracy()
        {
            Game.Dice.DoubleRoll(out int firstDice, out int secondDice);

            var accuracyCheck = new List<string>();

            var accuracy = 19 - Character.Protagonist.Skill;
            accuracyCheck.Add($"Текущая меткость: 19 - Ловскость ({Character.Protagonist.Skill}) = {accuracy}");

            var goodAccuracy = (firstDice + secondDice) <= accuracy;
            var accuracyLine = goodAccuracy ? "<=" : ">";

            accuracyCheck.Add($"Проверка меткости: " +
                $"{Game.Dice.Symbol(firstDice)} + " +
                $"{Game.Dice.Symbol(secondDice)} {accuracyLine} " +
                $"{accuracy}");

            accuracyCheck.Add(Result(goodAccuracy, "ТОЧНО В ЦЕЛЬ!", "ПРОМАХ..."));

            return accuracyCheck;
        }

        private static bool NoMoreEnemies(List<Character> enemies, bool EnemyWoundsLimit) =>
            enemies.Where(x => x.Strength > (EnemyWoundsLimit ? 2 : 0)).Count() == 0;

        public List<string> Fight()
        {
            var fight = new List<string>();

            var FightEnemies = new List<Character>();

            foreach (Character enemy in Enemies)
                FightEnemies.Add(enemy.Clone());

            var round = 1;

            while (true)
            {
                fight.Add($"HEAD|BOLD|Раунд: {round}");

                var attackAlready = false;
                var protagonistHitStrength = 0;

                foreach (Character enemy in FightEnemies)
                {
                    if (enemy.Strength <= 0)
                        continue;

                    fight.Add($"{enemy.Name} (сила {enemy.Strength})");

                    if (!attackAlready)
                    {
                        Game.Dice.DoubleRoll(out int protagonistRollFirst, out int protagonistRollSecond);
                        int protagonistSkill = Character.Protagonist.Skill;
                        protagonistHitStrength = protagonistRollFirst + protagonistRollSecond + protagonistSkill;

                        fight.Add($"Мощность вашего удара: " +
                            $"{Game.Dice.Symbol(protagonistRollFirst)} + " +
                            $"{Game.Dice.Symbol(protagonistRollSecond)} + " +
                            $"{protagonistSkill} = {protagonistHitStrength}");
                    }

                    Game.Dice.DoubleRoll(out int enemyRollFirst, out int enemyRollSecond);
                    var enemyHitStrength = enemyRollFirst + enemyRollSecond + enemy.Skill;

                    fight.Add($"Мощность его удара: " +
                        $"{Game.Dice.Symbol(enemyRollFirst)} + " +
                        $"{Game.Dice.Symbol(enemyRollSecond)} + " +
                        $"{enemy.Skill} = {enemyHitStrength}");

                    if ((protagonistHitStrength > enemyHitStrength) && !attackAlready)
                    {
                        fight.Add($"GOOD|{enemy.Name} ранен");

                        enemy.Strength -= 2;

                        var enemyLost = NoMoreEnemies(FightEnemies, EnemyWoundsLimit);

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

                        Character.Protagonist.Strength -= 2;

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
    }
}
