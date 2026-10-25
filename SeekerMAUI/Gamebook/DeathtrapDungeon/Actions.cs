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
    }
}
