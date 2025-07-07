using System;

namespace SeekerMAUI.Gamebook.YounglingTournament
{
    class Dice
    {
        public static List<string> Wounds()
        {
            List<string> diceCheck = new List<string> { };

            int dice = Game.Dice.Roll();

            diceCheck.Add($"На кубике выпало: {Game.Dice.Symbol(dice)}");

            Character.Protagonist.Hitpoints -= dice;

            diceCheck.Add($"BIG|BAD|Вы потеряли жизней: {dice}");

            return diceCheck;
        }

        public static List<string> EnemyWounds(Actions actions)
        {
            List<string> diceCheck = new List<string> { };

            int dice = Game.Dice.Roll();

            int bonus = 0;
            string bonusLine = String.Empty;
            string[] enemy = actions.Enemy.Split(',');

            bool withBonus = Enum.TryParse(actions.BonusTechnique, out Character.ForcesTypes techniqueType);

            if (withBonus)
            {
                bonus = Character.Protagonist.ForceTechniques[techniqueType];
                bonusLine = $" + {bonus} за ранг";
            }

            diceCheck.Add($"На кубике выпало: {Game.Dice.Symbol(dice)}{bonusLine}");

            dice += bonus;

            Character.SetHitpoints(enemy[0], dice, int.Parse(enemy[1]));

            diceCheck.Add($"BIG|GOOD|{enemy[0]} потерял жизней: {dice}");

            return diceCheck;
        }
    }
}
