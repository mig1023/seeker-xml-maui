using System;

namespace SeekerMAUI.Gamebook.Trap
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public string EnemyName = String.Empty;
        public int EnemyAttack = 0;

        public override List<string> Status() => new List<string>
        {
            $"Сила: {Character.Protagonist.Strength}",
            $"Ловкость: {Character.Protagonist.Skill}",
            $"Обаяние: {Character.Protagonist.Charm}",
            $"Здоровье: {Character.Protagonist.Hitpoints}",
        };

        public override bool GameOver(out int toEndParagraph, out string toEndText) =>
            GameOverBy(Character.Protagonist.Hitpoints, out toEndParagraph, out toEndText);

        public override List<string> Representer()
        {
            if (String.IsNullOrEmpty(EnemyName))
            {
                return new List<string>();
            }
            else
            {
                return new List<string> { $"{EnemyName}\nАтака {EnemyAttack}" };
            }
        }


    }
}
