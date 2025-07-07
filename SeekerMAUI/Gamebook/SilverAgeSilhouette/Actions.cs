using System;
using System.Text.RegularExpressions;

namespace SeekerMAUI.Gamebook.SilverAgeSilhouette
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public List<string> Verse() =>
            Character.Protagonist.Verse.Select(x => Regex.Unescape(x)).ToList();

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
