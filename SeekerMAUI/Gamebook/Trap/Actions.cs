using System;

namespace SeekerMAUI.Gamebook.Trap
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public override List<string> Status() => new List<string>
        {
            $"Сила: {Character.Protagonist.Strength}",
            $"Ловкость: {Character.Protagonist.Skill}",
            $"Обаяние: {Character.Protagonist.Charm}",
            $"Здоровье: {Character.Protagonist.Hitpoints}",
        };

        public override bool GameOver(out int toEndParagraph, out string toEndText) =>
            GameOverBy(Character.Protagonist.Hitpoints, out toEndParagraph, out toEndText);
    }
}
