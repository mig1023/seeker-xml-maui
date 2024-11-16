using System;

namespace SeekerMAUI.Gamebook.KoshcheisChain
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public override List<string> AdditionalStatus()
        {
            var money = Character.Protagonist.Money;
            var line = Game.Services.CoinsNoun(money, "ейка", "ейки", "еек");

            return new List<string>
            {
                $"Сила: {Character.Protagonist.Strength}/{Character.Protagonist.MaxStrength}",
                $"Экстрасенсорика: {Character.Protagonist.Extrasensory}",
                $"Ловкость: {Character.Protagonist.Skill}",
                $"Богатство: {money} коп{line}",
            };
        }

        public override bool GameOver(out int toEndParagraph, out string toEndText) =>
            GameOverBy(Character.Protagonist.Strength, out toEndParagraph, out toEndText);
    }
}
