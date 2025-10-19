using System;

namespace SeekerMAUI.Gamebook.KnightOfTheLivingDead
{
    class Dodecahedron
    {
        public static int Roll(ref List<string> text)
        {
            text.Add("GRAY|Бросаем додекаэдр:");

            var dice = Game.Dice.Roll();
            text.Add($"GRAY|Первый кубик (значение): {Game.Dice.Symbol(dice)}");

            var multiplicator = Game.Dice.Roll();
            text.Add($"GRAY|Второй кубик (мультипликатор): {Game.Dice.Symbol(multiplicator)}");

            var result = dice;

            if (multiplicator > 3)
            {
                result += 6;
                text.Add("GRAY|На мультипликаторе выбало больше трёх! Прибавляем к кубику шестёрку.");
                text.Add($"ИТОГ по додекаэдру: {Game.Dice.Symbol(dice)} + 6 = {result}");
            }
            else
            {
                text.Add("На мультипликаторе выбало меньше четырёх! Значение кубика остаётся.");
                text.Add($"ИТОГ по додекаэдру: {result}");
            }

            return result;
        }
    }
}
