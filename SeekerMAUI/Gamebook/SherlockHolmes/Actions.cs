using System;

namespace SeekerMAUI.Gamebook.SherlockHolmes
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public string Stat { get; set; }

        public override List<string> AdditionalStatus() => new List<string>
        {
            $"Ловкость: {Character.Protagonist.Dexterity}",
            $"Изобретательность: {Character.Protagonist.Ingenuity}",
            $"Интуиция: {Character.Protagonist.Intuition}",
            $"Красноречие: {Character.Protagonist.Eloquence}",
            $"Наблюдательность: {Character.Protagonist.Observation}",
            $"Эрудиция: {Character.Protagonist.Erudition}",
        };

        public override List<string> Representer()
        {
            if (Type.StartsWith("Get"))
            {
                int currentStat = GetProperty(Character.Protagonist, Stat);
                string diffLine = String.Empty;

                if (currentStat > 0)
                {
                    string count = Game.Services.CoinsNoun(currentStat, "единица", "единицы", "единицы");
                    diffLine = $"\n+{currentStat} {count}";
                }

                return new List<string> { $"{Head}{diffLine}" };
            }
            else if (!String.IsNullOrEmpty(Head))
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
                int stat = GetProperty(Character.Protagonist, Stat);

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

        public List<string> Test()
        {
            Game.Dice.DoubleRoll(out int firstDice, out int secondDice);

            int currentStat = GetProperty(Character.Protagonist, Stat);
            int result = firstDice + secondDice + currentStat;
            List<string> test = new List<string>();

            test.Add($"Кубики: {Game.Dice.Symbol(firstDice)} + {Game.Dice.Symbol(secondDice)}");
            test.Add($"{Constants.StatNames[Stat]} равна {currentStat}");
            test.Add($"BIG|BOLD|ИТОГО: {firstDice} + {secondDice} + {currentStat} = {result}");

            Character.Protagonist.LastDices = result;

            return test;
        }

        public List<string> Get() =>
            ChangeProtagonistParam(Stat, Character.Protagonist, "StatBonuses");

        public List<string> Decrease() =>
            ChangeProtagonistParam(Stat, Character.Protagonist, "StatBonuses", decrease: true);
    }
}
