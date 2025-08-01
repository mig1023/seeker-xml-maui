﻿using System;
using static SeekerMAUI.Gamebook.YounglingTournament.Character;

namespace SeekerMAUI.Gamebook.YounglingTournament
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public List<Character> Enemies { get; set; }
        public string Enemy { get; set; }
        public int HeroHitpointsLimith { get; set; }
        public int EnemyHitpointsLimith { get; set; }
        public int HeroRoundWin { get; set; }
        public int EnemyRoundWin { get; set; }
        public bool SpeedActivate { get; set; }
        public bool WithoutTechnique { get; set; }
        public bool NoStrikeBack { get; set; }
        public int EnemyHitpointsPenalty { get; set; }
        public string BonusTechnique { get; set; }

        public int AccuracyBonus { get; set; }
        public int Level { get; set; }
        

        public override List<string> Status() => new List<string>
        {
            $"Cветлая сторона: {Character.Protagonist.LightSide}",
            $"Тёмная сторона: {Character.Protagonist.DarkSide}",
        };

        public override List<string> AdditionalStatus()
        {
            List<string> newStatuses = new List<string>();

            newStatuses.Add($"Выносливость: {Character.Protagonist.Hitpoints}/{Character.Protagonist.MaxHitpoints}");

            if (Character.Protagonist.SecondPart == 0)
            {
                newStatuses.Add($"Взлом: {Character.Protagonist.Hacking}");
                newStatuses.Add($"Пилот: {Character.Protagonist.Pilot}");
                newStatuses.Add($"Меткость: {Character.Protagonist.Accuracy}");
            }
            else
            {
                if ((Character.Protagonist.Thrust > 0) || (Character.Protagonist.EnemyThrust > 0))
                    newStatuses.Add($"Уколов: {Character.Protagonist.Thrust} vs {Character.Protagonist.EnemyThrust}");

                newStatuses.Add($"Понимание Силы: {Character.Protagonist.ForceTechniques.Values.Sum()}");
                newStatuses.Add($"Форма {Fights.GetSwordSkillName(Fights.GetSwordType())}");
            }

            return newStatuses;
        }
            
        public override bool Availability(string option)
        {
            bool thisIsTechnique = Enum.TryParse(option, out Character.ForcesTypes techniqueType);

            if (thisIsTechnique && (Character.Protagonist.ForceTechniques[techniqueType] == 0))
            {
                return false;
            }
            else if (Game.Services.AvailabilityByСomparison(option))
            {
                var fail = Game.Services.AvailabilityByProperty(Character.Protagonist,
                    option, Constants.Availabilities, onlyFailTrueReturn: true);

                if (fail)
                    return false;

                int level = Game.Services.LevelParse(option);

                if (option.Contains("УКОЛОВ >") && (level >= Character.Protagonist.Thrust))
                    return false;

                if (option.Contains("УКОЛОВ У ВРАГА >") && (level >= Character.Protagonist.EnemyThrust))
                    return false;

                return true;
            }
            else
            {
                return AvailabilityTrigger(option);
            }
        }

        public override List<string> Representer()
        {
            if (Level > 0)
                return new List<string> { $"Пройдите проверку Понимания Силы\nсложность {Level}" };

            List<string> enemies = new List<string>();

            if ((Enemies == null) || (Type == "EnemyDiceWounds"))
                return enemies;

            foreach (Character enemy in Enemies)
            {
                string accuracy = (enemy.Accuracy > 0 ? $"  меткость {enemy.Accuracy}  " : String.Empty);
                string firepower = (enemy.Firepower > 5 ? $"  сила выстрела {enemy.Firepower}" : String.Empty);
                string shield = (enemy.Shield > 0 ? $"  энергощит {enemy.Shield}" : String.Empty);
                string skill = (enemy.Skill > 0 ? $"  ловкость {enemy.Skill}" : String.Empty);
                string technique = String.Empty, noStrikeBack = String.Empty;

                if (enemy.Rang > 0)
                {
                    bool anotherTechnique = Enum.TryParse(enemy.SwordTechnique,
                        out SwordTypes currectSwordTechniques);

                    if (!anotherTechnique)
                        currectSwordTechniques = SwordTypes.Rivalry;

                    technique = $"\nиспользует Форму " +
                        $"{Fights.GetSwordSkillName(currectSwordTechniques, rang: enemy.Rang)}";
                }

                if (NoStrikeBack)
                    noStrikeBack = "\nзнает защиту от Встречного удара";

                enemies.Add($"{enemy.Name}\n{accuracy}выносливость " +
                    $"{enemy.GetHitpoints(EnemyHitpointsPenalty)}" +
                    $"{firepower}{shield}{skill}{technique}{noStrikeBack}");
            }

            return enemies;
        }

        public List<string> DiceWounds() =>
            Dice.Wounds();

        public List<string> EnemyDiceWounds() =>
            Dice.EnemyWounds(this);
        
        public List<string> ForceTest()
        {
            List<string> test = new List<string>();

            int testDice = Game.Dice.Roll();
            int forceLevel = Character.Protagonist.ForceTechniques.Values.Sum();
            bool testPassed = testDice + forceLevel >= Level;
            string testLine = testPassed ? ">=" : "<";

            test.Add($"Проверка Понимания: " +
                $"{Game.Dice.Symbol(testDice)} + " +
                $"{forceLevel} {testLine} {Level}");

            test.Add(Result(testPassed, "ПРОВЕРКА ПРОЙДЕНА", "ПРОВЕРКА ПРОВАЛЕНА"));

            Game.Buttons.Disable(testPassed, "Все в порядке", "Не в порядке");

            return test;
        }

        public List<string> MixedFightAttack() =>
            MixedFight.Attack();

        public List<string> MixedFightDefence() =>
            MixedFight.Defence();

        public List<string> FireFight()
        {
            List<string> fight = new List<string>();

            Dictionary<Character, int> FightEnemies = new Dictionary<Character, int>();
            List<Character> EnemiesList = new List<Character>();

            foreach (Character enemy in Enemies)
            {
                Character newEnemy = enemy.Clone();
                FightEnemies.Add(newEnemy, 0);
                EnemiesList.Add(newEnemy);
            }
                
            int round = 1;

            while (true)
            {
                fight.Add($"HEAD|BOLD|Раунд: {round}");

                Game.Dice.DoubleRoll(out int protagonistFirstDice,
                    out int protagonistSecondDice);

                int shotAccuracy = Character.Protagonist.Accuracy + 
                    protagonistFirstDice + protagonistSecondDice + AccuracyBonus;

                string bonus = (AccuracyBonus > 0 ? $" + {AccuracyBonus} бонус" : String.Empty);

                fight.Add($"Ваш выстрел: " +
                    $"{Character.Protagonist.Accuracy} меткость{bonus} + " +
                    $"{Game.Dice.Symbol(protagonistFirstDice)} + " +
                    $"{Game.Dice.Symbol(protagonistSecondDice)} = {shotAccuracy}");

                foreach (Character enemy in EnemiesList)
                {
                    if (enemy.Hitpoints <= 0)
                    {
                        FightEnemies[enemy] = -1;
                    }
                    else
                    {
                        Game.Dice.DoubleRoll(out int enemyFirstDice, out int enemySecondDice);
                        FightEnemies[enemy] = enemy.Accuracy + enemyFirstDice + enemySecondDice;

                        fight.Add($"{enemy.Name} стреляет: " +
                            $"{enemy.Accuracy} + " +
                            $"{Game.Dice.Symbol(enemyFirstDice)} + " +
                            $"{Game.Dice.Symbol(enemySecondDice)} = {FightEnemies[enemy]}");
                    }
                }

                bool protaganistMakeShoot = false;

                foreach (KeyValuePair<Character, int> shooter in FightEnemies.OrderBy(x => x.Value))
                {
                    if (shooter.Value <= 0)
                    {
                        continue;
                    }
                    else if ((shooter.Value < shotAccuracy) && !protaganistMakeShoot)
                    {
                        protaganistMakeShoot = true;

                        if (shooter.Key.Shield > 0)
                        {
                            int damage = (Character.Protagonist.Firepower - shooter.Key.Shield);

                            if (damage <= 0)
                            {
                                fight.Add($"GOOD|Вы подстрелили {shooter.Key.Name}, " +
                                    $"но его энергощит полностью поглотил урон");

                                shooter.Key.Shield -= Character.Protagonist.Firepower;
                            }
                            else
                            {
                                fight.Add($"GOOD|Вы подстрелили " +
                                    $"{shooter.Key.Name}, его энергощит " +
                                    $"поглотил {shooter.Key.Shield} ед.урона, " +
                                    $"в результате он потерял {damage} " +
                                    $"ед.выносливости");

                                shooter.Key.Hitpoints -= damage;
                                shooter.Key.Shield = 0;
                            }
                        }
                        else
                        {
                            shooter.Key.Hitpoints -= Character.Protagonist.Firepower;
                            fight.Add($"GOOD|Вы подстрелили {shooter.Key.Name}, " +
                                $"он потерял {Character.Protagonist.Firepower} ед.выносливости");
                        }
                    }
                    else if (shooter.Value > shotAccuracy)
                    {
                        Character.Protagonist.Hitpoints -= shooter.Key.Firepower;

                        fight.Add($"BAD|{shooter.Key.Name} " +
                            $"подстрелил вас, вы потерял " +
                            $"{shooter.Key.Firepower} ед.выносливости " +
                            $"(осталось {Character.Protagonist.Hitpoints})");
                    }
                }

                if (Character.Protagonist.Hitpoints <= 0)
                    return Fail(fight);

                if (FightEnemies.Keys.Where(x => x.Hitpoints > 0).Count() == 0)
                    return Win(fight);

                fight.Add(String.Empty);

                round += 1;
            }
        }

        public List<string> SwordFight()
        {
            List<string> fight = new List<string>();

            Dictionary<Character, int> FightEnemies = new Dictionary<Character, int>();
            List<Character> EnemiesList = new List<Character>();

            foreach (Character enemy in Enemies)
            {
                Character newEnemy = enemy.Clone().SetHitpoints(EnemyHitpointsPenalty);
                FightEnemies.Add(newEnemy, 0);
                EnemiesList.Add(newEnemy);
            }

            SwordTypes currectSwordTechniques = Fights.GetSwordType();

            fight.Add($"Вы выбрали для боя Форму " +
                $"{Fights.GetSwordSkillName(currectSwordTechniques)}");

            int skill = Fights.SwordSkills(currectSwordTechniques, out string detail);

            fight.Add($"Ваша Ловкость в этом бою: {skill} (по формуле: {detail})");
            fight.Add(String.Empty);

            int round = 1, heroRoundWin = 0, enemyRoundWin = 0;
            bool speedActivate = false, irresistibleAttack = false, rapidAttack = false;
            bool strikeBack = NoStrikeBack;

            while (true)
            {
                fight.Add($"HEAD|BOLD|Раунд: {round}");

                if (Fights.UseForcesInFight(ref fight, ref speedActivate, EnemiesList, SpeedActivate, WithoutTechnique))
                {
                    int enemyLimit = (EnemyHitpointsLimith > 0 ? EnemyHitpointsLimith : 0);

                    if ((FightEnemies.Keys.Where(x => x.Hitpoints > enemyLimit).Count() == 0))
                        return Win(fight);
                }

                if (Game.Option.IsTriggered("Скоростная атака") && (round > 3) && !rapidAttack)
                {
                    Character target = EnemiesList.Where(x => x.Hitpoints > 0).FirstOrDefault();

                    irresistibleAttack = Fights.AdditionalAttack(ref fight, target,
                        "Вы проводите Скоростную атаку!", "Урон от атаки");
                }                  

                int protagonistFirstDice = Game.Dice.Roll();
                int protagonistSecondDice = Game.Dice.Roll();
                int hitSkill = skill + Character.Protagonist.SwordTechniques[currectSwordTechniques] + 
                    protagonistFirstDice + protagonistSecondDice;

                fight.Add($"Ваша скорость удара: {skill} ловкость + " +
                    $"{Character.Protagonist.SwordTechniques[currectSwordTechniques]} " +
                    $"ранг + {Game.Dice.Symbol(protagonistFirstDice)} + " +
                    $"{Game.Dice.Symbol(protagonistSecondDice)} = {hitSkill}");

                foreach (Character enemy in EnemiesList)
                {
                    if (enemy.Hitpoints <= 0)
                    {
                        FightEnemies[enemy] = -1;
                    }
                    else
                    {
                        Game.Dice.DoubleRoll(out int enemyFirstDice, out int enemySecondDice);
                        FightEnemies[enemy] = enemy.Skill + enemy.Rang + enemyFirstDice + enemySecondDice;

                        fight.Add($"Скорость удара {enemy.Name}: " +
                            $"{enemy.Skill} ловкость + {enemy.Rang} ранг + " +
                            $"{Game.Dice.Symbol(enemyFirstDice)} + " +
                            $"{Game.Dice.Symbol(enemySecondDice)} = {FightEnemies[enemy]}");
                    }
                }

                bool protaganistMakeHit = false;

                foreach (KeyValuePair<Character, int> enemy in FightEnemies.OrderBy(x => x.Value))
                {
                    if (enemy.Value <= 0)
                    {
                        continue;
                    }
                    else if ((enemy.Value < hitSkill) && !protaganistMakeHit)
                    {
                        protaganistMakeHit = true;

                        enemy.Key.Hitpoints -= 3;
                        heroRoundWin += 1;

                        fight.Add($"GOOD|Вы ранили {enemy.Key.Name}, " +
                            $"он потерял 3 ед.выносливости " +
                            $"(осталось {enemy.Key.Hitpoints})");

                        if ((heroRoundWin >= 3) && !irresistibleAttack && Game.Option.IsTriggered("Неотразимая атака"))
                        {
                            irresistibleAttack = Fights.AdditionalAttack(ref fight, enemy.Key,
                                "Вы проводите Неотразимую атаку!", "Урон от атаки");
                        }
                    }

                    else if (enemy.Value > hitSkill)
                    {
                        Character.Protagonist.Hitpoints -= 3;
                        enemyRoundWin += 1;

                        fight.Add($"BAD|{enemy.Key.Name} ранил вас, " +
                            $"вы потеряли 3 ед.выносливости " +
                            $"(осталось {Character.Protagonist.Hitpoints})");

                        if ((enemyRoundWin >= 3) && !strikeBack && Game.Option.IsTriggered("Встречный удар"))
                        {
                            strikeBack = Fights.AdditionalAttack(ref fight, enemy.Key,
                                "Вы проводите Встречный удар!", "Урон от удара");
                        }
                    }
                    else
                    {
                        fight.Add("BOLD|Вы парировали удары друг друга");
                    }
                }

                if (speedActivate)
                {
                    Fights.SpeedFightHitpointsLoss(ref fight, Character.Protagonist);

                    foreach (Character enemy in EnemiesList.Where(x => x.Hitpoints > 0))
                        Fights.SpeedFightHitpointsLoss(ref fight, enemy);
                }

                fight.Add(String.Empty);

                bool enemyRound = (EnemyRoundWin > 0) && (enemyRoundWin >= EnemyRoundWin);
                int hitpointsLimit = (HeroHitpointsLimith > 0 ? HeroHitpointsLimith : 0);

                if ((Character.Protagonist.Hitpoints <= hitpointsLimit) || enemyRound)
                {
                    if (enemyRound)
                    {
                        fight.Add($"BIG|BAD|Вы проиграли {enemyRoundWin} раунда :(");
                    }
                    else
                    {
                        fight.Add("BIG|BAD|Вы ПРОИГРАЛИ :(");
                    }

                    return fight;
                }

                bool heroRound = (HeroRoundWin > 0) && (heroRoundWin >= HeroRoundWin);
                hitpointsLimit = (EnemyHitpointsLimith > 0 ? EnemyHitpointsLimith : 0);

                if ((FightEnemies.Keys.Where(x => x.Hitpoints > hitpointsLimit).Count() == 0) || heroRound)
                {
                    if (heroRound)
                    {
                        fight.Add($"BIG|GOOD|Вы выиграли {heroRoundWin} раундов :)");
                    }
                    else
                    {
                        fight.Add("BIG|GOOD|Вы ПОБЕДИЛИ :)");
                    }

                    return fight;
                }

                round += 1;
            }
        }
    }
}
