using System;

namespace SeekerMAUI.Gamebook.NoYourGrace
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        private bool AvailabilityNode(string option)
        {
            if (Game.Services.AvailabilityByСomparison(option))
            {
                return Game.Services.AvailabilityByProperty(Character.Protagonist,
                    option, Constants.Availabilities);
            }

            return AvailabilityTrigger(option);
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

        public List<string> VictoryPoints()
        {
            var results = new List<string> { "BOLD|CЧИТАЕМ ПОБЕДНЫЕ ОЧКИ:" };
            var result = 0;

            results.Add("BOLD|\nПриравниваем кол-во очков к Золоту:");
            result = Character.Protagonist.Gold;
            results.Add($"Золото = {Character.Protagonist.Gold}, соответетвенно Очки = {result}");

            var prevResult = result;

            results.Add("BOLD|Делим на десять:");
            result /= 10;
            results.Add($"{prevResult} / 10 = {result}");

            prevResult = result;

            results.Add("BOLD|Прибавляем к очкам значения параметра 'Даверн':");
            result += Character.Protagonist.Davern;
            results.Add($"{prevResult} + {Character.Protagonist.Davern} = {result}");

            prevResult = result;

            results.Add("BOLD|Добавляем к очкам кол-во недель:");
            result += Character.Protagonist.Week;
            results.Add($"{prevResult} + {Character.Protagonist.Week} = {result}");

            prevResult = result;

            results.Add("BOLD|Прибавляем к очкам кол-во подвигов:");
            result += Character.Protagonist.Feats;
            results.Add($"{prevResult} + {Character.Protagonist.Feats} = {result}");

            var resultLine = Game.Services.CoinsNoun(result, "ое очко", "ых очка", "ых очков");
            results.Add($"\nBIG|BOLD|ИТОГО:");
            results.Add($"BIG|{result} победн{resultLine}!");

            return results;
        }
    }
}
