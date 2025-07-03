using System;
using static SeekerMAUI.Gamebook.LegendsAlwaysLie.Actions;

namespace SeekerMAUI.Gamebook.LegendsAlwaysLie
{
    class Events
    {
        public static List<string> MushroomsForConnery()
        {
            if (Character.Protagonist.ConneryTrust >= 6)
            {
                Character.Protagonist.ConneryHitpoints += 3;
                return new List<string> { "BIG|GOOD|Коннери хмыкнул и съел :)" };
            }
            else
            {
                return new List<string> { "BIG|BAD|Коннери отказался :(" };
            }
        }

        public static List<string> FootwrapsReplacement()
        {
            Game.Option.Trigger("Legs", remove: true);

            return new List<string> { "BIG|GOOD|Вы успешно поменяли портянки :)" };
        }

        public static List<string> FootwrapsDeadlyReplacement()
        {
            Character.Protagonist.Hitpoints += (Game.Option.IsTriggered("Legs") ? 4 : 2);

            return new List<string> { "BIG|GOOD|Вы успешно поменяли портянки :)" };
        }

        public static List<string> CureSprain()
        {
            Character.Protagonist.Strength += 1;
            Character.Protagonist.Magicpoints -= 1;

            return new List<string> { "BIG|GOOD|Вы успешно вылечили растяжение" };
        }

        public static List<string> ShareFood(FoodSharingType? foodSharing)
        {
            Game.Option.Trigger("FoodIsDivided");

            if (foodSharing == FoodSharingType.KeepMyself)
            {
                Character.Protagonist.Hitpoints += 5;
            }
            else if (foodSharing == FoodSharingType.ToHim)
            {
                Character.Protagonist.ConneryHitpoints += 5;
            }
            else
            {
                Character.Protagonist.Hitpoints += 3;
                Character.Protagonist.ConneryHitpoints += 3;
            }

            return new List<string> { "RELOAD" };
        }
    }
}
