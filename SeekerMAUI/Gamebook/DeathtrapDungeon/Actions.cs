using Microsoft.Maui.Controls.Shapes;
using System;

namespace SeekerMAUI.Gamebook.DeathtrapDungeon
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public List<Character> Enemies { get; set; }

        public override List<string> Status() => new List<string>
        {
            $"Мастерство: {Character.Protagonist.Skill}/{Character.Protagonist.MaxSkill}",
            $"Выносливость: {Character.Protagonist.Strength}/{Character.Protagonist.MaxStrength}",
            $"Удачливость: {Character.Protagonist.Luck}/{Character.Protagonist.MaxLuck}",
        };

        public override bool GameOver(out int toEndParagraph, out string toEndText) =>
            GameOverBy(Character.Protagonist.Strength, out toEndParagraph, out toEndText);

        public override List<string> Representer()
        {
            List<string> enemies = new List<string>();

            if (!String.IsNullOrEmpty(Head))
                return new List<string> { Head };

            if (Enemies == null)
                return enemies;

            foreach (Character enemy in Enemies)
                enemies.Add($"{enemy.Name}\nмастерство {enemy.Skill}  выносливость {enemy.Strength}");

            return enemies;
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

            Game.Buttons.Disable(goodLuck, "Повезло", "Не повезло");

            return luckCheck;
        }

        private static bool NoMoreEnemies(List<Character> enemies) =>
            enemies.Where(x => x.Strength > 0).Count() == 0;

        public List<string> Fight()
        {
            List<string> fight = new List<string>();

            List<Character> fightEnemies = new List<Character>();

            foreach (Character enemy in Enemies)
                fightEnemies.Add(enemy.Clone());

            int round = 1;

            while (true)
            {
                fight.Add($"HEAD|BOLD|Раунд: {round}");

                bool attackAlready = false;
                int protagonistHitStrength = 0;

                foreach (Character enemy in fightEnemies)
                {
                    if (enemy.Strength <= 0)
                        continue;

                    fight.Add($"{enemy.Name} (выносливость {enemy.Strength})");

                    if (!attackAlready)
                    {
                        Game.Dice.DoubleRoll(out int protagonistRollFirst, out int protagonistRollSecond);
                        protagonistHitStrength = protagonistRollFirst + protagonistRollSecond + Character.Protagonist.Skill;

                        fight.Add($"Сила твоего удара: " +
                            $"{Game.Dice.Symbol(protagonistRollFirst)} + " +
                            $"{Game.Dice.Symbol(protagonistRollSecond)} + " +
                            $"{Character.Protagonist.Skill} = {protagonistHitStrength}");
                    }

                    Game.Dice.DoubleRoll(out int enemyRollFirst, out int enemyRollSecond);
                    int enemyHitStrength = enemyRollFirst + enemyRollSecond + enemy.Skill;

                    fight.Add($"Сила его удара: " +
                        $"{Game.Dice.Symbol(enemyRollFirst)} + " +
                        $"{Game.Dice.Symbol(enemyRollSecond)} + " +
                        $"{enemy.Skill} = {enemyHitStrength}");

                    if ((protagonistHitStrength > enemyHitStrength) && !attackAlready)
                    {
                        fight.Add($"GOOD|BOLD|{enemy.Name} ранен");

                        enemy.Strength -= 2;

                        if (fightEnemies.Where(x => x.Strength > 0).Count() == 0)
                        {
                            fight.Add($"BIG|GOOD|BOLD|\nТы ПОБЕДИЛ :)");
                            return fight;
                        }
                    }
                    else if (protagonistHitStrength > enemyHitStrength)
                    {
                        fight.Add($"BOLD|{enemy.Name} не смог тебя ранить");
                    }
                    else if (protagonistHitStrength < enemyHitStrength)
                    {
                        fight.Add($"BAD|BOLD|{enemy.Name} ранил тебя");

                        Character.Protagonist.Strength -= 2;

                        if (Character.Protagonist.Strength <= 0)
                        {
                            fight.Add($"BIG|BAD|BOLD|\nТы ПРОИГРАЛ :(");
                            return fight;
                        }
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
