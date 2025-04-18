﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace SeekerMAUI.Gamebook.Tank
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public string Crew { get; set; }
        public bool FrontHit { get; set; } 

        public override List<string> Status()
        {
            string status = "исправен";

            bool optics = Game.Option.IsTriggered("оптика");
            bool gun = Game.Option.IsTriggered("орудие");
            bool machineGun1 = Game.Option.IsTriggered("пулемёт 1");
            bool machineGun2 = Game.Option.IsTriggered("пулемёт 2");
            bool immobil = Character.Protagonist.Immobilized > 0;

            if (optics || gun || machineGun1 || machineGun2 || immobil)
                status = "повреждён";

            if (Character.Protagonist.Dead > 0)
                status = "подбит";

            List<string> statuses = new List<string>
            {
                $"Победных очков: {Character.Protagonist.VictoryPoints}",
                $"Состояние танка: {status}"
            };

            return statuses;
        }

        private void CrewNames(string name, out string nominative, out string accusative)
        {
            string crewName = Constants.CrewNames[name];
            List<string> crew = crewName.ToUpper().Split('|').ToList();
            accusative = crew[0];
            nominative = crew[1];
        }

        public override List<string> Representer()
        {         
            string line = String.Empty;

            if (Type == "Test")
            {
                CrewNames(Crew, out string _, out string accusative);
                line = $"ТЕСТ {accusative}";
            }
            else if (Type == "Luck")
            {
                return new List<string>();
            }
            else if (Type == "HitTable")
            {
                line = "Бросок по Таблице попаданий";
            }
            else
            {
                CrewNames(Crew, out string nominative, out string _);
                int currentStat = GetProperty(Character.Protagonist, Crew);
                string points = Game.Services.CoinsNoun(currentStat, "очко", "очка", "очков");
                line = $"{nominative}" + (currentStat > 0 ? $"\nОпыт {currentStat} {points}" : String.Empty);
            }

            return new List<string> { line };
        }

        public override bool IsButtonEnabled(bool secondButton = false)
        {
            if (String.IsNullOrEmpty(Crew) || (Type != "Get-Decrease"))
                return true;

            int stat = GetProperty(Character.Protagonist, Crew);

            if (secondButton)
                return (stat > 0);
            else
                return (Character.Protagonist.StatBonuses > 0);
        }

        public override bool Availability(string option)
        {
            if (String.IsNullOrEmpty(option))
            {
                return true;
            }
            else
            {
                if (option.Contains("ПОБЕДНЫХ ОЧКОВ МАЛО"))
                {
                    return Character.Protagonist.VictoryPoints < 4;
                }
                else if (option.Contains("ПОБЕДНЫХ ОЧКОВ СРЕДНЕ"))
                {
                    bool less = Character.Protagonist.VictoryPoints < 7;
                    bool more = Character.Protagonist.VictoryPoints >= 4;
                    return less && more;
                }
                else if (option.Contains("ПОБЕДНЫХ ОЧКОВ МНОГО"))
                {
                    return Character.Protagonist.VictoryPoints > 7;
                }
                else
                {
                    return AvailabilityTrigger(option);
                }
            }
        }

        public List<string> Get() =>
            ChangeProtagonistParam(Crew, Character.Protagonist, "StatBonuses");

        public List<string> Decrease() =>
            ChangeProtagonistParam(Crew, Character.Protagonist, "StatBonuses", decrease: true);

        public List<string> Test()
        {
            List<string> testLines = new List<string>();
            CrewNames(Crew, out string nominative, out string accusative);

            int experience = GetProperty(Character.Protagonist, Crew);
            string points = Game.Services.CoinsNoun(experience, "очко", "очка", "очков");
            testLines.Add($"Опыт {accusative.ToLower()}: {experience} {points}");

            int dice = Game.Dice.Roll();
            bool testIsOk = dice <= experience;
            string diffLine = testIsOk ? "<=" : ">";
            testLines.Add($"Тест: {Game.Dice.Symbol(dice)} {diffLine} {experience} опыта");

            testLines.Add(Result(testIsOk, $"BIG|BOLD|{nominative} СПРАВИЛСЯ", $"BIG|BOLD|{nominative} НЕ СПРАВИЛСЯ"));

            Game.Buttons.Disable(testIsOk, "В случае успеха", "В случае провала");

            return testLines;
        }

        private void LuckCheck(out bool goodLuck, ref List<string> luckLines)
        {
            int luckDice = Game.Dice.Roll();
            goodLuck = luckDice % 2 == 0;
            string odd = goodLuck ? "чётное" : "нечётное";

            luckLines.Add($"Проверка удачи: {Game.Dice.Symbol(luckDice)} - {odd}");
        }

        public List<string> Luck()
        {
            List<string> luckCheck = new List<string>();
            LuckCheck(out bool goodLuck, ref luckCheck);

            luckCheck.Add(Result(goodLuck, "BIG|BOLD|УСПЕХ", "BIG|BOLD|НЕУДАЧА"));

            Game.Buttons.Disable(goodLuck, "Повезло", "Не повезло");

            return luckCheck;
        }

        private void DisableButtonsExcept(string button)
        {
            List<string> allButtons = new List<string>(Constants.FightStatuses);

            allButtons.Remove(button);

            Game.Buttons.Disable(String.Join(",", allButtons));
        }

        public List<string> HitTable()
        {
            List<string> hitLines = new List<string>();

            Game.Dice.DoubleRoll(out int firstDice, out int secondDixe);
            int hit = firstDice + secondDixe;

            hitLines.Add($"Бросок по таблице: " +
                $"{Game.Dice.Symbol(firstDice)} + {Game.Dice.Symbol(secondDixe)} = {hit}");
            
            string prefix = hit == 7 ? "GOOD|" : "BAD|";
            string hitLine = Constants.HitNames[hit];

            if (FrontHit && Constants.FrontMisses.Contains(hit))
            {
                prefix = "GOOD|";
                hitLine = "Мимо!";
            }

            hitLines.Add($"BIG|BOLD|{prefix}{hitLine}");

            switch (hit)
            {
                case 2:
                    Game.Option.Trigger("оптика");
                    return hitLines;

                case 3:
                    Game.Option.Trigger("пулемет 1");
                    return hitLines;

                case 4:
                    Character.Protagonist.Dead = 2;
                    DisableButtonsExcept("Танк подбит и вы еще живы");
                    return hitLines;

                case 5:
                case 6:
                    Character.Protagonist.Immobilized = 1;
                    DisableButtonsExcept("Танк обездвижен");
                    return hitLines;

                case 7:
                    DisableButtonsExcept("Танк еще цел");
                    return hitLines;

                case 8:
                case 9:
                case 11:
                    Character.Protagonist.Dead = 1;
                    DisableButtonsExcept("Все погибли");
                    return hitLines;

                case 10:
                    Game.Option.Trigger("орудие");
                    Game.Option.Trigger("пулемет 2");
                    DisableButtonsExcept("Танк еще цел");
                    return hitLines;

                case 12:
                    LuckCheck(out bool goodLuck, ref hitLines);

                    if (goodLuck)
                    {
                        hitLines.Add("GOOD|BOLD|Все спаслись!");
                        Character.Protagonist.Dead = 2;
                        DisableButtonsExcept("Танк подбит и вы еще живы");
                    }
                    else
                    {
                        hitLines.Add("BAD|BOLD|Все сгорели...");
                        Character.Protagonist.Dead = 1;
                        DisableButtonsExcept("Все погибли");
                    }
                    
                    return hitLines;

                default:
                    return hitLines;
            }
        }
    }
}
