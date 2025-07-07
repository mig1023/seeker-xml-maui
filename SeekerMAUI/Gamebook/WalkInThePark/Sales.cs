using System;

namespace SeekerMAUI.Gamebook.WalkInThePark
{
    class Sales
    {
        public static List<string> Beer(Actions actions)
        {
            List<string> sell = new List<string>();

            while (actions.IsTriggered("пиво", out string beer))
            {
                sell.Add($"Продаём {beer} (по 50р)");
                sell.Add($"BIG|GOOD|BOLD|Ты заработал полтинник!");
                Character.Protagonist.Money += 50;

                Game.Option.Trigger(beer, remove: true);
            }

            return sell;
        }

        public static List<string> Accordion()
        {
            List<string> sell = new List<string>
            {
                $"Продаём баян (1000р)",
                $"BIG|GOOD|BOLD|Ты заработал косарь!"
            };

            Character.Protagonist.Money += 1000;

            Game.Option.Trigger("баян", remove: true);

            return sell;
        }

        public static List<string> SunflowerSeeds()
        {
            List<string> sell = new List<string>
            {
                "BOLD|Продаём сэмки (по 25р пачка)",
            };

            int count = Character.Protagonist.SunflowerSeeds;
            string pack = Game.Services.CoinsNoun(count, "пачку", "пачки", "пачек");
            sell.Add($"Всего {count} {pack} сэмак");

            int sum = Character.Protagonist.SunflowerSeeds * 25;
            Character.Protagonist.Money += sum;

            sell.Add($"BIG|GOOD|BOLD|Ты заработал {sum} рублёв!");

            return sell;
        }
    }
}
