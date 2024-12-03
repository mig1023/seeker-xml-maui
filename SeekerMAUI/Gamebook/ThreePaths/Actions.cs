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

        public override bool Availability(string option)
        {
            if (String.IsNullOrEmpty(option))
                return true;

            foreach (string oneOption in option.Split(','))
            {
                if (Game.Services.AvailabilityByСomparison(oneOption))
                {
                    var fail = Game.Services.AvailabilityByProperty(Character.Protagonist,
                        oneOption, Constants.Availabilities, onlyFailTrueReturn: true);

                    if (fail)
                        return false;
                }
                else if (oneOption.Contains("ЗАКЛЯТИЕ"))
                {
                    return Character.Protagonist.Spells.Contains(oneOption.Trim());
                }
                else if (oneOption.Contains("!"))
                {
                    if (Game.Option.IsTriggered(oneOption.Replace("!", String.Empty).Trim()))
                        return false;
                }
                else if (!Game.Option.IsTriggered(oneOption.Trim()))
                {
                    return false;
                }
            }

            return true;
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
