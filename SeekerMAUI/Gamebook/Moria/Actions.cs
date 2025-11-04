using System;

namespace SeekerMAUI.Gamebook.Moria
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public List<string> Enemies { get; set; }

        public override List<string> Status()
        {
            if (!Character.Protagonist.Fellowship.Contains("Гэндальф"))
            {
                return new List<string> { "Гэндальф погиб..." };
            }
            else if (Character.Protagonist.MagicPause > 0)
            {
                return new List<string> { $"Гэндальф устал (ещё {Character.Protagonist.MagicPause} параграфа)" };
            }
            else
            {
                return new List<string> { "Гэндальф готов применять магию" };
            }
        }

        public override List<string> AdditionalStatus()
        {
            List<string> fellowship = Constants.Fellowship
                .OrderByDescending(x => x.Value)
                .Select(x => x.Key)
                .ToList();

            List<string> actualFellowship = new List<string>();

            foreach (string person in fellowship)
            {
                bool stillAlive = Character.Protagonist.Fellowship.Contains(person);
                actualFellowship.Add(stillAlive ? person : $"CROSSEDOUT|{person}");
            }

            return actualFellowship;
        }

        public override List<string> Representer()
        {
            if (Enemies == null)
                return new List<string>();

            string name = Enemies[0];
            int strength = Constants.Enemies[name];
            int count = Enemies.Count;
            string line = Game.Services.CoinsNoun(count, "штук", "штуки", "штук");

            return new List<string> { $"{name}\n{strength} сила каждого, всего {count} {line}" };
        }

        public override bool GameOver(out int toEndParagraph, out string toEndText) =>
            GameOverBy(Character.Protagonist.Fellowship.Count, out toEndParagraph, out toEndText);

        public string Declination(string enemy, int count)
        {
            List<string> declin = Constants.Declination[enemy]
                .Split(',')
                .Select(x => x.Trim())
                .ToList();

            string line = Game.Services.CoinsNoun(count, declin[0], declin[1], declin[1]);

            return $"{count} {line}";
        }

        public List<string> Fight()
        {
            List<string> fight = new List<string>();

            if (Game.Option.IsTriggered("Серебряный рог") && Game.Option.IsTriggered("Как использовать рог"))
            {
                fight.Add($"Вы используете Серебряный рог и духи пещер обрушиваются на ваших врагов!");
                fight.Add($"BIG|GOOD|Вы ПОБЕДИЛИ! :)");
                fight.Add($"GRAY|К сожалению, рог теперь бесполезен и его придётся выкинуть...");

                Game.Option.Trigger("Серебряный рог", remove: true);

                return fight;
            }

            var round = 0;

            while (Fights.IsStillSomeoneToFight(this))
            {
                List<string> strongWarriors = Fights.StrongWarriorsInFellowship();

                round += 1;
                fight.Add($"\nHEAD|BOLD|*  *  *   РАУНД: {round}   *  *  *\n");

                if (Enemies.Count <= (strongWarriors.Count * 3))
                {
                    fight.Add($"GRAY|Врагов не так уж много, поэтому против них выходят сильные войны!");
                    fight.Add(String.Empty);

                    int countForEach = Fights.EnemiesForEach(this, strongWarriors.Count);

                    foreach (string warrior in strongWarriors)
                        Fights.Part(this, ref fight, warrior, countForEach);
                }
                else
                {
                    fight.Add($"GRAY|Враги бесчисленны, сразиться придётся каждому!!");
                    fight.Add(String.Empty);

                    int countForEach = Fights.EnemiesForEach(this, Character.Protagonist.Fellowship.Count);

                    List<string> allWarriors = new List<string>(Character.Protagonist.Fellowship);

                    foreach (string warrior in allWarriors)
                        Fights.Part(this, ref fight, warrior, countForEach);
                }

                if (Fights.IsStillSomeoneToFight(this))
                {
                    fight.Add($"GRAY|Осталось ещё {Declination(Enemies[0], Enemies.Count)}!");
                }
            }

            fight.Add(Result(Character.Protagonist.Fellowship.Count > 0, "Вы ПОБЕДИЛИ!", "Вы ПРОИГРАЛИ..."));

            return fight;
        }

        public List<string> Goodluck()
        {
            List<string> diceCheck = new List<string>();

            int dice = Game.Dice.Roll();
            bool coin = dice % 2 == 0;

            if (coin)
            {
                diceCheck.Add("На кубике выпал: Орел");
                diceCheck.Add("BIG|GOOD|BOLD|Удача на вашей стороне! :)");

                Game.Buttons.Disable("Fail");
            }
            else
            {
                diceCheck.Add("На кубике выпала: Решка");
                diceCheck.Add("BIG|BAD|BOLD|Удача отвернулась от вас! :(");

                Game.Buttons.Disable("Win");
            }

            return diceCheck;
        }

        public override bool Availability(string option)
        {
            if (String.IsNullOrEmpty(option))
            {
                return true;
            }
            else if (option.Contains("GandalfMagic"))
            {
                if (!Character.Protagonist.Fellowship.Contains("Гэндальф"))
                    return false;
                else
                    return Character.Protagonist.MagicPause == 0;
            }
            else
            {
                return AvailabilityTrigger(option);
            }
        }

        public List<string> DeathsByArrows() =>
            Events.DeathsByArrows();

        public List<string> Balrog() =>
            Events.Balrog();

        public List<string> RunningUnderArrows() =>
            Events.RunningUnderArrows();

        public List<string> Cast()
        {
            List<string> fight = new List<string>();

            fight.Add("BOLD|Гэндальф творит свои заклятья");
            fight.Add(String.Empty);

            int prev = 0;
            List<string> rounds = new List<string> { "Первый", "Второй", "Третий" };

            for (int i = 1; i <= 3; i++)
            {
                int dice = Game.Dice.Roll();

                fight.Add($"{rounds[i - 1]} бросок Гэндальфа: {Game.Dice.Symbol(dice)}");

                if (dice < prev)
                {
                    fight.Add("BAD|Это меньше предыдущего броска!");
                    fight.Add("BIG|BAD|Волшебство провалено :(");
                    return fight;
                }
                else if (prev > 0)
                {
                    fight.Add("GOOD|Это больше предыдущего броска!");
                }

                prev = dice;
            }

            fight.Add("BIG|GOOD|Волшебство преуспело :)");
            return fight;
        }
    }
}
