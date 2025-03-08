using System;
using System.Collections.Generic;
using System.Linq;

namespace SeekerMAUI.Gamebook.ByTheWillOfRome
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public override List<string> Status()
        {
            if (Game.Data.CurrentParagraphID < Constants.AddonStartParagraph)
            {
                return new List<string>
                {
                    $"Сестерциев: {Character.Protagonist.Sestertius}",
                    $"Честь: {Character.Protagonist.Honor}",
                };
            }
            else
            {
                return null;
            }
        }           

        private string Squad(string symbol, int size)
        {
            string legionaries = new string('x', size).Replace("x", symbol);
            string squad = String.IsNullOrEmpty(legionaries) ? "ни одного" : legionaries;

            return squad;
        }

        public override List<string> AdditionalStatus()
        {
            if (Character.Protagonist.Legionaries > 0)
            {
                string legioner = Character.Protagonist.Discipline >= 0 ? "🙂" : "😡";

                return new List<string>
                {
                    $"Легионеров: {Squad(legioner, Character.Protagonist.Legionaries)}",
                    $"Дисциплина: {Game.Services.NegativeMeaning(Character.Protagonist.Discipline)}",
                };
            }
            else if (Character.Protagonist.Horsemen > 0)
            {
                return new List<string>
                {
                    $"Всадников: {Squad("🐎", Character.Protagonist.Horsemen)}",
                    $"Навыки рукопашного боя: 2",
                };
            }
            else
            {
                return null;
            }
        }

        public override List<string> Representer()
        {
            if (Price > 0)
            {
                string gold = Game.Services.CoinsNoun(Price, "сестерций", "сестерция", "сестерциев");
                return new List<string> { $"{Head}\n{Price} {gold}" };
            }
            else
            {
                return new List<string> { };
            }
        }

        public override bool GameOver(out int toEndParagraph, out string toEndText)
        {
            toEndParagraph = 0;
            toEndText = "Ущерб чести слишком велик, лучше броситься на меч, а игру начать сначала";

            return Character.Protagonist.Honor <= 0;
        }

        public override bool IsButtonEnabled(bool secondButton = false) =>
            !(Used || ((Price > 0) && (Character.Protagonist.Sestertius < Price)));

        public override bool AvailabilityNode(string option)
        {
            if (Game.Services.AvailabilityByСomparison(option))
            {
                var fail = Game.Services.AvailabilityByProperty(Character.Protagonist,
                    option, Constants.Availabilities, onlyFailTrueReturn: true);

                return !fail;
            }
            else
            {
                return AvailabilityTrigger(option);
            }
        }

        public List<string> Get()
        {
            Character.Protagonist.Sestertius -= Price;

            Used = true;

            return new List<string> { "RELOAD" };
        }
    }
}