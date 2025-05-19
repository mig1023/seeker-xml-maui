using System;

namespace SeekerMAUI.Gamebook.LoneWolf
{
    class BattleTable
    {
        private static Dictionary<int, string> Table { get; set; }

        public static void Init(int coefficient)
        {
            if (coefficient >= 11)
            {
                coefficient = 12;
            }
            else if (coefficient <= -11)
            {
                coefficient = -11;
            }
            else if (coefficient % 2 != 0)
            {
                coefficient += coefficient > 0 ? 1 : -1;
            }

            var negative = coefficient < 0 ? "N" : String.Empty;
            var tableName = $"Coefficient{negative}{Math.Abs(coefficient)}";

            Table = (Dictionary<int, string>)typeof(Constants)
                .GetProperties()
                .Where(x => x.Name == tableName)
                .FirstOrDefault()
                .GetValue(null);
        }

        public static void Get(int dice, out int heroDamage, out int enemyDamage)
        {
            var line = Table[dice + 1];
            var damages = line.Split('/');

            enemyDamage = int.Parse(damages[0]);
            heroDamage = int.Parse(damages[1]);
        }

    }
}
