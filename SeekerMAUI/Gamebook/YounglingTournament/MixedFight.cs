using System;
using static SeekerMAUI.Gamebook.YounglingTournament.Character;

namespace SeekerMAUI.Gamebook.YounglingTournament
{
    class MixedFight
    {
        public static List<string> Attack()
        {
            List<string> attackCheck = new List<string> { };

            int deflecting = 4 + Character.Protagonist.SwordTechniques[SwordTypes.Rivalry];

            attackCheck.Add("Выстрел: 10 (сила выстрела) x 9 (меткость) = 90");

            attackCheck.Add($"Отражение: 4 + " +
                $"{Character.Protagonist.SwordTechniques[SwordTypes.Rivalry]} " +
                $"ранг = {deflecting}");

            int result = 90 / deflecting;

            attackCheck.Add($"Результат: " +
                $"90 выстрел / {deflecting} отражение = {result}");

            if (result > 0)
            {
                Character.Protagonist.Hitpoints -= result;
                attackCheck.Add($"BIG|BAD|Вы потеряли жизней: {result}");
            }
            else
            {
                attackCheck.Add("BIG|GOOD|Вам удалось отразить " +
                    "выстрел противника в него самого!");
            }

            return attackCheck;
        }

        public static List<string> Defence()
        {
            List<string> defenseCheck = new List<string> { };

            int deflecting = 4 + Character.Protagonist.SwordTechniques[SwordTypes.Rivalry];

            Game.Dice.DoubleRoll(out int firstDice, out int secondDice);
            int shoot = firstDice + secondDice + 19;

            defenseCheck.Add($"Выстрел: " +
                $"{Game.Dice.Symbol(firstDice)} + " +
                $"{Game.Dice.Symbol(secondDice)} + " +
                $"10 (сила выстрела) + 9 (меткость) = {shoot}");

            defenseCheck.Add($"Отражение: 4 + " +
                $"{Character.Protagonist.SwordTechniques[SwordTypes.Rivalry]} " +
                $"ранг = {deflecting}");

            int result = shoot / deflecting;

            defenseCheck.Add($"Результат: " +
                $"{shoot} выстрел / {deflecting} " +
                $"отражение = {result}");

            Character.Protagonist.Hitpoints -= result;

            defenseCheck.Add($"BIG|BAD|Вы потеряли жизней: {result}");

            return defenseCheck;
        }
    }
}
