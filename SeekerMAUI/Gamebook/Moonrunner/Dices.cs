using System;

namespace SeekerMAUI.Gamebook.Moonrunner
{
    class Dices
    {
        public static List<string> Three()
        {
            List<string> dices = new List<string> { };

            int dicesResult = 0;

            for (int i = 1; i <= 3; i++)
            {
                int dice = Game.Dice.Roll();

                dicesResult += dice;

                dices.Add($"На {i} кубикe выпало: {Game.Dice.Symbol(dice)}");
            }

            dices.Add($"BOLD|Итого выпало: {dicesResult}");

            dices.Add(dicesResult > Character.Protagonist.Endurance ?
                "BIG|BAD|Больше, чем выносливость :(" : "BIG|GOOD|Меньше, чем выносливость :)");

            return dices;
        }

        public static List<string> Wounds()
        {
            List<string> wounds = new List<string> { };

            int dice = Game.Dice.Roll();

            wounds.Add($"На кубике выпало: {Game.Dice.Symbol(dice)}");

            Character.Protagonist.Endurance -= dice * 2;

            wounds.Add($"BIG|BAD|Вы потеряли жизней: {dice * 2}");

            return wounds;
        }

        public static List<string> Spells()
        {
            List<string> spell = new List<string> { };

            if (Character.Protagonist.EnemySpells <= 0)
            {
                spell.Add("BIG|GOOD|Вы смогли выдержать все его заклятья :)");
                return spell;
            }

            int dice = 0;

            while (true)
            {
                dice = Game.Dice.Roll();

                spell.Add($"На кубике выпало: {Game.Dice.Symbol(dice)}");

                if (Game.Option.IsTriggered(Constants.SpellsList[dice]))
                {
                    spell.Add("Уже было, кидаем ещё раз.");
                }
                else
                {
                    break;
                }
            }

            Character.Protagonist.EnemySpells -= dice;

            spell.Add($"Сила Натуры Грула снижается на {dice} и теперь равен {Character.Protagonist.EnemySpells}");
            spell.Add($"BIG|BAD|Вам нужно выдержать заклятье: {Constants.SpellsList[dice]}");

            if (Game.Option.IsTriggered("ecproc"))
                spell.Add("Выдержав это заклятье, посмотрите пункт про слово “ecproc”");

            return spell;
        }
    }
}
