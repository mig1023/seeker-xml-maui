using SeekerMAUI.Game;
using SeekerMAUI.Gamebook.CreatureOfHavoc;
using System;

namespace SeekerMAUI.Gamebook.LandOfTallGrasses
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public List<Character> Enemies { get; set; }

        public int RoundsToWin { get; set; }

        public override List<string> Status() => new List<string>
        {
            $"Мастерство: {Character.Protagonist.Skill}",
            $"Сила: {Character.Protagonist.Strength}",
            $"Удача: {Character.Protagonist.Luck}",
        };

        public override bool GameOver(out int toEndParagraph, out string toEndText) =>
            GameOverBy(Character.Protagonist.Strength, out toEndParagraph, out toEndText);

        public override List<string> Representer()
        {
            List<string> enemies = new List<string>();

            if (Enemies == null)
                return enemies;

            foreach (Character enemy in Enemies)
                enemies.Add($"{enemy.Name}\nмастерство {enemy.Skill}  сила {enemy.Strength}");

            return enemies;
        }

        public List<string> Luck()
        {
            var luck = new List<string> { "Проверка удачи:" };
            var dice = Game.Dice.Roll();
            luck.Add($"BOLD|BIG|Бросаем кубик: {Game.Dice.Symbol(dice)}");

            if (dice >= Character.Protagonist.Luck)
            {
                luck.Add("Выпавшее значение больше или равно значению удачи!");
                luck.Add("GOOD|BIG|BOLD|Вам повезло! :)");

                Game.Buttons.Disable("Fail");
            }
            else
            {
                luck.Add("Выпавшее значение меньше значения удачи!");
                luck.Add("BAD|BIG|BOLD|Вам НЕ повезло! :(");

                Game.Buttons.Disable("Win");
            }

            return luck;
        }

        private static bool NoMoreEnemies(List<Character> enemies) =>
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

                int protagonistDice = Game.Dice.Roll();

                int protagonistHit = protagonistDice + Character.Protagonist.Skill;

                fight.Add($"Ваш удар: " +
                    $"{Game.Dice.Symbol(protagonistDice)} + " +
                    $"{Character.Protagonist.Skill} = {protagonistHit}");

                EnemyHit max = null, min = null;

                foreach (Character enemy in FightEnemies)
                {
                    if (enemy.Strength <= 0)
                        continue;

                    int enemyDice = Game.Dice.Roll();

                    int enemyHit = enemyDice + enemy.Skill;

                    fight.Add($"{enemy.Name} (сила {enemy.Strength}): " +
                        $"{Game.Dice.Symbol(enemyDice)} + " +
                        $"{enemy.Skill} = {enemyHit}");

                    if ((max == null) || (max.Hit < enemyHit))
                        max = new EnemyHit(enemy.Name, enemyHit, enemy);

                    if ((min == null) || (min.Hit > enemyHit))
                        min = new EnemyHit(enemy.Name, enemyHit, enemy);
                }

                if (protagonistHit > max.Hit)
                {
                    fight.Add($"BOLD|GOOD|{min.Name} ранен");

                    min.Link.Strength -= 3;

                    if (NoMoreEnemies(FightEnemies))
                        return Win(fight);
                }
                else if (protagonistHit < max.Hit)
                {
                    fight.Add($"BOLD|BAD|{max.Name} ранил вас");

                    Character.Protagonist.Strength -= 3;

                    if (Character.Protagonist.Strength <= 0)
                        return Fail(fight);
                }
                else
                {
                    fight.Add("BOLD|Ничья в раунде");
                }

                if ((RoundsToWin > 0) && (RoundsToWin <= round))
                {
                    Character.Protagonist.Strength = 0;

                    fight.Add("BAD|Отведённые на победу раунды истекли...");
                    return Fail(fight);
                }

                fight.Add(String.Empty);

                round += 1;
            }
        }
    }
}
