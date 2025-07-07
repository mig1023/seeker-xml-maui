using System;

namespace SeekerMAUI.Gamebook.SilverAgeSilhouette
{
    class Availabilities
    {
        public static bool Rating(string option)
        {
            bool logic = option.Contains("!");

            List<string> ratings = option
                .Replace("!", String.Empty)
                .Split(',')
                .Select(x => x.Trim())
                .ToList();

            foreach (string rating in ratings)
            {
                int level = Game.Services.LevelParse(rating);

                if (rating.Contains("ОЦЕНКА >=") && (level > Character.Protagonist.Rating))
                    return logic;

                if (rating.Contains("ОЦЕНКА <") && (level <= Character.Protagonist.Rating))
                    return logic;
            }

            return !logic;
        }

        public static bool MultiplesTrigger(string option)
        {
            bool triggers = option
                .Split('|')
                .Where(x => Game.Option.IsTriggered(x.Trim()))
                .Count() > 0;

            return triggers;
        }

        public static bool ExclusiveTrigger(string option)
        {
            List<string> triggers = option
                .Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .ToList();

            return Game.Option.IsTriggered(triggers[0]) && !Game.Option.IsTriggered(triggers[1]);
        }

        public static bool SpecialTrigger()
        {
            if (!Game.Option.IsTriggered("Собственное издание"))
            {
                return true;
            }
            else if (Game.Option.IsTriggered("Неудачник"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
