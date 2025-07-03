using System;

namespace SeekerMAUI.Gamebook.HowlOfTheWerewolf
{
    class Dices
    {
        public static List<string> Roll() =>
            new List<string> { $"BIG|На кубике выпало: {Game.Dice.Symbol(Game.Dice.Roll())}" };

        public static List<string> Restore()
        {
            List<string> diceRestore = new List<string> { };

            int dice = Game.Dice.Roll();

            diceRestore.Add($"На кубике выпало: {Game.Dice.Symbol(dice)}");

            string line = "BIG|GOOD|Восстановлен";

            if (dice < 3)
            {
                Character.Protagonist.Mastery = Character.Protagonist.MaxMastery;
                line += "о Мастерство";
            }
            else if (dice > 4)
            {
                Character.Protagonist.Luck = Character.Protagonist.MaxLuck;
                line += "а Удача";
            }
            else
            {
                Character.Protagonist.Endurance = Character.Protagonist.MaxEndurance;
                line += "а Выносливость";
            }

            diceRestore.Add(line);

            return diceRestore;
        }

        public static List<string> Wounds(int value)
        {
            List<string> diceCheck = new List<string> { };

            int dice = Game.Dice.Roll();

            string bonus = (value > 0 ? $" + ещё {value}" : String.Empty);

            diceCheck.Add($"На кубике выпало: {Game.Dice.Symbol(dice)}{bonus}");

            Character.Protagonist.Endurance -= dice + value;

            diceCheck.Add($"BIG|BAD|Вы потеряли жизней: {dice + value}");

            return diceCheck;
        }

        public static List<string> Gold(int value)
        {
            List<string> diceCheck = new List<string> { };

            int dice = Game.Dice.Roll();

            diceCheck.Add($"На кубике выпало: {Game.Dice.Symbol(dice)} + ещё {value}");

            dice += value;

            Character.Protagonist.Gold -= dice;

            diceCheck.Add($"BIG|GOOD|Вы нашли золотых: {dice}");

            return diceCheck;
        }

        public static List<string> Anxiety(Actions actions)
        {
            List<string> diceCheck = new List<string> { };

            Game.Dice.DoubleRoll(out int firstDice, out int secondDice);
            int result = firstDice + secondDice;

            diceCheck.Add($"На кубиках выпало: {Game.Dice.Symbol(firstDice)} + " +
                $"{Game.Dice.Symbol(firstDice)} = {result}");

            diceCheck.Add($"Текущий уровень тревоги: {Character.Protagonist.Anxiety}");

            diceCheck.Add(actions.Result(result > Character.Protagonist.Anxiety, "Больше!", "Меньше"));

            return diceCheck;
        }

        public static List<string> Endurance(Actions actions)
        {
            List<string> diceCheck = new List<string> { };

            int result = 0;

            for (int i = 1; i <= 3; i++)
            {
                int dice = Game.Dice.Roll();
                result += dice;
                diceCheck.Add($"На {i} выпало: {Game.Dice.Symbol(dice)}");
            }

            diceCheck.Add($"BIG|Сумма на кубиках: {result}");

            diceCheck.Add(actions.Result(result < Character.Protagonist.Endurance, "Меньше!", "Больше"));

            return diceCheck;
        }
    }
}
