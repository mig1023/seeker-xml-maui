using System;
using System.Collections.Generic;
using System.Linq;

namespace SeekerMAUI.Gamebook.UndergroundRoad
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public override List<string> Status() =>
            new List<string> { $"Ранений: {Character.Protagonist.Wounds}" };

        public override bool AvailabilityNode(string option)
        {
            string opt = option.Trim();
            var hero = Character.Protagonist;

            if (opt.Contains("РАНЕН"))
            {
                if ((opt == "!РАНЕН ДВАЖДЫ") && (hero.Wounds > 1))
                {
                    return false;
                }
                else if ((opt == "!РАНЕН") && (hero.Wounds > 0))
                {
                    return false;
                }
                else if ((opt == "РАНЕН ДВАЖДЫ") && (hero.Wounds < 2))
                {
                    return false;
                }
                else if ((opt == "РАНЕН") && (hero.Wounds < 1))
                {
                    return false;
                }

                return true;
            }
            else
            {
                return AvailabilityTrigger(opt);
            }
        }

        public override bool Availability(string option)
        {
            if (String.IsNullOrEmpty(option))
            {
                return true;
            }
            else if (option.Contains("РАНЕН ДВАЖДЫ или НЕТ СЕРДЦА"))
            {
                bool heart = Game.Option.IsTriggered("Голубое каменное сердце");
                bool wounded = Character.Protagonist.Wounds > 1;
                return !heart || wounded;
            }
            else
            {
                return base.Availability(option);
            }
        }
    }
}
