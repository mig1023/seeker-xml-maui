using System;

namespace SeekerMAUI.Gamebook.MentorsAlwaysRight
{
    class Cure
    {
        public static List<string> Rabies()
        {
            if (Actions.CureSpellCount() < 1)
                return new List<string> { "BIG|BAD|У вас нет ЛЕЧИЛКИ :(" };

            List<string> cure = new List<string> { };

            Character.Protagonist.Spells.Remove("ЛЕЧЕНИЕ");

            Game.Option.Trigger("Rabies", remove: true);
            cure.Add("BIG|GOOD|Вы успешно вылечили болезнь!");

            Character.Protagonist.Hitpoints += 3;
            cure.Add("BOLD|Вы дополнительно получили +3 жизни.");

            return cure;
        }

        public static List<string> Fracture(int wound, string onlyOne)
        {
            if (wound > 1)
            {
                if (Actions.CureSpellCount() < 2)
                    return new List<string> { "BIG|BAD|У вас нет двух ЛЕЧИЛОК :(" };

                for (int i = 0; i <= 1; i++)
                    Character.Protagonist.Spells.Remove("ЛЕЧЕНИЕ");

                Character.Protagonist.Hitpoints += 4;
            }
            else
            {
                if (Actions.CureSpellCount() < 1)
                    return new List<string> { "BIG|BAD|У вас нет ЛЕЧИЛКИ :(" };

                Character.Protagonist.Spells.Remove("ЛЕЧЕНИЕ");
                Character.Protagonist.Strength -= 1;
            }

            Game.Option.Trigger(onlyOne);

            return new List<string> { "RELOAD" };
        }
    }
}
