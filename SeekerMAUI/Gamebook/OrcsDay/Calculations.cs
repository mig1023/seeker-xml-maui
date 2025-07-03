using System;
using System.Collections.Generic;
using System.Text;

namespace SeekerMAUI.Gamebook.OrcsDay
{
    class Calculations
    {
        public static bool Condition(string conditionParam)
        {
            string[] conditions = conditionParam.Split(',');

            foreach (string condition in conditions)
            {
                bool mustBeFalse = condition.Contains("!");
                bool isTriggered = Game.Option.IsTriggered(condition.Replace("!", String.Empty).Trim());

                if (mustBeFalse == isTriggered)
                    return false;
            }

            return true;
        }

        private static string OrcishnessChange(string line)
        {
            if (line.StartsWith("+"))
            {
                Character.Protagonist.Orcishness += 1;
                return $"BAD|{line}";
            }
            else
            {
                Character.Protagonist.Orcishness -= 1;
                return $"GOOD|{line}";
            }
        }

        public static List<string> Orcishness()
        {
            Character orc = Character.Protagonist;

            orc.Orcishness = 6;

            List<string> orcishness = new List<string> { "BOLD|Изначальное значение: 6" };

            if ((orc.Muscle < 0) || (orc.Wits < 0) || (orc.Courage < 0) || (orc.Luck < 0))
                orcishness.Add(OrcishnessChange(Constants.Orcishness["Negative"]));

            if (orc.Wits > orc.Muscle)
                orcishness.Add(OrcishnessChange(Constants.Orcishness["Wits"]));

            if (orc.Luck > 0)
                orcishness.Add(OrcishnessChange(Constants.Orcishness["Luck"]));

            if ((orc.Muscle > orc.Wits) && (orc.Muscle > orc.Courage) || (orc.Muscle > orc.Luck))
                orcishness.Add(OrcishnessChange(Constants.Orcishness["Muscle"]));

            if (orc.Courage > orc.Wits)
                orcishness.Add(OrcishnessChange(Constants.Orcishness["Courage"]));

            if (orc.Courage > 2)
                orcishness.Add(OrcishnessChange(Constants.Orcishness["TooMuch"]));

            orcishness.Add($"BIG|BOLD|Итоговая Оркишность: {orc.Orcishness}");

            return orcishness;
        }

        public static List<string> Overcome()
        {
            List<string> overcome = new List<string> { "BOLD|CЧИТАЕМ:" };

            while (true)
            {
                int sense = Character.Protagonist.Courage + Character.Protagonist.Wits;
                int orcishness = Character.Protagonist.Muscle + Character.Protagonist.Orcishness + 5;

                overcome.Add("BOLD|Борьба:");

                overcome.Add($"С одной стороны: {Character.Protagonist.Courage} (смелость) + " +
                    $"{Character.Protagonist.Wits} (мозги) = {sense}");

                overcome.Add($"С другой: {Character.Protagonist.Muscle} (мышцы) + " +
                    $"{Character.Protagonist.Orcishness} (оркишность) + 5 = {orcishness}");

                overcome.Add($"В результате: {sense} " +
                    $"{Game.Services.Сomparison(sense, orcishness)} {orcishness}");

                if (sense >= orcishness)
                {
                    overcome.Add("BOLD|GOOD|Ты выиграл!");
                    overcome.Add("Оркишность снизилась на единицу!");

                    Character.Protagonist.Orcishness -= 1;

                    if (Character.Protagonist.Orcishness <= 0)
                    {
                        overcome.Add("BIG|GOOD|Ты освободился от своей Оркской природы! :)");
                        overcome.Add("Ты получаешь за это дополнительно 3 единицы Смелости!");

                        Character.Protagonist.Courage += 3;

                        if (Game.Option.IsTriggered("Кандидат в Властелины"))
                        {
                            overcome.Add("\nBOLD|Ты стал Тёмным Властелином!");
                            Game.Option.Trigger("Тёмный Властелин");
                        }

                        return overcome;
                    }
                }
                else
                {
                    overcome.Add("BOLD|BAD|Ты проиграл!");
                    overcome.Add("Смелость снизилась на единицу!");

                    Character.Protagonist.Courage -= 1;

                    if (Character.Protagonist.Courage <= 0)
                    {
                        overcome.Add("BIG|BAD|Ты остался рабом своей Оркской природы :(");
                        return overcome;
                    }
                }
            }
        }
    }
}
