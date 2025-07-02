using System;

namespace SeekerMAUI.Gamebook.SwampFever
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public string EnemyName { get; set; }
        public string EnemyCombination { get; set; }

        public int Level { get; set; }
        public bool Birds { get; set; }
        public bool DeathTest { get; set; }

        public override List<string> Representer()
        {
            if (Price > 0)
            {
                string creds = Game.Services.CoinsNoun(Price, "кредит", "кредита", "кредитов");
                return new List<string> { $"{Head}\n{Price} {creds}" };
            }
            else if (Level > 0)
            {
                return new List<string> { $"Ментальная проверка\nуровень {Level}" };
            }
            else if (!String.IsNullOrEmpty(EnemyName))
            {
                return new List<string> { EnemyName };
            }
            else
            {
                return new List<string> { };
            }
        }

        public override List<string> AdditionalStatus() => new List<string>
        {
            $"Ярость: {Constants.GetFuryLevel[Character.Protagonist.Fury]}",
            $"Креды: {Character.Protagonist.Creds}",
            $"Стигон: {Character.Protagonist.Stigon}/6",
            $"Котировка: 1:{Character.Protagonist.Rate}",
        };

        public override bool GameOver(out int toEndParagraph, out string toEndText) =>
            GameOverBy(Character.Protagonist.HarversterDestroyed, out toEndParagraph, out toEndText);

        public override bool IsButtonEnabled(bool secondButton = false)
        {
            bool byPrice = (Price > 0) && (Character.Protagonist.Creds < Price);          
            bool byMembrane = (Type == "SellAcousticMembrane") && (Character.Protagonist.AcousticMembrane <= 0);
            bool byMucus = (Type == "SellLiveMucus") && (Character.Protagonist.LiveMucus <= 0);

            bool byUsed = false;

            if ((Type == "Get") && (Head == "Серенитатин"))
            {
                return (GetProperty(Character.Protagonist, Benefit.Name) > -2) && (Character.Protagonist.Creds > 0);
            }
            else
            {
                bool already = Benefit != null && (GetProperty(Character.Protagonist, Benefit.Name) > 0);
                byUsed = (String.IsNullOrEmpty(EnemyName) && (Benefit != null) && already);
            }

            return !(byPrice || byUsed || byMembrane || byMucus);
        }

        public override bool AvailabilityNode(string option) =>
            AvailabilityTrigger(option);

        public List<string> MentalTest()
        {
            int mentalDice = Game.Dice.Roll();
            int fury = Character.Protagonist.Fury;
            int mentalAndFury = mentalDice + fury;
            int level = Level;
            string furyLine = fury < 0 ? "-" : "+";

            List<string> mentalCheck = new List<string>
            {
                $"Ментальная проверка (по уровню {level}):",
                $"1. Бросок кубика: {Game.Dice.Symbol(mentalDice)}",
                $"2. {furyLine}{Math.Abs(fury)} к броску за уровень Ярости",
            };

            int ord = 3;

            if (Character.Protagonist.Harmonizer > 0)
            {
                level += 1;
                ord += 1;
                mentalCheck.Add($"3. +1 к уровню проверки за Гармонизатор (теперь уровень {level})");
            }

            bool success = level > mentalAndFury;
            string not = success ? String.Empty : "не ";

            mentalCheck.Add($"{ord}. " +
                $"Итого получаем {mentalAndFury}, что " +
                $"{not}меньше {level} уровня проверки");

            mentalCheck.Add(Result(success, "УСПЕХ", "НЕУДАЧА"));

            if (DeathTest && !success)
            {
                mentalCheck.Add("\nBOLD|Ваш харвестер подбит и уничтожен :(");
                Character.Protagonist.HarversterDestroyed = true;
            }

            return mentalCheck;
        }

        public List<string> Get()
        {
            if ((Price > 0) && (Character.Protagonist.Creds >= Price))
            {
                Character.Protagonist.Creds -= Price;

                if (Benefit != null)
                    Benefit.Do();
            }

            return new List<string> { "RELOAD" };
        }

        private bool Upgrade(ref List<int> myCombination, ref List<string> myCombinationLine, ref List<string> fight)
        {
            int upgrades = 0;

            bool upgradeInAction = false;

            for (int i = 1; i <= Constants.GetUpgrates.Count; i++)
                upgrades += GetProperty(Character.Protagonist, Details.GetUpgratesValues(i, part: 1));

            if (upgrades == 0)
                return upgradeInAction;

            fight.Add(String.Empty);

            int upgradeDice = Game.Dice.Roll();

            fight.Add($"Кубик проверки апгрейда: {Game.Dice.Symbol(upgradeDice)}");

            for (int i = 1; i <= Constants.GetUpgrates.Count; i++)
            {
                if (GetProperty(Character.Protagonist, Details.GetUpgratesValues(i, part: 1)) == 0)
                    continue;

                bool inAction = upgradeDice == i;
                string good = inAction ? "GOOD|" : String.Empty;
                string action = inAction ? "В ДЕЙСТВИИ!" : "нет";

                fight.Add($"{good}{Details.GetUpgratesValues(i, part: 2)} - {action}");

                if (inAction)
                {
                    myCombination.Add(upgradeDice);
                    myCombinationLine.Add(Game.Dice.Symbol(upgradeDice));
                    upgradeInAction = true;
                }
            }

            fight.Add(String.Empty);

            return upgradeInAction;
        }

        private string ToTxt(int num, bool all = false) =>
            all ? Constants.AllNumTexts[num] : Constants.NumTexts[num];

        public List<string> Fight()
        {
            List<string> fight = new List<string>();

            Dictionary<int, string> rangeType = Constants.GetRangeTypes;

            List<int> myCombination = new List<int>();
            List<string> myCombinationLine = new List<string>();

            int combinationLength = 6 + Character.Protagonist.Fury;

            for (int i = 0; i < combinationLength; i++)
            {
                int dice = Game.Dice.Roll();
                myCombination.Add(dice);
                myCombinationLine.Add(Game.Dice.Symbol(dice));
            }

            string combination = String.Join(String.Empty, myCombinationLine.ToArray());
            fight.Add($"Ваша комбинация: {combination}");

            List<int> enemyCombination = new List<int>();
            List<string> enemyCombinationLine = new List<string>();

            foreach (string dice in EnemyCombination.Split('-'))
            {
                int enemyNumber = int.Parse(dice);
                enemyCombination.Add(enemyNumber);
                enemyCombinationLine.Add(Game.Dice.Symbol(enemyNumber));
            }

            string lineCombination = String.Join(String.Empty, enemyCombinationLine.ToArray());
            fight.Add($"Его комбинация: {lineCombination}");

            if (Upgrade(ref myCombination, ref myCombinationLine, ref fight))
            {
                string newCombination = String.Join(String.Empty, myCombinationLine.ToArray());
                fight.Add($"Теперь ваша комбинация: {newCombination}");
            }

            bool birds = Birds;

            while (true)
            {
                if (myCombination.Contains(1))
                {
                    fight.Add(String.Empty);
                    fight.Add("BOLD|МАНЕВРИРОВАНИЕ");

                    int maneuvers = Details.CountInCombination(myCombination, 1);
                    bool failManeuvers = true;

                    foreach (int dice in new int[] { 6, 5, 4 })
                    {
                        for (int i = 0; i < enemyCombination.Count; i++)
                        {
                            if ((enemyCombination[i] == dice) && (maneuvers > 0))
                            {
                                fight.Add($"Убираем у противника {ToTxt(dice)} за ваше маневрирование");
                                enemyCombination[i] = 0;
                                maneuvers -= 1;
                                failManeuvers = false;
                            }
                        }
                    }

                    if (failManeuvers)
                    {
                        fight.Add("Маневрирование ничего не дало противникам");
                    }
                }

                foreach (int range in new int[] { 6, 5, 4 })
                {
                    fight.Add(String.Empty);
                    fight.Add($"BOLD|{rangeType[range]}");

                    int roundResult = 0;

                    if (!myCombination.Contains(range) && !enemyCombination.Contains(range))
                    {
                        fight.Add("Противникам нечего друг другу противопоставить");
                    }
                    else if (myCombination.Contains(range) && !enemyCombination.Contains(range))
                    {
                        roundResult = 1;

                        if (range == 4)
                        {
                            fight.Add("BIG|GOOD|Вы уничтожили противника тараном, оружием героев :)");

                            if (Benefit != null)
                                Benefit.Do();

                            return fight;
                        }
                        else
                        {
                            fight.Add("GOOD|Вы накрыли противника огнём");
                        }
                    }
                    else if (!myCombination.Contains(range) && enemyCombination.Contains(range))
                    {
                        roundResult = -1;

                        if (range == 4)
                        {
                            fight.Add("BIG|BAD|Противник уничтожил вас тараном :(");
                            Character.Protagonist.HarversterDestroyed = true;
                            return fight;
                        }
                        else
                        {
                            fight.Add("BAD|Противник накрыл вас огнём");
                        }
                    }
                    else
                    {
                        fight.Add(range == 4 ? "Взаимные манёвры:" : "Перестрелка:");

                        while (roundResult == 0)
                        {
                            string bonuses = String.Empty;

                            int myDice = Game.Dice.Roll();
                            int myBonus = Details.CountInCombination(myCombination, range);
                            int myAttack = myDice + myBonus;

                            if (myBonus > 0)
                                bonuses = $", +{myBonus} за {ToTxt(range, all: true)}, итого {myAttack}";

                            fight.Add($"Ваша атака: {Game.Dice.Symbol(myDice)}{bonuses}");

                            bonuses = String.Empty;

                            int enemyDice = Game.Dice.Roll();
                            int enemyBonus = Details.CountInCombination(enemyCombination, range);
                            int enemyAttack = enemyDice + enemyBonus;

                            if (enemyBonus > 0)
                                bonuses = $", +{enemyBonus} за {ToTxt(range, all: true)}, итого {enemyAttack}";


                            fight.Add($"Атака противника: {Game.Dice.Symbol(enemyDice)}{bonuses}");

                            if ((myAttack > enemyAttack) && (range == 4))
                            {
                                fight.Add("BIG|GOOD|Вы уничтожили противника тараном, оружием героев :)");

                                if (Benefit != null)
                                    Benefit.Do();

                                return fight;
                            }
                            else if ((myAttack < enemyAttack) && (range == 4))
                            {
                                fight.Add("BIG|BAD|Противник уничтожил вас тараном :(");
                                Character.Protagonist.HarversterDestroyed = true;
                                return fight;
                            }
                            else if (myAttack > enemyAttack)
                            {
                                roundResult = 1;
                                fight.Add("GOOD|Вы подавили противника огнём");
                            }
                            else if (myAttack < enemyAttack)
                            {
                                roundResult = -1;
                                fight.Add("BAD|Противник подавил вас огнём");
                            }
                            else
                            {
                                fight.Add("Перестрелка продолжается:");
                            }
                        }
                    }

                    if (roundResult == 1)
                    {
                        string bonuses = String.Empty, penalties = String.Empty;

                        int myDice = Game.Dice.Roll();
                        int myBonus = Details.CountInCombination(myCombination, 3);
                        int myPenalty = Details.CountInCombination(enemyCombination, 2);
                        int enemyEvasion = myDice + myBonus - myPenalty;

                        if (myBonus > 0)
                            bonuses = $", +{myBonus} за ваши тройки";

                        if (myPenalty > 0)
                            penalties = $", -{myPenalty} за его двойки";

                        fight.Add($"Противник пытется уклониться: " +
                            $"{Game.Dice.Symbol(myDice)}{bonuses}{penalties}, " +
                            $"итого {enemyEvasion} - это " +
                            $"{Game.Services.Сomparison(enemyEvasion, 2)} " +
                            $"порогового значения 'два'");

                        if (enemyEvasion > 2)
                        {
                            if (birds)
                            {
                                fight.Add("GOOD|Вы уничтожили одну из птиц");

                                birds = false;

                                foreach (int dice in new int[] { 5, 4, 3 })
                                    enemyCombination.RemoveAt(dice);
                            }
                            else
                            {
                                fight.Add("BIG|GOOD|Вы уничтожили противника :)");

                                if (Benefit != null)
                                    Benefit.Do();

                                return fight;
                            }
                        }
                        else
                            fight.Add("Противник смог уклониться");
                    }
                    else if (roundResult == -1)
                    {
                        string bonuses = String.Empty, penalties = String.Empty;

                        int enemyDice = Game.Dice.Roll();
                        int enemyBonus = Details.CountInCombination(enemyCombination, 3);
                        int enemyPenalty = Details.CountInCombination(myCombination, 2);
                        int myEvasion = enemyDice + enemyBonus - enemyPenalty;

                        if (enemyBonus > 0)
                            bonuses = $", +{enemyBonus} за его тройки";

                        if (enemyPenalty > 0)
                            penalties = $", -{enemyPenalty} за ваши двойки";

                        fight.Add($"Вы пытется уклониться: " +
                            $"{Game.Dice.Symbol(enemyDice)}{bonuses}{penalties}, " +
                            $"итого {myEvasion} - это " +
                            $"{Game.Services.Сomparison(myEvasion, 2)} " +
                            $"порогового значения 'два'");

                        if (myEvasion > 2)
                        {
                            fight.Add("BIG|BAD|Противник уничтожил вас :(");
                            Character.Protagonist.HarversterDestroyed = true;
                            return fight;
                        }
                        else
                            fight.Add("Вы смогли уклониться");
                    }
                }

                fight.Add("BOLD|Бой окончился ничьёй");

                return fight;
            }
        }

        public List<string> SellStigon()
        {
            List<string> accountingReport = new List<string>();

            int soldStigon = Character.Protagonist.Stigon;
            int earnedCreds = 0, sellNum = 0;

            accountingReport.Add($"В вашем грузовом отсеке {Character.Protagonist.Stigon} кубометров стигона");
            accountingReport.Add($"Курс стигона на начало продажи: 1:{Character.Protagonist.Rate}"); 

            while (Character.Protagonist.Stigon > 0)
            {
                sellNum += 1;
                accountingReport.Add(String.Empty);

                Character.Protagonist.Stigon -= 1;
                earnedCreds += Character.Protagonist.Rate;
                accountingReport.Add($"BOLD|Продажа {sellNum} кубометра стигона: +{Character.Protagonist.Rate} кредов");
                accountingReport.Add($"Подытог к зачислению: {earnedCreds} кредов");

                if (Character.Protagonist.Rate > 5)
                {
                    Character.Protagonist.Rate -= 5;
                    accountingReport.Add($"Курс стигона упал до: {Character.Protagonist.Rate} кредов");
                }
                else
                {
                    accountingReport.Add("Курсу стигона уже некуда падать...");
                }
            }

            accountingReport.Add(String.Empty);
            accountingReport.Add("BIG|ИТОГО:");
            accountingReport.Add($"Вы продали: {soldStigon} кубометров стигона");
            accountingReport.Add($"BOLD|GOOD|Вы получили по плавающему курсу: {earnedCreds} кредов");

            Character.Protagonist.Creds += earnedCreds;

            return accountingReport;
        }

        public List<string> VariousPurchases()
        {
            List<string> purchasesReport = new List<string>();

            bool affordable = false, anything = false;
            bool? prevAffordable = null;

           
            if (Character.Protagonist.Creds >= 1500)
            {
                purchasesReport.Add("GOOD|BOLD|ВАМ ДОСТУПНО ВСЁ!! :)\n");
                anything = true;
            }
            else if (Character.Protagonist.Creds < 5)
            {
                purchasesReport.Add("BAD|BOLD|ВАМ НЕ ДОСТУПНО НИЧЕГО!! :(\n");
                anything = true;
            }
                
            foreach (KeyValuePair<string, int> purchase in Constants.GetPurchases.OrderBy(x => x.Value))
            {
                affordable = (purchase.Value <= Character.Protagonist.Creds) && !anything;
                string affLine = (affordable ? "GOOD|BOLD|" : String.Empty);

                if (!anything)
                    Details.PurchasesHeads(ref purchasesReport, affordable, prevAffordable);

                purchasesReport.Add($"{affLine}{purchase.Key} — {purchase.Value} кредов.");

                prevAffordable = affordable;
            }

            return purchasesReport;
        }

        public List<string> SellAcousticMembrane()
        {
            Character.Protagonist.Creds += 100;
            Character.Protagonist.AcousticMembrane -= 1;

            return new List<string> { "RELOAD" };
        }

        public List<string> SellLiveMucus()
        {
            Character.Protagonist.Creds += 100;
            Character.Protagonist.LiveMucus -= 1;

            return new List<string> { "RELOAD" };
        }

        public List<string> TrackPull() =>
            Minigames.TrackPull(this);

        public List<string> PropellersPull() =>
            Minigames.PropellersPull(this);

        public List<string> TugOfWar() =>
            Minigames.TugOfWar(this);

        public List<string> Hunt() => 
            Minigames.Hunt();

        public List<string> Pursuit() =>
            Minigames.Pursuit();

        public List<string> SulfurCavity() =>
            Minigames.SulfurCavity();
    }
}
