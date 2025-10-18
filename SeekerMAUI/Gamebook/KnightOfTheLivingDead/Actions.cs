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
    }
}
