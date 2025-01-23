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

        public override bool GameOver(out int toEndParagraph, out string toEndText) =>
            GameOverBy(Character.Protagonist.Strength, out toEndParagraph, out toEndText);

        public override bool IsButtonEnabled(bool secondButton = false)
        {
            if (Disciplines && Game.Option.IsTriggered(this.Button))
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
                Game.Option.Trigger(this.Button);
            }

            return new List<string> { "RELOAD" };
        }
    }
}
