using System;

namespace SeekerMAUI.Gamebook.Usurper
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public override List<string> Status() => new List<string>
        {
            $"Влияние: {Character.Protagonist.Influence}",
            $"Здоровье: {Character.Protagonist.Health}",
            $"Лояльность: {Character.Protagonist.Loyalty}",
            $"Стабильность: {Character.Protagonist.Stability}",
        };

        private bool AvailabilityNode(string option)
        {
            if (Game.Services.AvailabilityByСomparison(option))
            {
                return Game.Services.AvailabilityByProperty(Character.Protagonist,
                    option, Constants.Availabilities);
            }

            var isTriggered = Game.Option.IsTriggered(option.Replace("!", String.Empty).Trim());

            if (option.Contains("!"))
            {
                return !isTriggered;
            }
            else
            {
                return isTriggered;
            }
        }

        public override bool Availability(string option)
        {
            if (String.IsNullOrEmpty(option))
            {
                return true;
            }
            else if (option.Contains(","))
            {
                var availability = option
                    .Split(',')
                    .Where(x => !AvailabilityNode(x.Trim()))
                    .Count() == 0;

                return availability;
            }
            else if (option.Contains("|"))
            {
                var availability = option
                   .Split('|')
                   .Where(x => AvailabilityNode(x.Trim()))
                   .Count() > 0;

                return availability;
            }
            else
            {
                return AvailabilityNode(option);
            }
        }
    }
}
