using System;

namespace SeekerMAUI.Gamebook.MentorsAlwaysRight
{
    class Events
    {
        public static List<string> Camouflage()
        {
            Game.Option.Trigger("Camouflage");

            return new List<string> { "Вы успешно себя закамуфлировали грязью :)" };
        }

        public static List<string> MagicBlade()
        {
            Game.Option.Trigger("MagicSword");
            Character.Protagonist.Gold -= 5;

            return new List<string> { "BIG|GOOD|Ваш меч теперь заколдован :)" };
        }

        public static List<string> StoneThrow(Actions actions)
        {
            if (Character.Protagonist.Specialization == Character.SpecializationType.Thrower)
                return new List<string> { "BIG|GOOD|Ваша специализациея является метание ножей - и вы не промахнулись :)" };

            int dice = Game.Dice.Roll();

            List<string> stoneThrow = new List<string> { };

            stoneThrow.Add($"На кубике выпало: {Game.Dice.Symbol(dice)}");
            stoneThrow.Add(actions.Result(dice > 4, "Вы попали", "Вы промахнулись"));

            return stoneThrow;
        }
    }
}
