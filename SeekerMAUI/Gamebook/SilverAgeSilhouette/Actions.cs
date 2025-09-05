using System;
using System.Text.RegularExpressions;

namespace SeekerMAUI.Gamebook.SilverAgeSilhouette
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public List<string> Verse() =>
            Character.Protagonist.Verse.Select(x => Regex.Unescape(x)).ToList();

        public List<string> Loyalty()
        {
            var results = new List<string> { "BOLD|CЧИТАЕМ:" };
            var result = 0;

            foreach (string add in Constants.AddLoyalty)
            {
                if (Game.Option.IsTriggered(add))
                {
                    results.Add($"GOOD|+1 за «{add}»");
                    result += 1;
                }
            }

            foreach (string sub in Constants.SubLoyalty)
            {
                if (Game.Option.IsTriggered(sub))
                {
                    results.Add($"BAD|-1 за «{sub}»");
                    result -= 1;
                }
            }

            if (results.Count == 0)
            {
                results.Add("BAD|Даже, как-то, и вспомнить нечего...");
            }

            var line = result < 0 ? "минус " : string.Empty;
            results.Add($"BIG|BOLD|ИТОГО: {line}{result}");

            if (result > 0)
            {
                results.Add("Это положительное число.");
                Game.Buttons.Disable("Отрицательное");
            }
            else
            {
                results.Add("Это " + (result == 0 ? "полный ноль..." : "отрицательное число."));
                Game.Buttons.Disable("Положительное");
            }

            return results;
        }

        public override bool Availability(string option)
        {
            if (String.IsNullOrEmpty(option))
            {
                return true;
            }
            else if (option == "Нет издания или неудачник")
            {
                return Availabilities.SpecialTrigger();
            }
            else if (option.Contains("||"))
            {
                return Availabilities.ExclusiveTrigger(option);
            }
            else if (option.Contains("|"))
            {
                return Availabilities.MultiplesTrigger(option);
            }
            else if (option.Contains("ОЦЕНКА"))
            {
                return Availabilities.Rating(option);
            }
            else
            {
                return AvailabilityTrigger(option);
            }
        }

        
    }
}
