using System;

namespace SeekerMAUI.Gamebook.DangerFromBehindTheSnowWall
{
    class Wounds
    {
        private static string Strength(int dice) =>
            Game.Services.CoinsNoun(dice, "СИЛУ", "СИЛЫ", "СИЛ");

        public static List<string> Dice()
        {
            var dice = Game.Dice.Roll();
            Character.Protagonist.Strength -= dice;

            return new List<string>
            {
                $"BIG|На кубике выпало: {Game.Dice.Symbol(dice)}",
                $"BIG|BAD|BOLD|Вы потеряли {dice} {Strength(dice)}"
            };
        }

        private static int ColdNight(int dice)
        {
            if (dice == 1)
            {
                return 1;
            }
            else if (dice < 5)
            {
                return 2;
            }
            else
            {
                return 4;
            }
        }

        private static int ColdDay(int dice)
        {
            if (dice < 3)
            {
                return 4;
            }
            else if (dice < 5)
            {
                return 5;
            }
            else
            {
                return 6;
            }
        }

        public static List<string> ColdDice(bool dayWounds)
        {
            var dice = Game.Dice.Roll();
            var loss = dayWounds ? ColdDay(dice) : ColdNight(dice);

            return new List<string>
            {
                $"BIG|На кубике выпало: {Game.Dice.Symbol(dice)}",
                $"BIG|BAD|BOLD|Вы потеряли {loss} {Strength(dice)}"
            };
        }
    }
}
