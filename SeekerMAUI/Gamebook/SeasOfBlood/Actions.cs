using System;

namespace SeekerMAUI.Gamebook.SeasOfBlood
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public int Less { get; set; }
        public int More { get; set; }
        public int MasteryPenalty { get; set; }
        public bool SilentMonk { get; set; }
        public bool Leech { get; set; }
        public bool FirstBlood { get; set; }

        public List<Character> Enemies { get; set; }

        public override List<string> Status()
        {
            if (Character.Protagonist.Cyclops == null)
            {
                return new List<string>
                {
                    $"Сила команды: {Character.Protagonist.TeamStrength}",
                    $"Численность: {Character.Protagonist.TeamSize}/{Character.Protagonist.MaxTeamSize}",
                    $"День: {Character.Protagonist.Logbook}/50",
                };
            }
            else
            {
                return new List<string>
                {
                    $"Выносливость циклопа: {Character.Protagonist.Cyclops}/16"
                };
            }
        }

        public override List<string> AdditionalStatus() => new List<string>
        {
            $"Мастерство: {Character.Protagonist.Mastery}",
            $"Выносливость: {Character.Protagonist.Endurance}/{Character.Protagonist.MaxEndurance}",
            $"Удачливость: {Character.Protagonist.Luck}/{Character.Protagonist.MaxLuck}",
            $"Золото: {Character.Protagonist.Coins}",
            $"Рабы: {Character.Protagonist.Spoils}",
        };

        public override List<string> Representer()
        {
            List<string> enemies = new List<string>();

            if (Type.StartsWith("HirePirates"))
            {
                string coins = Game.Services.CoinsNoun(Price, "монету", "монеты", "монет");
                string multi = Type.EndsWith("Random") ? "ОВ" : "А";
                return new List<string> { $"НАЙМ ПИРАТ{multi}\nза {Price} {coins}" };
            }

            if (Enemies == null)
            {
                return enemies;
            }

            if (Type == "TeamFight")
            {
                Character enemy = Enemies.First();
                enemies.Add($"{enemy.Name}\nсила {enemy.TeamStrength}  численность {enemy.TeamSize}");
            }
            else
            {
                foreach (Character enemy in Enemies)
                    enemies.Add($"{enemy.Name}\nмастерство {enemy.Mastery}  выносливость {enemy.Endurance}");
            }
            
            return enemies;
        }

        public override bool GameOver(out int toEndParagraph, out string toEndText)
        {
            if (Character.Protagonist.Cyclops == 0)
            {
                toEndParagraph = 311;
                toEndText = "Вы победили циклопа!";
                Character.Protagonist.Cyclops = null;

                return true;
            }
            else
            {
                toEndParagraph = 0;
                toEndText = Output.Constants.GAMEOVER_TEXT;

                bool byEndurance = Character.Protagonist.Endurance <= 0;
                bool byTeamSize = Character.Protagonist.TeamSize <= 0;

                return (byEndurance || byTeamSize);
            }
        }

        public override bool Availability(string option)
        {
            if (String.IsNullOrEmpty(option))
            {
                return true;
            }
            else if (option.StartsWith("СОКРОВИЩА"))
            {
                int level = Game.Services.LevelParse(option);
                int coins = (int)Math.Round((double)Character.Protagonist.Coins / 100) * 100;

                if (level == 100)
                {
                    return coins <= 100;
                }
                else if (level == 800)
                {
                    return coins >= 800;
                }
                else
                {
                    return coins == level;
                }
            }
            else if (option.StartsWith("ДЕНЬГИ >="))
            {
                int money = Game.Services.LevelParse(option);
                return Character.Protagonist.Coins >= money;
            }
            else if (option.EndsWith("ЧИСЛО ДНЕЙ"))
            {
                bool even = Character.Protagonist.Logbook % 2 == 0;
                return option.StartsWith("ЧЕТНОЕ") ? even : !even;
            }
            else
            {
                return AvailabilityTrigger(option);
            }
        }

        public override bool IsButtonEnabled(bool secondButton = false)
        {
            if (Type == "SellSlaves")
            {
                return Character.Protagonist.Spoils > 0;
            }
            else if (Type.StartsWith("HirePirates"))
            {
                bool coins = Character.Protagonist.Coins >= Price;
                bool team = Character.Protagonist.TeamSize < Character.Protagonist.MaxTeamSize;

                return coins && team;
            }
            else
            {
                return true;
            }
        }

        public static bool NoMoreEnemies(List<Character> enemies) =>
            enemies.Where(x => x.Endurance > 0).Count() == 0;

        public List<string> Fight()
        {
            List<string> fight = new List<string>();

            List<Character> FightEnemies = new List<Character>();

            foreach (Character enemy in Enemies)
                FightEnemies.Add(enemy.Clone());

            int round = 1;

            while (true)
            {
                fight.Add($"HEAD|BOLD|Раунд: {round}");

                bool attackAlready = false;
                int protagonistHitStrength = 0;

                foreach (Character enemy in FightEnemies)
                {
                    if (enemy.Endurance <= 0)
                        continue;

                    fight.Add($"{enemy.Name} (выносливость {enemy.Endurance})");

                    if (!attackAlready)
                    {
                        int mastery = Character.Protagonist.Mastery - MasteryPenalty;

                        Game.Dice.DoubleRoll(out int protagonistRollFirst, out int protagonistRollSecond);
                        protagonistHitStrength = protagonistRollFirst + protagonistRollSecond + mastery;

                        fight.Add($"Сила твоего удара: " +
                            $"{Game.Dice.Symbol(protagonistRollFirst)} + " +
                            $"{Game.Dice.Symbol(protagonistRollSecond)} + " +
                            $"{mastery} = {protagonistHitStrength}");
                    }

                    Game.Dice.DoubleRoll(out int enemyRollFirst, out int enemyRollSecond);
                    int enemyHitStrength = enemyRollFirst + enemyRollSecond + enemy.Mastery;

                    fight.Add($"Сила его удара: " +
                        $"{Game.Dice.Symbol(enemyRollFirst)} + " +
                        $"{Game.Dice.Symbol(enemyRollSecond)} + " +
                        $"{enemy.Mastery} = {enemyHitStrength}");

                    if ((protagonistHitStrength > enemyHitStrength) && !attackAlready)
                    {
                        fight.Add($"GOOD|{enemy.Name} ранен");

                        bool alreadyHit = false;

                        if (FirstBlood)
                        {
                            Game.Buttons.Disable("Проиграл");

                            fight.Add(String.Empty);
                            fight.Add("BIG|GOOD|Ты ПОБЕДИЛ великана :)");
                            return fight;
                        }

                        if (Game.Option.IsTriggered("Посох безмолвного монаха"))
                        {
                            int monkStaff = Game.Dice.Roll();

                            fight.Add("GRAY|Бросаем кубик ранений от посоха: " +
                                $"{Game.Dice.Symbol(monkStaff)}");

                            if (monkStaff < 3)
                            {
                                fight.Add("Противник теряет 1 очко Мастерства");
                                enemy.Mastery -= 1;
                                alreadyHit = true;
                            }
                        }

                        if (!alreadyHit)
                        {
                            fight.Add("Противник теряет 2 очка Выносливости");
                            enemy.Endurance -= 2;
                        }

                        if (NoMoreEnemies(FightEnemies))
                            return Win(fight, you: true);
                    }
                    else if ((protagonistHitStrength > enemyHitStrength) && !Leech)
                    {
                        fight.Add($"BOLD|{enemy.Name} не смог тебя ранить");
                    }
                    else if ((protagonistHitStrength < enemyHitStrength) && !Leech)
                    {
                        fight.Add($"BAD|{enemy.Name} ранил тебя");

                        if (FirstBlood)
                        {
                            Game.Buttons.Disable("Победил");

                            fight.Add(String.Empty);
                            fight.Add("BIG|BAD|Ты ПРОИГРАЛ великану :(");
                            return fight;
                        }

                        if (SilentMonk)
                        {
                            int monkDice = Game.Dice.Roll();

                            fight.Add("GRAY|Бросаем кубик ранений от посоха: " +
                                $"{Game.Dice.Symbol(monkDice)}");

                            if (monkDice < 3)
                            {
                                fight.Add("Ты теряешь 2 очка Выносливости обычным образом");
                                Character.Protagonist.Endurance -= 2;
                            }
                            else if (monkDice < 5)
                            {
                                fight.Add("Ты теряешь 1 очко Мастерства");
                                Character.Protagonist.Mastery -= 1;
                            }
                            else
                            {
                                fight.Add("Ты теряешь 1 очко Удачливости");
                                Character.Protagonist.Luck -= 1;
                            }
                        }
                        else
                        {
                            fight.Add("Ты теряешь 2 очка Выносливости");
                            Character.Protagonist.Endurance -= 2;
                        }

                        if (Character.Protagonist.Endurance <= 0)
                            return Fail(fight, you: true);
                    }
                    else if (!Leech)
                    {
                        fight.Add("BOLD|Ничья в раунде");
                    }

                    if (Leech)
                    {
                        fight.Add($"BAD|Пиявка высасывает силы");
                        fight.Add("Ты теряешь 2 очка Выносливости");
                        Character.Protagonist.Endurance -= 2;
                    }

                    attackAlready = true;

                    fight.Add(String.Empty);
                }

                round += 1;
            }
        }

        public List<string> TeamFight() =>
            Team.Fight(this, Enemies);

        public List<string> TeamSizeTest() =>
            Team.SizeTest(More, Less);

        public List<string> Luck()
        {
            Game.Dice.DoubleRoll(out int firstDice, out int secondDice);

            bool goodLuck = (firstDice + secondDice) <= Character.Protagonist.Luck;
            string luckLine = goodLuck ? "<=" : ">";

            List<string> luckCheck = new List<string> {
                $"Проверка удачи: {Game.Dice.Symbol(firstDice)} + " +
                $"{Game.Dice.Symbol(secondDice)} {luckLine} {Character.Protagonist.Luck}" };

            luckCheck.Add(goodLuck ? "BIG|GOOD|УСПЕХ :)" : "BIG|BAD|НЕУДАЧА :(");

            if (Character.Protagonist.Luck > 2)
            {
                Character.Protagonist.Luck -= 1;
                luckCheck.Add("Уровень удачи снижен на единицу");
            }

            Game.Buttons.Disable(goodLuck, "Повезло", $"Не повезло");

            return luckCheck;
        }

        public List<string> SellSlaves()
        {
            List<string> sell = new List<string>();

            int spoils = Character.Protagonist.Spoils;

            sell.Add($"Число невольников: {spoils}");
            sell.Add($"Продажная цена: {Price}");

            int summ = spoils * Price;

            Character.Protagonist.Spoils = 0;
            Character.Protagonist.Coins += summ;

            string coins = Game.Services.CoinsNoun(summ, "монета", "монеты", "монет");
            sell.Add($"BIG|Выручка: {spoils} x {Price} = {summ} {coins}");

            return sell;
        }

        public List<string> DebtGame() =>
            Dices.Games();

        public List<string> DeadByDice() =>
            Dices.Dead();

        public List<string> WoundsByDice() =>
            Dices.Wounds();

        private List<string> Hire(int count)
        {
            Character.Protagonist.Coins -= Price;
            Character.Protagonist.TeamSize += count;

            return new List<string> { "RELOAD" };
        }

        public List<string> HirePirates() =>
            Hire(1);

        public List<string> HirePiratesRandom() =>
            Hire(Game.Dice.Roll());

        public List<string> SlavesByDice() =>
            Dices.Slaves();

        public List<string> DayByDice() =>
            Dices.Day();

        public List<string> PathByDice() =>
            Dices.Path();

        public List<string> MasteryTest()
        {
            List<string> test = new List<string>();

            Game.Dice.DoubleRoll(out int first, out int second);

            int dicesSumm = first + second;

            test.Add($"Бросаем кубики: {Game.Dice.Symbol(first)} + " +
                $"{Game.Dice.Symbol(second)} = {dicesSumm}");

            if (dicesSumm < Character.Protagonist.Mastery)
            {
                test.Add("BIG|BOLD|Выпало МЕНЬШЕ твоего мастерства!");
                Game.Buttons.Disable("Больше или равно уровню твоего МАСТЕРСТВА");
            }
            else
            {
                test.Add("BIG|BOLD|Выпало БОЛЬШЕ или равно твоего мастерства!");
                Game.Buttons.Disable("Меньше уровня твоего МАСТЕРСТВА");
            }

            return test;
        }
    }
}
