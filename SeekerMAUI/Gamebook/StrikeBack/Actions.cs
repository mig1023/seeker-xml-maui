﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace SeekerMAUI.Gamebook.StrikeBack
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public List<Character> Allies { get; set; }
        public List<Character> Enemies { get; set; }
        public List<Character.SpecialTechniques> SpecialTechniques { get; set; }

        public bool GroupFight { get; set; }
        public int RoundsToWin { get; set; }
        public int WoundsToWin { get; set; }
        public int Count { get; set; }
        public bool WoundsByDices { get; set; }
        public int WoundsMultiple { get; set; }

        public override List<string> Status() => new List<string>
        {
            $"Атака: {Character.Protagonist.Attack}",
            $"Защита: {Character.Protagonist.Defence}",
            $"Выносливость: {Character.Protagonist.Endurance}/{Character.Protagonist.MaxEndurance}",
        };

        public override List<string> Representer()
        {
            List<string> enemies = new List<string>();

            if ((Type != "Fight") && String.IsNullOrEmpty(Head))
            {
                return enemies;
            }
            else if (Type != "Fight")
            {
                return new List<string> { Head };
            }

            if ((Allies != null) && GroupFight)
            {
                if (!SpecialTechniques.Contains(Character.SpecialTechniques.WithoutProtagonist))
                {
                    enemies.Add($"Вы\n" +
                        $"нападение {Character.Protagonist.Attack}  " +
                        $"защита {Character.Protagonist.Defence}  " +
                        $"жизнь {Character.Protagonist.Endurance}" +
                        $"{Character.Protagonist.GetSpecialTechniques()}");
                }

                foreach (Character ally in Allies)
                {
                    enemies.Add($"{ally.Name}\n" +
                        $"нападение {ally.Attack}  " +
                        $"защита {ally.Defence}  " +
                        $"жизнь {ally.GetEndurance()}" +
                        $"{ally.GetSpecialTechniques()}");
                }

                enemies.Add("SPLITTER|против");
            }

            foreach (Character enemy in Enemies)
            {
                enemies.Add($"{enemy.Name}\n" +
                    $"нападение {enemy.Attack}  " +
                    $"защита {enemy.Defence}  " +
                    $"жизнь {enemy.GetEndurance()}" +
                    $"{enemy.GetSpecialTechniques()}");
            }

            return enemies;
        }

        public override bool GameOver(out int toEndParagraph, out string toEndText)
        {
            if (SpecialTechniques?.Contains(Character.SpecialTechniques.WithoutGameover) ?? false)
            {
                return base.GameOver(out toEndParagraph, out toEndText);
            }
            else
            {
                return GameOverBy(Character.Protagonist.Endurance, out toEndParagraph, out toEndText);
            }
        }

        public List<string> InlineWoundsByDices()
        {
            WoundsByDices = true;
            return Dices();
        }

        public List<string> Dices() =>
            Dice.Roll(Count, WoundsByDices, WoundsMultiple);

        public List<string> FindTheWay() =>
            Dice.FindTheWay();

        public override bool Availability(string option)
        {
            if (String.IsNullOrEmpty(option))
            {
                return true;
            }
            else if (option.Contains('|'))
            {
                foreach (string optionsPart in option.Split('|'))
                {
                    if (Character.Protagonist.Creature == optionsPart)
                        return true;
                }

                return false;
            }
            else if (option.Contains(','))
            {
                List<string> singlOption = option
                    .Split(',')
                    .Select(x => x.Trim())
                    .Select(y => y.Replace("!", String.Empty))
                    .ToList();

                foreach (string optionsPart in singlOption)
                {
                    if (Character.Protagonist.Creature == optionsPart)
                        return false;
                }

                return true;
            }
            else if (Character.Protagonist.Creature == option)
            {
                return true;
            }
            else if (Character.Protagonist.Creature == option.Replace("!", String.Empty))
            {
                return false;
            }
            else
            {
                return AvailabilityTrigger(option.Trim());
            }
        }

        private static Character FindEnemyIn(List<Character> Enemies) =>
            Enemies.Where(x => x.Endurance > 0).OrderBy(y => Game.Dice.Roll(size: 100)).FirstOrDefault();

        private static Character FindEnemy(Character fighter, List<Character> Allies, List<Character> Enemies)
        {
            if (Allies.Contains(fighter))
            {
                return FindEnemyIn(Enemies);
            }
            else if (Enemies.Contains(fighter))
            {
                return FindEnemyIn(Allies);
            }
            else
            {
                return null;
            }
        }

        private static bool IsProtagonist(string name) =>
            name == Character.Protagonist.Name;

        public List<string> HobgoblinGame()
        {
            List<string> game = new List<string> { "BIG|Играем:" };

            for (int round = 1; round < 5; round++)
            {
                if (Character.Protagonist.Endurance <= 0)
                    continue;

                game.Add(String.Empty);

                Game.Dice.DoubleRoll(out int firstRoll, out int secondRoll);
                int hitStrength = firstRoll + secondRoll + 1;

                game.Add($"Cила атаки кинжала: " +
                    $"{Game.Dice.Symbol(firstRoll)} + " +
                    $"{Game.Dice.Symbol(secondRoll)} + 1 = {hitStrength}");

                int hitDiff = hitStrength - Character.Protagonist.Defence;
                bool success = hitDiff > 0;

                game.Add($"Твоя защита: " +
                    $"{Character.Protagonist.Defence}, это " +
                    $"{Game.Services.Сomparison(Character.Protagonist.Defence, hitStrength)} силы атаки");

                if (success)
                {
                    Character.Protagonist.Endurance -= hitDiff;

                    game.Add("BAD|BOLD|Ты ранен");
                    game.Add($"Ты потерял вносливости: {hitDiff} (осталось: {Character.Protagonist.Endurance})");
                }
                else
                {
                    game.Add("GOOD|BOLD|Ты увернулся");
                }
            }

            game.Add(String.Empty);
            game.Add(Character.Protagonist.Endurance > 0 ? "BIG|GOOD|ТЫ ПОБЕДИЛ :)" : "BIG|BAD|ТЫ ПРОИГРАЛ :(");

            return game;
        }

        public List<string> Fight()
        {
            List<string> fight = new List<string>();

            int round = 1, enemyWounds = 0;

            List<Character> FightAllies = new List<Character>();
            List<Character> FightEnemies = new List<Character>();
            List<Character> FightOrder = new List<Character>();
            Dictionary<string, int> WoundsCount = new Dictionary<string, int>();
            Dictionary<string, List<int>> AttackStory = new Dictionary<string, List<int>>();

            bool withoutProtagonist = SpecialTechniques.Contains(Character.SpecialTechniques.WithoutProtagonist);
            bool toFirstDeath = SpecialTechniques.Contains(Character.SpecialTechniques.ToFirstDeathOnly);
            bool werewolf = SpecialTechniques.Contains(Character.SpecialTechniques.Werewolf) &&
                (Character.Protagonist.Creature == "ОБОРОТЕНЬ");

            if (!withoutProtagonist)
                FightAllies.Add(Character.Protagonist);

            foreach (Character enemy in Enemies)
                FightEnemies.Add(enemy.Clone().SetEndurance());

            if (Allies != null)
            {
                foreach (Character ally in Allies)
                    FightAllies.Add(ally.Clone().SetEndurance());
            }
                
            if ((FightEnemies.Count > 1) && (FightAllies.Count == 1) && !withoutProtagonist)
            {
                fight.Add("Противников много, а ты один, поэтому атакуют они первые :(");
                fight.Add(String.Empty);

                FightOrder.AddRange(FightEnemies);
                FightOrder.AddRange(FightAllies);
            }
            else
            {
                FightOrder.AddRange(FightAllies);
                FightOrder.AddRange(FightEnemies);
            }

            while (true)
            {
                fight.Add($"HEAD|BOLD|*  *  *    РАУНД: {round}    *  *  * ");
                fight.Add(String.Empty);

                foreach (Character fighter in FightOrder)
                {
                    if (fighter.Endurance <= 0)
                        continue;

                    Character enemy = FindEnemy(fighter, FightAllies, FightEnemies);

                    if (enemy == null)
                        continue;

                    if (GroupFight)
                    {
                        fight.Add($"BOLD|{fighter.Name} выбрает противника для атаки: {enemy.Name}");
                    }
                    else
                    {
                        fight.Add($"BOLD|{fighter.Name} атакует");
                    }

                    Game.Dice.DoubleRoll(out int firstRoll, out int secondRoll);
                    int hitStrength = firstRoll + secondRoll + fighter.Attack;

                    fight.Add($"Сила атаки: " +
                        $"{Game.Dice.Symbol(firstRoll)} + " +
                        $"{Game.Dice.Symbol(secondRoll)} + " +
                        $"{fighter.Attack} (атака) = {hitStrength}");

                    int hitDiff = hitStrength - enemy.Defence;
                    bool success = hitDiff > 0;
                    string name = IsProtagonist(enemy.Name) ? "Ваша защита" : "Защита противника";

                    fight.Add($"{name}: {enemy.Defence}, это " +
                        $"{Game.Services.Сomparison(enemy.Defence, hitStrength)} силы атаки");

                    if (IsProtagonist(enemy.Name))
                    {
                        bool rumbleKnife = fighter.SpecialTechnique
                            .Contains(Character.SpecialTechniques.RumbleKnife);

                        if (werewolf && success && !rumbleKnife)
                        {
                            fight.Add("GOOD|Атака противника не может навредить оборотню!");
                            success = false;
                        }
                        else
                        {
                            fight.Add(success ? "BAD|BOLD|Ты ранен" : "GOOD|Атака отбита");
                        }
                    }
                    else  if (FightAllies.Contains(enemy))
                    {
                        fight.Add(success ? $"BAD|{enemy.Name} ранен" : "GOOD|Атака отбита");
                    }
                    else
                    {
                        fight.Add(success ? $"GOOD|BOLD|{enemy.Name} ранен" : "BAD|Атака отбита");
                    }

                    if (success)
                    {
                        fight.Add($"Тяжесть раны: " +
                            $"{hitStrength} (сила атаки) - " +
                            $"{enemy.Defence} (защита) = {hitDiff}");

                        enemy.Endurance -= hitDiff;

                        fight.Add($"{enemy.Name} потерял вносливости: " +
                            $"{hitDiff} (осталось: {enemy.Endurance})");

                        if (toFirstDeath && (enemy.Endurance <= 0) && !IsProtagonist(enemy.Name))
                        {
                            fight.Add("BIG|BOLD|В бою случилась первая смерть!");
                            return fight;
                        }
                    }

                    fight.Add(String.Empty);
                }

                bool enemyLost = FightEnemies
                    .Where(x => (x.Endurance > 0) || (x.MaxEndurance == 0)).Count() == 0;

                if (enemyLost || ((WoundsToWin > 0) && (WoundsToWin <= enemyWounds)))
                {
                    fight.Add("BIG|GOOD|ТЫ ПОБЕДИЛ :)");
                    return fight;
                }

                int allyLost = FightAllies
                    .Where(x => x.Endurance > 0)
                    .Count();

                if (allyLost == 0)
                {
                    fight.Add("BIG|BAD|ТЫ ПРОИГРАЛ :(");
                    return fight;
                }

                if ((RoundsToWin > 0) && (RoundsToWin <= round))
                {
                    fight.Add("BAD|Отведённые на победу раунды истекли.");
                    fight.Add("BIG|BAD|ТЫ ПРОИГРАЛ :(");
                    return fight;
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
