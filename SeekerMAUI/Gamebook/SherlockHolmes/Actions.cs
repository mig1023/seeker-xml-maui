using System;

namespace SeekerMAUI.Gamebook.SherlockHolmes
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public string Stat { get; set; }

        public string Bonus { get; set; }

        public override List<string> AdditionalStatus()
        {
            var statuses = new List<string>();

            if (Character.Protagonist.Dexterity != null)
                statuses.Add($"Ловкость: {Character.Protagonist.Dexterity}");

            if (Character.Protagonist.Ingenuity != null)
                statuses.Add($"Изобретательность: {Character.Protagonist.Ingenuity}");

            if (Character.Protagonist.Intuition != null)
                statuses.Add($"Интуиция: {Character.Protagonist.Intuition}");

            if (Character.Protagonist.Eloquence != null)
                statuses.Add($"Красноречие: {Character.Protagonist.Eloquence}");

            if (Character.Protagonist.Observation != null)
                statuses.Add($"Наблюдательность: {Character.Protagonist.Observation}");

            if (Character.Protagonist.Erudition != null)
                statuses.Add($"Эрудиция: {Character.Protagonist.Erudition}");

            if (Constants.StoryPart() > 3)
            {
                return null;
            }
            else
            {
                return statuses.Count > 0 ? statuses : null;
            }
        }

        public override List<string> Representer()
        {
            if (Type.StartsWith("Get"))
            {
                var currentStat = GetProperty(Character.Protagonist, Stat);
                var diffLine = string.Empty;

                if (currentStat > 0)
                {
                    var count = Game.Services.CoinsNoun(currentStat, "единица", "единицы", "единицы");
                    diffLine = $"\n+{currentStat} {count}";
                }

                return new List<string> { $"{Head}{diffLine}" };
            }
            else if (!string.IsNullOrEmpty(Stat))
            {
                var property = Constants.StatNames[Stat].ToUpper();
                return new List<string> { $"ПРОВЕРЯЕТСЯ {property}" };
            }
            else if (!string.IsNullOrEmpty(Head))
            {
                return new List<string> { Head };
            }
            else
            {
                return new List<string> { };
            }
        }

        public override bool IsButtonEnabled(bool secondButton = false)
        {
            if (!String.IsNullOrEmpty(Stat))
            {
                var stat = GetProperty(Character.Protagonist, Stat);

                if (secondButton)
                    return (stat > 0);
                else
                    return (Character.Protagonist.StatBonuses > 0);
            }
            else
            {
                return true;
            }
        }

        public override bool AvailabilityNode(string option)
        {
            if (Game.Services.AvailabilityByСomparison(option))
            {
                return Game.Services.AvailabilityByProperty(Character.Protagonist,
                    option, Constants.Availabilities);
            }
            else
            {
                return AvailabilityTrigger(option);
            }
        }

        public List<string> Test() =>
            TestByProperties();

        public List<string> Dexterity() =>
            TestByProperties("Dexterity");

        public List<string> Ingenuity() =>
            TestByProperties("Ingenuity");

        public List<string> Intuition() =>
            TestByProperties("Intuition");

        public List<string> Eloquence() =>
            TestByProperties("Eloquence");

        public List<string> Observation() =>
            TestByProperties("Observation");

        public List<string> Erudition() =>
            TestByProperties("Erudition");

        public List<string> TestByProperties(string property = "")
        {
            Game.Dice.DoubleRoll(out int firstDice, out int secondDice);
            List<string> test = new List<string>();

            var result = firstDice + secondDice;
            test.Add($"BIG|Кубики: {Game.Dice.Symbol(firstDice)} + {Game.Dice.Symbol(secondDice)}");

            if (string.IsNullOrEmpty(property))
            {
                test.Add($"BIG|BOLD|ИТОГО: {firstDice} + {secondDice} = {result}");
                return test;
            }

            var currentStat = GetProperty(Character.Protagonist, property);
            result += currentStat;

            var equal = property == "Eloquence" ? "равно" : "равна";
            test.Add($"BIG|{Constants.StatNames[property]} {equal} {currentStat}");

            if (currentStat <= 0)
            {
                result -= 2;

                test.Add("BAD|Навык равен нуля, поэтому при броске будет применяться штраф в -2 единицы");
                test.Add($"BIG|BOLD|ИТОГО: {firstDice} + {secondDice} - 2 = {result}");
            }
            else
            {
                test.Add($"BIG|BOLD|ИТОГО: {firstDice} + {secondDice} + {currentStat} = {result}");
            }

            if (!string.IsNullOrEmpty(Bonus))
            {
                var bonuses = Bonus
                    .Split("->")
                    .Select(x => x.Trim())
                    .ToList();

                if (Game.Option.IsTriggered(bonuses[0]))
                {
                    result += int.Parse(bonuses[2]);

                    test.Add($"Особый бонус +{bonuses[2]} за {bonuses[1]}");
                    test.Add($"Таким образом итоговое значение стало {result}");
                }
            }

            if (result < 2)
            {
                result = 2;
                test.Add("GRAY|Округляем вверх до 2 единиц");
            }
                
            if (result > 12)
            {
                result = 12;
                test.Add("GRAY|Округляем ввниз до 12 единиц");
            }
                
            Character.Protagonist.LastDices = result;

            foreach (var option in Game.Data.CurrentParagraph.Options)
            {
                if (!option.Text.StartsWith("Получилось") && !option.Text.StartsWith("Результат"))
                    continue;

                var range = option.Text.Split(" ");

                if (range.Length == 2)
                {
                    var dice = int.Parse(range[1]);

                    if (dice != result)
                        Game.Buttons.Disable(option.Text);
                }
                else
                {
                    int min, max;

                    if (range.Length == 4)
                    {
                        min = int.Parse(range[1]);
                        max = int.Parse(range[3]);
                    }
                    else
                    {
                        min = int.Parse(range[2]);
                        max = int.Parse(range[4]);
                    }
                        
                    if ((result < min) || (result > max))
                        Game.Buttons.Disable(option.Text);
                }
            }

            return test;
        }

        public List<string> Get() =>
            ChangeProtagonistParam(Stat, Character.Protagonist, "StatBonuses");

        public List<string> Decrease() =>
            ChangeProtagonistParam(Stat, Character.Protagonist, "StatBonuses", decrease: true);
    }
}
