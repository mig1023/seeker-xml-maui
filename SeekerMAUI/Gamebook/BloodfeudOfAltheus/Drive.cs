using System;

namespace SeekerMAUI.Gamebook.BloodfeudOfAltheus
{
    class Drive
    {
        static int[] teams = { 0, 0, 0, 0, 0, 0, 0 };
        static string[] teamsColor = { String.Empty, "BLUE|", "RED|", "YELLOW|", "GREEN|" };
        static string[] names = { String.Empty, "Cиняя", "Красная", "Жёлтая", "Зелёная" };

        public static List<string> Start(bool yourRacing)
        {
            List<string> racing = new List<string> { "ГОНКА НАЧИНАЕТСЯ!" };

            int distance = (yourRacing ? 20 : 10);

            while (true)
            {
                Game.Dice.DoubleRoll(out int firstDice, out int secondDice);
                bool diceDouble = (firstDice == secondDice);
                bool nobodyCantForward = (diceDouble && (teams[firstDice] == -1)) ||
                    ((teams[firstDice] == -1) && (teams[secondDice] == -1));

                racing.Add(String.Empty);
                racing.Add($"BOLD|Следующий бросок: {Game.Dice.Symbol(firstDice)} " +
                    $"и {Game.Dice.Symbol(secondDice)}");

                if ((firstDice == 6) && diceDouble)
                {
                    racing.Add("BAD|Произошло столкновение!");

                    int crashDice = Game.Dice.Roll();

                    racing.Add($"Кубик столкновения: {Game.Dice.Symbol(crashDice)}");

                    if (crashDice < 5)
                    {
                        racing.Add($"BOLD|{names[crashDice]} команда выбывает из гонки!");
                        teams[crashDice] = -1;
                    }
                    else if (crashDice == 6)
                    {
                        racing.Add(String.Empty);
                        racing.Add("BIG|BAD|Произошла серьёзная авария, " +
                            "все колесницы выбывают, гонка остановлена :(");

                        return racing;
                    }
                    else
                    {
                        racing.Add("Происшествие было несерьёзным, все колесницы продолжают гонку");
                    }
                }
                else if (yourRacing && (Character.Protagonist.Patron == "Посейдон") && ((firstDice == 5) || (secondDice == 5)))
                {
                    racing.Add("Сам Посейдон помогает вам: Красная команда продвинулась вперёд!");
                    teams[2] += 1;
                }
                else if ((firstDice == 5) || (secondDice == 5) || nobodyCantForward)
                {
                    racing.Add("Никто не смог продвинуться вперёд");
                }
                else if (yourRacing && Character.Protagonist.IsGodsDisFavor("Посейдон") && ((firstDice == 6) || (secondDice == 6)))
                {
                    racing.Add("Все команды продвинулись вперёд, кроме вашей - " +
                        "сам Посейдон выказывает вам свою немилость!");

                    foreach (int i in new List<int> { 1, 3, 4 })
                        teams[i] += teams[i] >= 0 ? 1 : 0;
                }
                else if ((firstDice == 6) || (secondDice == 6))
                {
                    racing.Add("Все команды продвинулись вперёд");

                    foreach (int i in new List<int> { 1, 2, 3, 4 })
                        teams[i] += teams[i] >= 0 ? 1 : 0;
                }
                else if (firstDice == secondDice)
                {
                    racing.Add($"{names[firstDice]} команда продвинулась сразу на два сектора!");
                    teams[firstDice] += 2;
                }
                else
                {
                    foreach (int i in new List<int> { firstDice, secondDice })
                    {
                        if (teams[i] >= 0)
                        {
                            racing.Add($"{names[i]} команда продвинулась вперёд");
                            teams[i] += 1;
                        }
                    }
                }

                int maxSector = 0;
                bool doubleMaxSector = false;
                int winner = 0;

                racing.Add(String.Empty);

                foreach (int i in new List<int> { 1, 2, 3, 4 })
                {
                    if (teams[i] < 0)
                    {
                        racing.Add($"{teamsColor[i]}{names[i]} команда выбыла из гонки");
                    }
                    else
                    {
                        string path = String.Empty;

                        for (int p = 0; p < teams[i]; p++)
                            path += "|";

                        racing.Add($"{teamsColor[i]}{path}█");

                        if (teams[i] == maxSector)
                        {
                            doubleMaxSector = true;
                        }
                        else if (teams[i] > maxSector)
                        {
                            maxSector = teams[i];
                            doubleMaxSector = false;
                            winner = i;
                        }
                    }
                }

                if ((maxSector >= distance) && !doubleMaxSector)
                {
                    racing.Add(String.Empty);

                    if (yourRacing)
                    {
                        string other = $"BIG|{teamsColor[winner]}Вы проиграли, победила {names[winner]} команда :(";
                        racing.Add(winner == 2 ? "BIG|RED|Вы ПОБЕДИЛИ, Красная команда пришла первой! :)" : other);
                    }
                    else
                    {
                        racing.Add($"BIG|{teamsColor[winner]}Гонка окончена, {names[winner]} команда победила!");
                    }

                    return racing;
                }
            }
        }
    }
}
