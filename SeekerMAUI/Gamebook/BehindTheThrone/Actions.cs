using System;

namespace SeekerMAUI.Gamebook.BehindTheThrone
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public string Stat { get; set; }

        public override List<string> Status() => new List<string>
        {
            $"Проворство: {Character.Protagonist.Agility}",
            $"Меткость: {Character.Protagonist.Marksmanship}",
            $"Фехтование: {Character.Protagonist.Swashbuckling}",
            $"Живучесть: {Character.Protagonist.Vitality}",
        };

        public override bool GameOver(out int toEndParagraph, out string toEndText) =>
            GameOverBy(Character.Protagonist.Vitality, out toEndParagraph, out toEndText);

        public override List<string> Representer() =>
            new List<string> { "Проверка " + Constants.CharactersParams[Stat] };

        public List<string> Test()
        {
            List<string> test = Representer();

            int param = GetProperty(Character.Protagonist, Stat);

            Game.Dice.DoubleRoll(out int firstDice, out int secondDice);

            int result = firstDice + secondDice;

            test.Add($"BOLD|BIG|Бросок кубиков: " +
                $"{Game.Dice.Symbol(firstDice)} + {Game.Dice.Symbol(secondDice)} = " +
                $"{result}");

            if (result <= param)
            {
                test.Add($"Сумма на кубиках не превышает значения параметра, равного {param}!");
                test.Add("GOOD|BOLD|ПРОВЕРКА УСПЕШНО ПРОЙДЕНА :)");

                Game.Buttons.Disable("Fail");
            }
            else
            {
                test.Add($"Сумма на кубиках превышает значение параметра, равного {param}!");
                test.Add("BAD|BOLD|ПРОВЕРКА ПРОВАЛЕНА :(");

                Game.Buttons.Disable("Win");
            }

            return test;
        }

        public override bool Availability(string option) =>
            AvailabilityTrigger(option);
    }
}
