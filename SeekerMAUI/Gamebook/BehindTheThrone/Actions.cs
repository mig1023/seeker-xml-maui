using System;

namespace SeekerMAUI.Gamebook.BehindTheThrone
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public override List<string> Status() => new List<string>
        {
            $"Проворство: {Character.Protagonist.Agility}",
            $"Меткость: {Character.Protagonist.Marksmanship}",
            $"Фехтование: {Character.Protagonist.Swashbuckling}",
            $"Живучесть: {Character.Protagonist.Vitality}",
        };

        public override bool GameOver(out int toEndParagraph, out string toEndText) =>
            GameOverBy(Character.Protagonist.Vitality, out toEndParagraph, out toEndText);
    }
}
