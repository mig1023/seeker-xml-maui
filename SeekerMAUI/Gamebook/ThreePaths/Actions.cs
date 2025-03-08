using System;
using System.Collections.Generic;
using System.Linq;

namespace SeekerMAUI.Gamebook.ThreePaths
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public bool ThisIsSpell { get; set; }

        public override List<string> Status()
        {
            if (Character.Protagonist.Time == null)
                return null;
                
            return new List<string> { $"Время: {Character.Protagonist.Time:d2}:00" };
        }

        public override bool IsButtonEnabled(bool secondButton = false)
        {
            bool bySpellAdd = ThisIsSpell && (Character.Protagonist.SpellSlots <= 0) && !secondButton;
            bool bySpellRemove = ThisIsSpell && !Character.Protagonist.Spells.Contains(Head) && secondButton;

            return !(bySpellAdd || bySpellRemove);
        }

        public List<string> Get()
        {
            Character.Protagonist.Spells.Add(Head);
            Character.Protagonist.SpellSlots -= 1;

            return new List<string> { "RELOAD" };
        }

        public List<string> Decrease()
        {
            Character.Protagonist.Spells.Remove(Head);
            Character.Protagonist.SpellSlots += 1;

            return new List<string> { "RELOAD" };
        }

        public override bool AvailabilityNode(string option)
        {
            if (Game.Services.AvailabilityByСomparison(option))
            {
                var fail = Game.Services.AvailabilityByProperty(Character.Protagonist,
                    option, Constants.Availabilities, onlyFailTrueReturn: true);

                return !fail;
            }
            else if (option.Contains("ЗАКЛЯТИЕ"))
            {
                return Character.Protagonist.Spells.Contains(option.Trim());
            }
            else
            {
                return AvailabilityTrigger(option);
            }
        }

        public override List<string> Representer()
        {
            int count = Character.Protagonist.Spells.Where(x => x == Head).Count();
            string countLine = Game.Services.CoinsNoun(count, "штука", "штуки", "штук");
            string line = count > 0 ? $"\n{count} {countLine}" : String.Empty;

            return new List<string> { $"{Head}{line}" };
        }
    }
}
