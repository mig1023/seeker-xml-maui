using System;

namespace SeekerMAUI.Gamebook.OctopusIsland
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public List<Character> Enemies { get; set; }
        public Character Enemy { get; set; }
        public int WoundsToWin { get; set; }
        public int DinnerHitpointsBonus { get; set; }
        public bool DinnerAlready { get; set; }
        public bool ReturnedStuffs { get; set; }
        public bool DeathMatch { get; set; }

        public override List<string> Representer()
        {
            List<string> enemy = new List<string>();

            if (Enemy == null)
                return enemy;

            enemy.Add($"{Enemy.Name}\nловкость {Enemy.Skill}  жизни {Enemy.Hitpoint}");

            return enemy;
        }

        public override List<string> Status() => new List<string>
        {
            $"Обедов: {Character.Protagonist.Food}",
            $"Животворная мазь: {Character.Protagonist.LifeGivingOintment}",
        };

        public override List<string> AdditionalStatus() => new List<string>
        {
            $"Серж: {Character.Protagonist.SergeSkill}/{Character.Protagonist.SergeHitpoint}",
            $"Ксолотл: {Character.Protagonist.XolotlSkill}/{Character.Protagonist.XolotlHitpoint}",
            $"Тибо: {Character.Protagonist.ThibautSkill}/{Character.Protagonist.ThibautHitpoint}",
            $"Суи: {Character.Protagonist.SouhiSkill}/{Character.Protagonist.SouhiHitpoint}",
        };

        public override List<string> StaticButtons()
        {
            List<string> staticButtons = new List<string> { };

            if (Character.Protagonist.LifeGivingOintment <= 0)
                return staticButtons;

            if (Character.Protagonist.SergeHitpoint < 20)
                staticButtons.Add("ВЫЛЕЧИТЬ СЕРЖА");

            if (Character.Protagonist.XolotlHitpoint < 20)
                staticButtons.Add("ВЫЛЕЧИТЬ КСОЛОТЛА");

            if (Character.Protagonist.ThibautHitpoint < 20)
                staticButtons.Add("ВЫЛЕЧИТЬ ТИБО");

            if (Character.Protagonist.SouhiHitpoint < 20)
                staticButtons.Add("ВЫЛЕЧИТЬ СУИ");

            return staticButtons;
        }

        public override bool StaticAction(string action)
        {
            if (action.Contains("СЕРЖА"))
            {
                Character.Protagonist.SergeHitpoint = Ointment.Cure(Character.Protagonist.SergeHitpoint);
            }
            else if (action.Contains("КСОЛОТЛА"))
            {
                Character.Protagonist.XolotlHitpoint = Ointment.Cure(Character.Protagonist.XolotlHitpoint);
            }
            else if (action.Contains("ТИБО"))
            {

                Character.Protagonist.ThibautHitpoint = Ointment.Cure(Character.Protagonist.ThibautHitpoint);
            }
            else if (action.Contains("СУИ"))
            {
                Character.Protagonist.SouhiHitpoint = Ointment.Cure(Character.Protagonist.SouhiHitpoint);
            }
            else
            {
                return false;
            }

            return true;
        }

        public override bool IsButtonEnabled(bool secondButton = false) =>
            !((DinnerHitpointsBonus > 0) && ((Character.Protagonist.Food <= 0) || DinnerAlready));

        public override bool AvailabilityNode(string option) =>
            AvailabilityTrigger(option);

        public override bool GameOver(out int toEndParagraph, out string toEndText) =>
            GameOverBy(Character.Protagonist.GameOver, out toEndParagraph, out toEndText);

        public List<string> Fight()
        {
            List<string> fight = new List<string>();

            int round = 1, enemyWounds = 0;

            Fights.SetCurrentWarrior(ref fight, start: true);

            while (true)
            {
                fight.Add($"HEAD|Раунд: {round}");

                if (Enemy.Hitpoint <= 0)
                    continue;

                Game.Dice.DoubleRoll(out int protagonistRollFirst, out int protagonistRollSecond);
                int protagonistHitStrength = protagonistRollFirst + protagonistRollSecond + Character.Protagonist.Skill;

                fight.Add($"{Character.Protagonist.Name}: мощность удара: " +
                    $"{Game.Dice.Symbol(protagonistRollFirst)} + " +
                    $"{Game.Dice.Symbol(protagonistRollSecond)} + " +
                    $"{Character.Protagonist.Skill} = {protagonistHitStrength}");

                Game.Dice.DoubleRoll(out int enemyRollFirst, out int enemyRollSecond);
                int enemyHitStrength = enemyRollFirst + enemyRollSecond + Enemy.Skill;

                fight.Add($"{Enemy.Name}: мощность удара: " +
                    $"{Game.Dice.Symbol(enemyRollFirst)} + " +
                    $"{Game.Dice.Symbol(enemyRollSecond)} + " +
                    $"{Enemy.Skill} = {enemyHitStrength}");

                if (protagonistHitStrength > enemyHitStrength)
                {
                    Enemy.Hitpoint -= 2;
                    enemyWounds += 1;

                    if ((Enemy.Hitpoint <= 0) || ((WoundsToWin > 0) && (WoundsToWin <= enemyWounds)))
                    {
                        fight.Add($"GOOD|BOLD|{Enemy.Name} ранен и повержен!");

                        fight.Add(String.Empty);
                        fight.Add("BIG|GOOD|Вы ПОБЕДИЛИ :)");

                        if (ReturnedStuffs)
                        {
                            fight.Add("GOOD|Вы вернули украденные у вас рюкзаки!");
                            Character.Protagonist.StolenStuffs = 0;
                        }

                        Fights.SaveCurrentWarriorHitPoints();

                        return fight;
                    }
                    else
                    {
                        var enemyHitpoints = Game.Services.CoinsNoun(Enemy.Hitpoint,
                            "жизнь", "жизни", "жизней");

                        fight.Add($"GOOD|BOLD|{Enemy.Name} ранен " +
                            $"(осталось {Enemy.Hitpoint} {enemyHitpoints})");
                    }
                }
                else if (protagonistHitStrength < enemyHitStrength)
                {
                    Character.Protagonist.Hitpoint -= 2;

                    var hitpoints = Game.Services.CoinsNoun(Character.Protagonist.Hitpoint, 
                        "жизнь", "жизни", "жизней");

                    fight.Add($"BAD|BOLD|{Enemy.Name} ранил {Character.Protagonist.Name} " +
                        $"(осталось {Character.Protagonist.Hitpoint} {hitpoints})");

                    if (!Fights.SetCurrentWarrior(ref fight))
                    {
                        if (DeathMatch)
                        {
                            fight.Add($"BAD|\nК сожалению, вам придётся " +
                                $"отказаться от миссии спасения Оллира... " +
                                $"Возможно, вам больше повезёт в следующий раз...");

                            Character.Protagonist.GameOver = true;
                        }
                            
                        return Fail(fight);
                    }
                }
                else
                {
                    fight.Add("BOLD|Ничья в раунде");
                }

                fight.Add(String.Empty);
            }

            round += 1;
        }

        public List<string> Dinner()
        {
            Character.Protagonist.Food -= 1;

            Character.Protagonist.SouhiHitpoint += DinnerHitpointsBonus;
            Character.Protagonist.SergeHitpoint += DinnerHitpointsBonus;
            Character.Protagonist.ThibautHitpoint += DinnerHitpointsBonus;
            Character.Protagonist.XolotlHitpoint += DinnerHitpointsBonus;

            DinnerAlready = true;

            return new List<string> { "RELOAD" };
        }
    }
}
