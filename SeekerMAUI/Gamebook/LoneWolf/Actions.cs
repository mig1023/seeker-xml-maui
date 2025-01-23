using System;

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
            if (Disciplines && Game.Option.IsTriggered(Head))
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
            }

            return new List<string> { "RELOAD" };
        }
    }
}
