using System;
using System.Collections.Generic;
using System.Linq;

namespace SeekerMAUI.Gamebook.Moonrunner
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public List<Character> Enemies { get; set; }

        public int DevastatingAttack { get; set; }
        public int RoundsToFight { get; set; }
        public int WoundsLimit { get; set; }
        public bool ThisIsSkill { get; set; }
        public bool BitesEveryRound { get; set; }
        public bool Invulnerable { get; set; }
        public bool EnemyMasteryInc { get; set; }
        public bool DoubleFail { get; set; }
        public bool ThreeDiceAttack { get; set; }
        public string Stat { get; set; }

        public override List<string> Status() => new List<string>
        {
            $"Мастерство: {Character.Protagonist.Mastery}",
            $"Выносливость: {Character.Protagonist.Endurance}/{Character.Protagonist.MaxEndurance}",
            $"Удача: {Character.Protagonist.Luck}",
            $"Золото: {Character.Protagonist.Gold}",
        };

        public override bool AvailabilityNode(string option)
        {
            if (Game.Services.AvailabilityByСomparison(option))
            {
                var fail = Game.Services.AvailabilityByProperty(Character.Protagonist,
                    option, Constants.Availabilities, onlyFailTrueReturn: true);

                return !fail;
            }
            else
            {
                return AvailabilityTrigger(option);
            }
        }

        public override bool Availability(string option)
        {
            if (String.IsNullOrEmpty(option))
            {
                return true;
            }
            else if (option.Contains(";"))
            {
                string[] options = option.Split(';');

                int optionMustBe = int.Parse(options[0]);
                string direction = options[1];
                int optionCount = options.Where(x => Game.Option.IsTriggered(x.Trim())).Count();

                switch (direction)
                {
                    case "less":
                        return optionCount < optionMustBe;

                    case "more":
                        return optionCount > optionMustBe;

                    case "exactly":
                    default:
                        return optionCount == optionMustBe;
                }
            }
            else
            {
                return base.Availability(option);
            }
        }

        public override bool GameOver(out int toEndParagraph, out string toEndText) =>
            GameOverBy(Character.Protagonist.Endurance, out toEndParagraph, out toEndText);

        public override bool IsButtonEnabled(bool secondButton = false)
        {
            if (!String.IsNullOrEmpty(Stat))
            {
                int stat = GetProperty(Character.Protagonist, Stat);

                if (secondButton)
                {
                    return stat > 0;
                }
                else
                {
                    return stat < Character.Protagonist.Gold;
                }
            }

            bool disabledSkillSlots = ThisIsSkill && (Character.Protagonist.SkillSlots < 1);
            bool disabledSkillAlready = ThisIsSkill && Game.Option.IsTriggered(Head);

            return !(disabledSkillSlots || disabledSkillAlready || Used);
        }

        public override List<string> Representer()
        {
            List<string> enemies = new List<string>();

            if (Price > 0)
            {
                string gold = Game.Services.CoinsNoun(Price, "золотой", "золотых", "золотых");
                return new List<string> { $"{Head}\n{Price} {gold}" };
            }
            else if (!String.IsNullOrEmpty(Stat))
            {
                int currentStat = GetProperty(Character.Protagonist, Stat);
                string diffLine = String.Empty;

                if (currentStat > 0)
                {
                    string count = Game.Services.CoinsNoun(currentStat, "единица", "единицы", "единицы");
                    diffLine = $"\nтекущее: {currentStat} {count}";
                }

                return new List<string> { $"{Head}{diffLine}" };
            }
            else if (ThisIsSkill)
            {
                return new List<string> { Head };
            }

            if (Enemies == null)
                return enemies;

            foreach (Character enemy in Enemies)
            {
                if (enemy.Endurance > 0)
                {
                    enemies.Add($"{enemy.Name}\nмастерство {enemy.Mastery}  " +
                        $"выносливость {enemy.Endurance}");
                }
                else
                {
                    enemies.Add($"{enemy.Name}\nмастерство {enemy.Mastery} ");
                }
            }

            return enemies;
        }

        public List<string> Get()
        {
            if (!String.IsNullOrEmpty(Stat))
            {
                SetProperty(Character.Protagonist, Stat, GetProperty(Character.Protagonist, Stat) + 1);
            }
            else if (ThisIsSkill && (Character.Protagonist.SkillSlots >= 1))
            {
                Game.Option.Trigger(Head);
                Character.Protagonist.SkillSlots -= 1;
            }
            else if ((Price > 0) && (Character.Protagonist.Gold >= Price))
            {
                Character.Protagonist.Gold -= Price;

                Used = true;

                if (Benefit != null)
                    Benefit.Do();
            }

            return new List<string> { "RELOAD" };
        }

        public List<string> Decrease() => 
            ChangeProtagonistParam(Stat, Character.Protagonist, String.Empty, decrease: true);

        public List<string> ThreeDice() =>
            Dices.Three();

        public List<string> DiceWounds() =>
            Dices.Wounds();

        public List<string> SpellsDice() =>
            Dices.Spells();

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

                if (BitesEveryRound && (Character.Protagonist.Endurance > 1))
                {
                    fight.Add("BAD|Из-за укусов вы теряете одну Выносливость!");
                    Character.Protagonist.Endurance -= 1;
                }

                foreach (Character enemy in FightEnemies)
                {
                    if ((enemy.Endurance <= 0) && !Invulnerable)
                        continue;

                    if (Invulnerable)
                    {
                        fight.Add(enemy.Name);
                    }
                    else
                    {
                        fight.Add($"{enemy.Name} (выносливость {enemy.Endurance})");
                    }

                    if (!attackAlready)
                    {
                        Game.Dice.DoubleRoll(out int protagonistRollFirst, out int protagonistRollSecond);
                        protagonistHitStrength = protagonistRollFirst + protagonistRollSecond + Character.Protagonist.Mastery;

                        fight.Add($"Сила вашего удара: " +
                            $"{Game.Dice.Symbol(protagonistRollFirst)} + " +
                            $"{Game.Dice.Symbol(protagonistRollSecond)} + " +
                            $"{Character.Protagonist.Mastery} = {protagonistHitStrength}");
                    }

                    int enemyHitStrength = 0;

                    if (ThreeDiceAttack && !Game.Option.IsTriggered("Акробатика"))
                    {
                        List<int> dices = Fights.TripleDiceRoll(out int failIndex);

                        enemyHitStrength += dices.Sum() - dices[failIndex] + enemy.Mastery;

                        fight.Add($"Сила его удара: " +
                            $"{Game.Dice.Symbol(dices[0])} + " +
                            $"{Game.Dice.Symbol(dices[1])} + " +
                            $"{Game.Dice.Symbol(dices[2])} " +
                            $"(отбрасываем наименьшее значение: {dices[failIndex]}) + " +
                            $"{enemy.Mastery} = {enemyHitStrength}");
                    }
                    else
                    {
                        Game.Dice.DoubleRoll(out int enemyRollFirst, out int enemyRollSecond);
                        enemyHitStrength = enemyRollFirst + enemyRollSecond + enemy.Mastery;

                        fight.Add($"Сила его удара: " +
                            $"{Game.Dice.Symbol(enemyRollFirst)} + " +
                            $"{Game.Dice.Symbol(enemyRollSecond)} + " +
                            $"{enemy.Mastery} = {enemyHitStrength}");

                        if (DoubleFail && (enemyRollFirst == enemyRollSecond))
                        {
                            fight.Add(String.Empty);
                            fight.Add("BOLD|У противника выпал дубль!");
                            return fight;
                        }
                    }

                    if ((protagonistHitStrength > enemyHitStrength) && (!attackAlready || Game.Option.IsTriggered("Сражение")))
                    {
                        if (Invulnerable)
                        {
                            fight.AddRange(Luck.Check(out bool goodLuck));

                            if (goodLuck)
                                return fight;
                        }
                        else
                        {
                            fight.Add($"GOOD|{enemy.Name} ранен");

                            enemy.Endurance -= 2;

                            bool enemyLost = Fights.NoMoreEnemies(FightEnemies, WoundsLimit);

                            if (enemyLost)
                                return Win(fight);
                        }
                    }
                    else if (protagonistHitStrength > enemyHitStrength)
                    {
                        fight.Add($"BOLD|{enemy.Name} не смог вас ранить");
                    }
                    else if (protagonistHitStrength < enemyHitStrength)
                    {
                        fight.Add($"BAD|{enemy.Name} ранил вас");

                        Character.Protagonist.Endurance -= (DevastatingAttack > 0 ? DevastatingAttack : 2);

                        if (Character.Protagonist.Endurance <= 0)
                            return Fail(fight);

                        if (EnemyMasteryInc)
                        {
                            fight.Add("BOLD|Мастерство противника увеличилось на единицу");
                            enemy.MaxMastery += 1;
                            enemy.Mastery += 1;
                        }
                    }
                    else
                    {
                        fight.Add("BOLD|Ничья в раунде");
                    }

                    attackAlready = true;

                    if ((RoundsToFight > 0) && (RoundsToFight <= round))
                    {
                        fight.Add(String.Empty);
                        fight.Add("BOLD|Отведённые на победу раунды истекли.");
                        return fight;
                    }

                    fight.Add(String.Empty);
                }

                round += 1;
            }
        }

        public override bool IsHealingEnabled() =>
            Character.Protagonist.Endurance < Character.Protagonist.MaxEndurance;

        public override void UseHealing(int healingLevel) =>
            Character.Protagonist.Endurance += healingLevel;
    }
}
