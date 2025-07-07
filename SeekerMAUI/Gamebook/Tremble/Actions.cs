﻿using System;

namespace SeekerMAUI.Gamebook.Tremble
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public Character Enemy { get; set; }

        public int RoundsToFight { get; set; }
        public bool WoundsByDice { get; set; }
        public bool HeavyDamageByDice { get; set; }
        public bool LightDamageByDice { get; set; }


        public override List<string> Status() => new List<string>
        {
            $"Ловкость: {Character.Protagonist.Skill}/{Character.Protagonist.MaxSkill}",
            $"Выносливость: {Character.Protagonist.Endurance}/{Character.Protagonist.MaxEndurance}",
            $"Счастье: {Character.Protagonist.Luck}/{Character.Protagonist.MaxLuck}",
        };

        public override List<string> Representer()
        {
            List<string> enemies = new List<string>();

            if (Enemy != null)
            {
                enemies.Add($"{Enemy.Name}\nмастерство {Enemy.Skill}  выносливость {Enemy.Endurance}");
            }

            return enemies;
        }

        public override bool GameOver(out int toEndParagraph, out string toEndText) =>
            GameOverBy(Character.Protagonist.Endurance, out toEndParagraph, out toEndText);

        private List<List<int>> KeysPermutations(List<int> items)
        {
            var result = new List<List<int>>();

            for (int a = 0; a < items.Count; a++)
            {
                for (int b = 0; b < items.Count; b++)
                {
                    if (items[b] == items[a])
                        continue;

                    for (int c = 0; c < items.Count; c++)
                    {
                        if ((items[c] == items[a]) || (items[c] == items[b]))
                            continue;

                        result.Add(new List<int> { items[a], items[b], items[c] });
                    }
                }
            }

            return result;
        }

        public override bool Availability(string option)
        {
            if (String.IsNullOrEmpty(option))
            {
                return true;
            }
            else if (option.StartsWith("КЛЮЧИ"))
            {
                var keys = Character.Protagonist.Keys
                    .Split(' ')
                    .Select(x => int.Parse(x))
                    .ToList();

                var summ = option.Split(' ');
                var optionKeys = int.Parse(summ[1]);

                if (keys.Count < 3)
                {
                    return false;
                }
                else if (keys.Count == 3)
                {
                    return optionKeys == keys.Sum(x => x);
                }
                else
                {
                    foreach (var permutation in KeysPermutations(keys))
                    {
                        if (optionKeys == permutation.Sum(x => x))
                            return true;
                    }

                    return false;
                }
            }
            else if (option.StartsWith("КЛЮЧЕЙ >="))
            {
                var keys = Character.Protagonist.Keys
                    .Split(' ')
                    .Count();

                var count = Game.Services.LevelParse(option);

                return keys >= count;
            }
            else
            {
                return AvailabilityTrigger(option);
            }
        }

        public List<string> GoodLuck() =>
            Dices.Luck();

        public List<string> DiceCheck() =>
            Dices.Check(WoundsByDice, HeavyDamageByDice, LightDamageByDice);

        public List<string> Fight()
        {
            List<string> fight = new List<string>();

            int round = 1;

            while (true)
            {
                fight.Add($"HEAD|BOLD|Раунд: {round}");
                fight.Add($"{Enemy.Name} (выносливость {Enemy.Endurance})");

                Game.Dice.DoubleRoll(out int protagonistRollFirst, out int protagonistRollSecond);

                var protagonistAttack = protagonistRollFirst + 
                    protagonistRollSecond + Character.Protagonist.Skill;

                fight.Add($"Твоя атака: " +
                    $"{Game.Dice.Symbol(protagonistRollFirst)} + " +
                    $"{Game.Dice.Symbol(protagonistRollSecond)} + " +
                    $"{Character.Protagonist.Skill} = {protagonistAttack}");

                Game.Dice.DoubleRoll(out int enemyRollFirst, out int enemyRollSecond);
                int enemyAttack = enemyRollFirst + enemyRollSecond + Enemy.Skill;

                fight.Add($"Атака чудовища: " +
                    $"{Game.Dice.Symbol(enemyRollFirst)} + " +
                    $"{Game.Dice.Symbol(enemyRollSecond)} + " +
                    $"{Enemy.Skill} = {enemyAttack}");

                if (protagonistAttack > enemyAttack)
                {
                    fight.Add($"GOOD|{Enemy.Name} ранен");

                    Enemy.Endurance -= 2;

                    if (Enemy.Endurance <= 0)
                        return Win(fight, you: true);
                }
                else if (protagonistAttack < enemyAttack)
                {
                    fight.Add($"BAD|{Enemy.Name} ранил тебя");

                    Character.Protagonist.Endurance -= 2;

                    if (Character.Protagonist.Endurance <= 0)
                        return Fail(fight, you: true);
                }
                else
                {
                    fight.Add("BOLD|Ничья в раунде");
                }

                if ((RoundsToFight > 0) && (RoundsToFight <= round))
                {
                    fight.Add("BAD|Отведённые на битву раунды истекли.");
                    return Fail(fight, you: true);
                }

                fight.Add(String.Empty);

                round += 1;
            }
        }
    }
}
