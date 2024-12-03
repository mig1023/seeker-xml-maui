using System;
using System.Collections.Generic;

namespace SeekerMAUI.Gamebook.RockOfTerror
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public override List<string> Status()
        {
            TimeSpan time = TimeSpan.FromMinutes(Character.Protagonist.Time);

            List<string> statusLines = new List<string> {
                $"Прошедшее время: {time.Hours:d2}:{time.Minutes:d2}" };

            if (Character.Protagonist.MonksHeart != null)
                statusLines.Add($"Сила сердца монаха: {Character.Protagonist.MonksHeart}");

            return statusLines;
        }

        public override bool GameOver(out int toEndParagraph, out string toEndText)
        {
            toEndParagraph = 0;
            toEndText = "Время вышло...";

            return Character.Protagonist.Time >= 720;
        }

        public override bool Availability(string option)
        {
            if (String.IsNullOrEmpty(option))
            {
                return true;
            }
            else if (Game.Services.AvailabilityByСomparison(option))
            {
                return Game.Services.AvailabilityByProperty(Character.Protagonist, option, Constants.Availabilities);
            }
            else
            {
                return AvailabilityTrigger(option.Trim());
            }
        }
    }
}
