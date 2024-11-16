using System;
using System.Collections.Generic;
using System.Linq;
using System;

namespace SeekerMAUI.Gamebook.KoshcheisChain
{
    class Octagon
    {
        private static Random rand = new Random();

        private static string DiceSymbols = "□⚀⚁⚂⚃⚄⚅◯";

        public static int Roll() =>
            rand.Next(8);

        public static int RollValue(int dices = 1)
        {
            int result = 0;

            for (int i = 0; i < dices; i++)
            {
                int dice = Roll();
                dice = dice == 7 ? 10 : dice;
                result += dice;
            }

            return result;
        }

        public static int RingValueFuse(int dice) =>
            dice == 7 ? 0 : dice;

        public static void DoubleRoll(out int firstDice, out int secondDice)
        {
            firstDice = Roll();
            secondDice = Roll();
        }

        public static string Symbol(int dice) =>
            DiceSymbols[dice].ToString();
    }
}
