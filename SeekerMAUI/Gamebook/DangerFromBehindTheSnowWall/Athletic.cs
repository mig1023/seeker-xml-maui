using System;

namespace SeekerMAUI.Gamebook.DangerFromBehindTheSnowWall
{
    class Athletic
    {
        public static List<string> Shape()
        {
            List<string> lines = new List<string> { "BIG|Рассчитываем ФОРМУ:" };

            int athletic = Character.Protagonist.Skill - 8;

            lines.Add($"1. Из Ловкости ({Character.Protagonist.Skill} ед.) " +
                $"вычитаем восемь и получаем {athletic} ед. Формы.");

            if (Character.Protagonist.Strength < 4)
            {
                athletic -= 2;

                lines.Add($"2. Т.к. Сила равна или меньше трём (а она равна " +
                    $"{Character.Protagonist.Strength}), то Форма уменьшается " +
                    $"на 2 ед. и становится равна {athletic}.");
            }
            else if ((Character.Protagonist.Strength > 3) && (Character.Protagonist.Strength < 7))
            {
                lines.Add($"2. Т.к. Сила в диапазоне от 4 до 6 ед. (а она равна " +
                    $"{Character.Protagonist.Strength}), то Форма не получает никаких " +
                    $"бонусов и остаётся равна {athletic}.");
            }
            else
            {
                string range = String.Empty;
                int bonus = 0;

                if (Character.Protagonist.Strength < 15)
                {
                    bonus += 1;
                    range = "в диапазоне от 7 до 14 ед.";
                }
                else if ((Character.Protagonist.Strength > 14) && (Character.Protagonist.Strength < 21))
                {
                    bonus += 2;
                    range = "в диапазоне от 15 до 20 ед.";
                }
                else
                {
                    bonus += 3;
                    range = "равна или больше 20 ед.";
                }

                athletic += bonus;

                lines.Add($"2. Т.к. Сила {range} (а она равна {Character.Protagonist.Strength}), " +
                    $"то Форма увеличивается на {bonus} ед. и становится равна {athletic}.");
            }

            Character.Protagonist.AthleticShape = athletic;
            string athleticLine = Game.Services.CoinsNoun(athletic, "единица", "единицы", "единиц");
            lines.Add($"BIG|BOLD|Итоговая Форма: {athletic} {athleticLine}");

            return lines;
        }

        public static List<string> Bonus()
        {
            int dice = Game.Dice.Roll();
            int athleticShape = Character.Protagonist.AthleticShape ?? 0;
            Character.Protagonist.AthleticShape += dice;

            return new List<string>
           {
                $"BIG|На кубике выпало: {Game.Dice.Symbol(dice)}",
                $"BIG|BOLD|Форма увеличилась на {athleticShape} и теперь равна {athleticShape + dice}"
            };
        }
    }
}
