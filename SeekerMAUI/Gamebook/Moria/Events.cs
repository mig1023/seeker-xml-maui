using System;

namespace SeekerMAUI.Gamebook.Moria
{
    class Events
    {
        public static List<string> DeathsByArrows()
        {
            List<string> deaths = new List<string>();

            for (int i = 0; i < 2; i++)
            {
                if (Character.Protagonist.Fellowship.Count < 1)
                    continue;

                int dice = Game.Dice.Roll(size: Character.Protagonist.Fellowship.Count) - 1;
                string name = Character.Protagonist.Fellowship[dice];

                deaths.Add($"BIG|BAD|BOLD|Погиб {name}! :(");

                Character.Protagonist.Fellowship.Remove(name);
            }

            return deaths;
        }

        public static List<string> Balrog()
        {
            List<string> fight = new List<string>();

            int strength = Constants.Fellowship["Гэндальф"];
            int success = 0;

            fight.Add("BOLD|Гэндальф сражается против Балрога");
            fight.Add(String.Empty);

            for (int i = 1; i <= 4; i++)
            {
                if (success >= 3)
                    continue;

                fight.Add($"Раунд {i}");

                int gendalfDice = Game.Dice.Roll();

                fight.Add($"Бросок Гэндальфа: {Game.Dice.Symbol(gendalfDice)}");

                if (gendalfDice > 4)
                {
                    fight.Add("BOLD|GOOD|Гэндальф успешно нанёс ранение Балрогу!");
                    success += 1;
                }
                else
                {
                    fight.Add("BOLD|BAD|Гэндальф не смог ранить Балрога!");
                }

                if (success >= 3)
                    continue;

                int balrogDice = Game.Dice.Roll();

                fight.Add($"Бросок Балрога: {Game.Dice.Symbol(balrogDice)}");

                if (balrogDice >= strength)
                {
                    fight.Add("BOLD|BAD|Балрог наносит смертельный удар!");
                    fight.Add("BOLD|BIG|BAD|Гэндальф погиб :(");
                    Character.Protagonist.Fellowship.Remove("Гэндальф");

                    return fight;
                }
                else
                {
                    fight.Add($"BOLD|GOOD|Балрог не смог ранить Гэндальфа!");
                }

                fight.Add(String.Empty);
            }

            if (success >= 3)
            {
                fight.Add("BIG|GOOD|Гэндальф ПОБЕДИЛ :)");
            }
            else
            {
                fight.Add("BIG|GOOD|Гэндальф не смог победить, Балрога, " +
                    "но, тем не менее, он продержался все 4 раунда! :)");
            }

            return fight;
        }

        public static List<string> RunningUnderArrows()
        {
            List<string> deaths = new List<string>();
            List<string> fellowship = new List<string>(Character.Protagonist.Fellowship);

            foreach (string warrior in fellowship)
            {
                if ((warrior == "Гэндальф") || (warrior == "Фродо"))
                    continue;

                deaths.Add($"BOLD|Свою судьбу испытывает {warrior}");

                int dice = Game.Dice.Roll();
                bool coin = dice % 2 == 0;

                if (coin)
                {
                    deaths.Add("На кубике выпал: Орел");
                    deaths.Add("GOOD|Ему повезло!");
                }
                else
                {
                    deaths.Add("На кубике выпала: Решка");
                    deaths.Add("BAD|Удача отвернулась от него!");
                    deaths.Add("BAD|BOLD|Стрела орка его настигла...");

                    Character.Protagonist.Fellowship.Remove(warrior);
                }

                deaths.Add(String.Empty);
            }

            return deaths;
        }
    }
}
