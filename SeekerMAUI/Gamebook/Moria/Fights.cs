using System;

namespace SeekerMAUI.Gamebook.Moria
{
    class Fights
    {
        public static List<string> StrongWarriorsInFellowship() =>
            Character.Protagonist.Fellowship.Where(x => Constants.Fellowship[x] > 3).ToList();

        public static bool IsStillSomeoneToFight(Actions actions) =>
            (actions.Enemies.Count > 0) && (Character.Protagonist.Fellowship.Count > 0);

        public static int EnemiesForEach(Actions actions, int count)
        {
            int countForEach = actions.Enemies.Count / count;
            return countForEach > 0 ? countForEach : 1;
        }

        private static void DeathInFight(ref List<string> fight, string hero)
        {
            fight.Add($"BAD|BOLD|{hero} погиб в бою!");
            fight.Add(String.Empty);
            Character.Protagonist.Fellowship.Remove(hero);
        }

        public static void Part(Actions actions,
            ref List<string> fight, string hero, int count)
        {
            if (!IsStillSomeoneToFight(actions))
                return;

            int frags = 0;
            int lastDice = 0;
            bool secondRound = false;
            string enemy = actions.Enemies[0];

            fight.Add($"BOLD|{hero} сражается против {actions.Declination(enemy, count)}");

            while (frags < count)
            {
                int strength = Constants.Fellowship[hero];
                int dice = Game.Dice.Roll();
                int heroAttack = strength + dice;

                fight.Add($"{hero}: {strength} Сила + {Game.Dice.Symbol(dice)} = {heroAttack}");

                if ((dice < lastDice) && !secondRound)
                {
                    fight.Add($"{hero} выкинул кубик меньше, чем с предудщим врагом - к сожалению, это смертельно...");
                    DeathInFight(ref fight, hero);
                    return;
                }

                lastDice = dice;
                secondRound = false;

                strength = Constants.Enemies[enemy];
                dice = Game.Dice.Roll();
                int enemyAttack = strength + dice;

                fight.Add($"{enemy}: {strength} Сила + {Game.Dice.Symbol(dice)} = {enemyAttack}");

                if (heroAttack > enemyAttack)
                {
                    fight.Add($"GOOD|BOLD|{hero} победил!");
                    actions.Enemies.Remove(enemy);
                    frags += 1;
                }
                else if (heroAttack < enemyAttack)
                {
                    DeathInFight(ref fight, hero);
                    return;
                }
                else
                {
                    fight.Add("BOLD|Ничья! Силы противников равны! Сейчас они сойдутся ещё раз!");
                    secondRound = true;
                }
            }

            fight.Add(String.Empty);
        }
    }
}
