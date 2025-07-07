using System;

namespace SeekerMAUI.Gamebook.StrikeBack
{
    class Dice
    {
        public static List<string> Roll(int count, bool woundsByDices, int woundsMultiple)
        {
            List<string> diceCheck = new List<string> { };

            int dices = count == 0 ? 1 : count;
            int result = 0;
            string lineFormat = ((dices > 1) || woundsByDices ? String.Empty : "BIG|") +
                "На{0} кубике выпало: {1}";

            for (int i = 1; i <= dices; i++)
            {
                int dice = Game.Dice.Roll();
                result += dice;
                string diceNum = dices > 1 ? $" {i}" : String.Empty;
                diceCheck.Add(String.Format(lineFormat, diceNum, Game.Dice.Symbol(dice)));
            }

            if (woundsByDices)
            {
                if (woundsMultiple > 0)
                {
                    diceCheck.Add($"Сумма ({result}) умножается на {woundsMultiple}");
                    result *= woundsMultiple;
                }

                Character.Protagonist.Endurance -= result;
                diceCheck.Add($"BIG|BAD|Ты потерял выносливостей: {result}");
            }
            else if (dices > 1)
            {
                diceCheck.Add($"BIG|BOLD|Выпало всего: {result}");
            }

            return diceCheck;
        }

        public static List<string> FindTheWay()
        {
            List<string> way = new List<string> { "BIG|Ищем путь:" };

            double wayPoint = 202;

            while (true)
            {
                int direction = Game.Dice.Roll(size: 2);

                way.Add("GRAY|Подбрасываем монетку: " + (direction == 1 ? "орёл" : "решка"));

                if ((wayPoint < 12) && (direction == 2))
                {
                    direction = 1;
                    way.Add("GRAY|Так в стенку упрёмся, лучше пойдём направо");
                }

                if ((wayPoint >= 1250) && (direction == 1))
                {
                    direction = 2;
                    way.Add("GRAY|Так в стенку упрёмся, лучше пойдём налево");
                }

                if (direction == 1)
                {
                    way.Add("BOLD|Поворачиваем направо...");
                    double newWayPoint = wayPoint * 4;
                    way.Add($"{wayPoint} * 4 = {newWayPoint}");
                    wayPoint = newWayPoint;
                }
                else
                {
                    way.Add("BOLD|Поворачиваем налево...");
                    double newWayPoint = (wayPoint - 1) / 3;
                    way.Add($"({wayPoint} - 1) / 3 = {newWayPoint}");
                    wayPoint = newWayPoint;
                }

                int cleanWayPoint = Convert.ToInt32(wayPoint);

                if ((cleanWayPoint != wayPoint) || (wayPoint > 5000))
                {
                    way.Add(String.Empty);
                    way.Add("BIG|BAD|Всё, тупик...");
                    return way;
                }
                else if ((wayPoint >= 301) && (wayPoint <= 450))
                {
                    Game.Option.OpenButtonByGoto(cleanWayPoint);

                    way.Add(String.Empty);
                    way.Add($"BIG|GOOD|Кхм, число {wayPoint} вроде бы подходит...");
                    return way;
                }
            }
        }
    }
}
