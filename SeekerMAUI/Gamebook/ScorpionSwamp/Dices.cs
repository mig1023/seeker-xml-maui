using System;

namespace SeekerMAUI.Gamebook.ScorpionSwamp
{
    class Dices
    {
        public static List<string> Endurance()
        {
            Game.Dice.DoubleRoll(out int firstDice, out int secondDice);

            List<string> endurance = new List<string> {
                $"Бросаем кубики: {Game.Dice.Symbol(firstDice)} и " +
                $"{Game.Dice.Symbol(secondDice)}" };

            endurance.Add($"Вычитаем из выносливости наименьший");

            int lost = firstDice > secondDice ? firstDice : secondDice;

            Character.Protagonist.Endurance -= lost;

            string count = Game.Services.CoinsNoun(lost, "единицу", "единицы", "единиц");
            endurance.Add($"BIG|BAD|Выносливость снижена на {lost} {count}");
            return endurance;
        }

        public static List<string> Wounds()
        {
            List<string> wounds = new List<string> { };

            int dice = Game.Dice.Roll();
            wounds.Add($"BIG|На кубике выпало: {Game.Dice.Symbol(dice)}");

            Character.Protagonist.Endurance -= dice;

            wounds.Add($"BIG|BAD|Вы потеряли жизней: {dice}");

            return wounds;
        }
    }
}
