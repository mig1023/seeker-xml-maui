using SeekerMAUI.Game;
using System;
using System.Text.RegularExpressions;

namespace SeekerMAUI.Gamebook.LoneWolf
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public bool Disciplines { get; set; }

        public List<Character> Enemies { get; set; }

        public override List<string> Status() => new List<string>
        {
            $"Боевой навык: {Character.Protagonist.Skill}",
            $"Выносливость: {Character.Protagonist.Strength}/{Character.Protagonist.MaxStrength}",
            $"Монеты: {Character.Protagonist.Gold}",
        };

        public override List<string> Representer()
        {
            if (Disciplines)
                return new List<string> { Head };

            List<string> enemies = new List<string>();

            if (Enemies == null)
                return enemies;

            foreach (Character enemy in Enemies)
                enemies.Add($"{enemy.Name}\nбоевой навык {enemy.Skill}  выносливость {enemy.Strength}");

            return enemies;
        }

        public override bool GameOver(out int toEndParagraph, out string toEndText) =>
            GameOverBy(Character.Protagonist.Strength, out toEndParagraph, out toEndText);

        public override bool IsButtonEnabled(bool secondButton = false)
        {
            if (Disciplines && (Game.Option.IsTriggered(Head) || Character.Protagonist.Disciplines <= 0))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public List<string> Get()
        {
            if (Disciplines)
            {
                Game.Option.Trigger(Head);
                Character.Protagonist.Disciplines -= 1;
            }

            return new List<string> { "RELOAD" };
        }

        public List<string> Random()
        {
            var dice = Game.Dice.Roll(size: 10) - 1;

            foreach (var option in Game.Option.GetTexts())
            {
                if (!option.Contains("—"))
                    continue;

                var regex = new Regex(@"(\d+)\s*—\s*(\d+)");
                var matches = regex.Match(option);
                var parts = matches.Value.Split('—');

                if ((dice < int.Parse(parts[0])) || (dice > int.Parse(parts[1])))
                {
                    Game.Buttons.Disable(option);
                }
            }

            return new List<string> { "BOLD|Таблица случайных чисел", $"BIG|Случайное число: {dice}" };
        }
    }
}
