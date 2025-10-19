using System;

namespace SeekerMAUI.Gamebook.KnightOfTheLivingDead
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public List<Character> Enemies { get; set; }

        public override List<string> Status() => new List<string>
        {
            $"Оружие: {Character.Protagonist.Weapon}",
            $"Атака: {Character.Protagonist.Attack}",
            $"Повреждения: {Character.Protagonist.Damage}",
            $"Очки нежизни: {Character.Protagonist.Hitpoints}",
        };

        public override bool GameOver(out int toEndParagraph, out string toEndText) =>
            GameOverBy(Character.Protagonist.Hitpoints, out toEndParagraph, out toEndText);

        public override List<string> Representer()
        {
            List<string> enemies = new List<string>();

            if (Enemies == null)
                return enemies;

            foreach (Character enemy in Enemies)
                enemies.Add($"{enemy.Name}\nатака {enemy.Attack}  повреждения {enemy.Damage}  очки нежизни {enemy.Hitpoints}");

            return enemies;
        }

        public List<string> Fight()
        {
            List<string> fight = new List<string>();

            int round = 1;

            while (true)
            {
                fight.Add($"HEAD|BOLD|Раунд: {round}");

                var attackAlready = false;
                var hit = 0;

                foreach (Character enemy in Enemies)
                {
                    if (enemy.Hitpoints <= 0)
                        continue;

                    fight.Add($"{enemy.Name} (очки нежизни {enemy.Hitpoints})");

                    if (!attackAlready)
                    {
                        fight.Add("BOLD|ТВОЙ УДАР:");

                        attackAlready = true;
                        hit = Dodecahedron.Roll(ref fight);

                        if (hit <= Character.Protagonist.Attack)
                        {
                            fight.Add("Выброшенное значение не превышает уровня атаки, " +
                                $"равное {Character.Protagonist.Attack}!");

                            fight.Add($"GOOD|BOLD|{enemy.Name} ранен!");

                            fight.Add($"{enemy.Name} теряет " +
                                $"{Character.Protagonist.Damage} очков нежизни!");

                            enemy.Hitpoints -= Character.Protagonist.Damage;

                            if (Enemies.Where(x => x.Hitpoints > 0).Count() <= 0)
                                return Win(fight, you: true);
                        }
                        else
                        {
                            fight.Add("Выброшенное значение выше уровня атаки, " +
                                $"равное {Character.Protagonist.Attack}...");

                            fight.Add($"BOLD|Ты промахнулся...");
                        }
                    }

                    fight.Add("BOLD|\nУДАР ПРОТИВНИКА:");
                    hit = Dodecahedron.Roll(ref fight);

                    if (hit <= enemy.Attack)
                    {
                        fight.Add("Выброшенное значение не превышает уровня атаки, " +
                            $"равное {enemy.Attack}");

                        fight.Add($"BAD|BOLD|Ты ранен!");

                        fight.Add($"Ты теряешь " +
                            $"{enemy.Damage} очков нежизни!");

                        Character.Protagonist.Hitpoints -= enemy.Damage;

                        if (Character.Protagonist.Hitpoints <= 0)
                            return Fail(fight, you: true);
                    }
                    else
                    {
                        fight.Add("Выброшенное значение выше уровня атаки, " +
                            $"равное {enemy.Attack}!");

                        fight.Add($"BOLD|Враг промахнулся!");
                    }

                    fight.Add(String.Empty);
                }

                round += 1;
            }
        }
    }
}
