using System;

namespace SeekerMAUI.Gamebook.SherlockHolmes
{
    class Availabilities
    {
        public static bool TriggersString(string triggers)
        {
            return (triggers == "NOE") || (triggers == "QREX") || (triggers == "EOX") || (triggers == "DZS");
        }

        private static Dictionary<string, bool> Parsing(string triggers)
        {
            var triggerDict = triggers
                .Split("")
                .ToDictionary(x => x, x => Game.Option.IsTriggered(x));

            return triggerDict;
        }

        public static bool TriggersCheck(string triggers)
        {
            var trigger = Parsing(triggers);

            if (triggers == "NOE")
            {
                return (trigger["N"] || trigger["O"]) && !trigger["E"];
            }
            else if (triggers == "QREX")
            {
                return (trigger["Q"] || trigger["R"]) && (trigger["E"] || trigger["X"]);
            }
            else if (triggers == "EOX")
            {
                return (trigger["E"] || trigger["O"]) && trigger["X"];
            }
            else if (triggers == "DZS")
            {
                var r19 = Game.Option.IsTriggered("19");
                return (trigger["D"] && trigger["Z"]) || (trigger["S"] && r19);
            }
            else
            {
                return true;
            }
        }
    }
}
