using System;
using System.Xml.Linq;

namespace SeekerMAUI.Gamebook.PeachKwacho
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public List<string> RollCoin()
        {
            var coin = Game.Dice.Roll() % 2 == 0;
            var line = coin ? "На монетке выпал ОРЁЛ" : "На монетке выпала РЕШКА";

            return new List<string> { $"BIG|BOLD|{line}" };
        }

        private string CoinsNoun(int value)
        {
            if (value == 0)
            {
                return "единиц";
            }
            else
            {
                return Game.Services.CoinsNoun(value, "единице", "единицам", "единицам");
            }
        }

        public List<string> Fight()
        {
            var fight = new List<string> { "BIG|БОЙ С ЖИРОВИКОМ:" };

            var car = 7;
            var robot = 5;

            while (true)
            {
                fight.Add(string.Empty);

                var coin = Game.Dice.Roll() % 2 == 0;

                if (coin)
                {
                    robot -= 1;

                    var hitpoints = CoinsNoun(robot);

                    fight.Add("GOOD|BOLD|На монетке выпал ОРЁЛ!");
                    fight.Add($"Жировик получил попадание и теперь его " +
                        $"прочность равна {robot} {hitpoints}!");

                    if (robot <= 0)
                        return Win(fight);
                }
                else
                {
                    car -= 1;

                    var hitpoints = CoinsNoun(car);

                    fight.Add("BAD|BOLD|На монетке выпала РЕШКА!");
                    fight.Add($"Жировик попал по машине и теперь " +
                        $"прочность автомобиля равна {car} {hitpoints}!");

                    if (car <= 0)
                        return Fail(fight);
                }
            }
        }

        public override bool Availability(string option) =>
            AvailabilityTrigger(option);
    }
}
