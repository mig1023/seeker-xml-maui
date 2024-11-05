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
    }
}
