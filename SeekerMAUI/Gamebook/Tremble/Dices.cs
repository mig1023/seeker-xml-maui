using System;

namespace SeekerMAUI.Gamebook.Tremble
{
    class Dices
    {
        public static List<string> Luck()
        {
            Game.Dice.DoubleRoll(out int firstDice, out int secondDice);
            var goodLuck = (firstDice + secondDice) < Character.Protagonist.Luck;
            var luckLine = goodLuck ? "<=" : ">";

            List<string> luckCheck = new List<string> {
                $"Проверка удачи: {Game.Dice.Symbol(firstDice)} + " +
                $"{Game.Dice.Symbol(secondDice)} {luckLine} {Character.Protagonist.Luck}" };

            if (firstDice == secondDice)
            {
                luckCheck.Add("На кубиках выпал дубль - это всегда удача!");
                goodLuck = true;
            }

            luckCheck.Add(goodLuck ? "BIG|GOOD|УСПЕХ :)" : "BIG|BAD|НЕУДАЧА :(");
            Game.Buttons.Disable(goodLuck, "Win", "Fail");

            if (Character.Protagonist.Luck > 1)
            {
                Character.Protagonist.Luck -= 1;
                luckCheck.Add("Уровень удачи снижен на единицу");
            }

            return luckCheck;
        }

        public static List<string> Check(bool woundsByDice,
            bool heavyDamageByDice, bool lightDamageByDice)
        {
            var dice = Game.Dice.Roll();
            var check = new List<string> { $"BIG|Кубик: {Game.Dice.Symbol(dice)}" };

            if (woundsByDice)
            {
                Character.Protagonist.Endurance -= dice;
                var line = Game.Services.CoinsNoun(dice, "единицу", "единицы", "единиц");
                check.Add($"BAD|BOLD|Вы потеряли {dice} {line} Выносливости");

                if (Character.Protagonist.Endurance <= 0)
                {
                    Character.Protagonist.Endurance = 1;
                    check.Add("1 единица Выносливости всё-таки остаётся!");
                }
            }
            else if (heavyDamageByDice)
            {
                if (dice > 3)
                {
                    Character.Protagonist.Endurance -= 4;
                    Character.Protagonist.Skill -= 1;
                    check.Add("BAD|BOLD|На кубике выпало 4+!");
                    check.Add("Вы теряете 3 единицы Выносливости и 1 единицу Ловкости!");
                }
                else
                {
                    check.Add("GOOD|BOLD|Обошлось..!");
                }
            }
            else if (lightDamageByDice)
            {
                if (dice <= 2)
                {
                    Character.Protagonist.Endurance -= 1;
                    check.Add("BAD|BOLD|На кубике выпало меньше 3!");
                    check.Add("Вы теряете 1 единицу Выносливости!");
                }
                else
                {
                    check.Add("GOOD|BOLD|Обошлось..!");
                }
            }

            return check;
        }
    }
}
