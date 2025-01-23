using Microsoft.Maui.Controls;
using System.Collections.Generic;

namespace SeekerMAUI.Gamebook.FIFA1966
{
    class Modification : Prototypes.Modification, Abstract.IModification
    {
        public string Path { get; set; }

        public override void Do()
        {
            if (!Character.Protagonist.Vars.ContainsKey(Path))
            {
                Character.Protagonist.Vars[Path] = 0;
            }

            if (Name == "Mod")
            {
                Character.Protagonist.Vars[Path] += Value;
            }
            else if (Name == "Set")
            {
                Character.Protagonist.Vars[Path] = Value;
            }
            else if (Name == "Disable")
            {
                if (Character.Protagonist.Vars[Path] == 1)
                {
                    Character.Protagonist.Vars[Path] = 0;
                    Character.Protagonist.Vars["силы соперников/СССР"] += Value;
                }
            }
            else if (Name == "Random")
            {
                Character.Protagonist.Vars[Path] = Game.Dice.Roll(size: Value);
            }
            else if (Name == "Enemy")
            {
                Character.Protagonist.Vars["силы соперников/сила соперника"] =
                    Character.Protagonist.Vars[$"силы соперников/{ValueString}"];
            }
            else if (Name == "BonusesDisable")
            {
                var bonuses = new List<string>
                {
                    "корейский",
                    "итальянский",
                    "чилийский",
                    "португальский",
                    "немецкий"
                };

                foreach (var bonus in bonuses)
                {
                    bool disabled = false;

                    if (Character.Protagonist.Vars[$"особенности игр/{bonus}"] == 1)
                    {
                        Character.Protagonist.Vars[$"особенности игр/{bonus}"] = 0;
                        disabled = true;
                    }

                    if (disabled)
                    {
                        Character.Protagonist.Vars["силы соперников/СССР"] -= 2;
                    }
                }
            }
            else if (Name == "WinByDice")
            {
                var bet = Character.Protagonist.Vars["выбор/орел-решка"];
                var result = Character.Protagonist.Vars["исход поединка/орел-решка"];

                if (bet == result)
                {
                    Character.Protagonist.Vars["плейофф/исход поединка"] = 1;
                    Character.Protagonist.Vars["ИГРА/СССР"] += 1;
                }
                else
                {
                    Character.Protagonist.Vars["плейофф/исход поединка"] = 2;
                    Character.Protagonist.Vars["расходники/вороги"] += 1;
                }
            }
            else if (Name == "CleanMatchResult")
            {
                Character.Protagonist.Enemy = string.Empty;
                Character.Protagonist.Vars["ИГРА/СССР"] = 0;
                Character.Protagonist.Vars["расходники/вороги"] = 0;
            }
            else if (Name == "FxChances")
            {
                var stronger = Character.Protagonist.Vars["расходники/кто сильнее"];
                var chance = Game.Dice.Roll(size: 7);
                var moment = 0;

                Character.Protagonist.Vars["1 из 7"] = chance;

                if ((chance < 5) && (stronger == 1))
                {
                    moment = 1;
                }
                else if ((chance == 5) && (stronger == 1))
                {
                    moment = 0;
                }
                else if ((chance > 5) && (stronger == 1))
                {
                    moment = 2;
                }
                else if ((chance < 4) && (stronger == 3))
                {
                    moment = 2;
                }
                else if ((chance > 3) && (chance < 6) && (stronger == 3))
                {
                    moment = 0;
                }
                else if ((chance > 5) && (stronger == 3))
                {
                    moment = 1;
                }
                else if ((chance < 4) && (stronger == 2))
                {
                    moment = 1;
                }
                else if ((chance > 3) && (chance < 6) && (stronger == 2))
                {
                    moment = 0;
                }
                else
                {
                    moment = 2;
                }

                Character.Protagonist.Vars["расходники/кому момент"] = moment;
            }
            else if (Name == "FxComparisonOfTeam")
            {
                var ussr = Character.Protagonist.Vars["силы соперников/СССР"];
                var enemy = Character.Protagonist.Vars["силы соперников/сила соперника"];

                if (ussr > enemy)
                {
                    Character.Protagonist.Vars["расходники/кто сильнее"] = 1;
                }
                else if (ussr < enemy)
                {
                    Character.Protagonist.Vars["расходники/кто сильнее"] = 3;
                }
                else
                {
                    Character.Protagonist.Vars["расходники/кто сильнее"] = 2;
                }
            }
            else if (Name == "FxEnemy")
            {
                var enemies = new Dictionary<int, string>
                {
                    [1] = "Уругвай",
                    [2] = "Бразилия",
                    [3] = "Бельгия",
                    [4] = "Франция",
                    [5] = "КНДР",
                    [6] = "Италия",
                    [7] = "Чили",
                    [8] = "Венгрия",
                    [9] = "Германия",
                    [11] = "Португалия",
                    [81] = "Португалия",
                    [82] = "Англия",
                    [83] = "Венгрия",
                    [100] = "Англия",
                    [101] = "Германия",
                };

                var game = Character.Protagonist.Vars["расходники/номер игры"];

                Character.Protagonist.Enemy = enemies[game];

                if (!Character.Protagonist.Vars.ContainsKey("расходники/вороги"))
                    return;

                Character.Protagonist.Vars[$"ИГРА/{enemies[game]}"] =
                    Character.Protagonist.Vars["расходники/вороги"];
            }
            else if (Name == "FxRanking")
            {
                var group = Character.Protagonist.Vars
                    .ToDictionary()
                    .Where(x => x.Key.StartsWith("групповой этап/"))
                    .OrderByDescending(x => x.Value);

                int place = 0;

                foreach (var team in group)
                {
                    place += 1;
                    var name = team.Key.Replace("групповой этап/", "места в группе/");
                    Character.Protagonist.Vars[name] = place;
                }
            }
            else if (Name == "FxWinner")
            {
                var ussr = Character.Protagonist.Vars["ИГРА/СССР"];
                var enemy = Character.Protagonist.Vars["расходники/вороги"];

                if (ussr > enemy)
                {
                    Character.Protagonist.Vars["исход поединка"] = 1;
                }
                else if (ussr < enemy)
                {
                    Character.Protagonist.Vars["исход поединка"] = 2;
                }
                else
                {
                    Character.Protagonist.Vars["исход поединка"] = 3;
                }

                Character.Protagonist.Enemy = String.Empty;
            }
            else if (Name == "FxTactics")
            {
                var chance = Game.Dice.Roll(size: 3);
                var attack = Character.Protagonist.Vars["тактическое построение/атака"] == 1;
                var mod = Character.Protagonist.Vars["тактическое построение/модификатор"];

                if ((chance == 1) && attack && (mod != 1))
                {
                    Character.Protagonist.Vars["силы соперников/СССР"] += 1;
                    Character.Protagonist.Vars["тактическое построение/модификатор"] = 1;
                    Character.Protagonist.Vars["тактическое построение/время"] = 3;
                }

                chance = Game.Dice.Roll(size: 3);
                var defence = Character.Protagonist.Vars["тактическое построение/защита"] == 1;

                if ((chance == 1) && defence && (mod != 2))
                {
                    Character.Protagonist.Vars["силы соперников/сила соперника"] -= 1;
                    Character.Protagonist.Vars["тактическое построение/модификатор"] = 2;
                    Character.Protagonist.Vars["тактическое построение/время"] = 3;
                }
            }
            else if (Name == "FxProblems")
            {
                AddProblem("модификаторы/алкашня", "последствия решений/попойка", -1);
                AddProblem("модификаторы/угождание спорткомитету", "последствия решений/ослабление угожданием", -1);
                AddProblem("модификаторы/терки со спорткомитетом", "последствия решений/комитет зуб", -1);
                AddProblem("расходники/критика", String.Empty, -1);

                var chance = Game.Dice.Roll(size: 3);
                var master = Character.Protagonist.Vars["модификаторы/мастер"] == 1;
                var hardWorker = Character.Protagonist.Vars["модификаторы/трудяга"] == 1;

                if (((chance == 1) && master) || ((chance == 2) && hardWorker))
                {
                    Character.Protagonist.Vars["последствия решений/выстрелил"] = 1;
                    Character.Protagonist.Vars["силы соперников/СССР"] += 1;
                }

                chance = Game.Dice.Roll(size: 3);

                if (chance == 1)
                {
                    Character.Protagonist.Vars["силы соперников/сила соперника"] += 2;
                    Character.Protagonist.Vars["модификаторы/соперник в ударе"] = 1;
                }

                chance = Game.Dice.Roll(size: 3);

                if (chance == 2)
                {
                    Character.Protagonist.Vars["модификаторы/травма"] = 1;
                }
            }
            else if (Name == "FxNoProblems")
            {
                SolveProblem("последствия решений/попойка", 1);
                SolveProblem("последствия решений/выстрелил", -1);
                SolveProblem("последствия решений/ослабление угожданием", 1);
                SolveProblem("последствия решений/комитет зуб", 1);
                SolveProblem("модификаторы/соперник в ударе", 0);
            }
        }

        private void AddProblem(string name, string marker, int bonus)
        {
            var chance = Game.Dice.Roll(size: 3);
            var problem = Character.Protagonist.Vars[name] == 1;

            if ((chance == 1) && problem)
            {
                if (!String.IsNullOrEmpty(marker))
                {
                    Character.Protagonist.Vars[marker] = 1;
                }
                    
                Character.Protagonist.Vars["силы соперников/СССР"] += bonus;
            }
        }

        private void SolveProblem(string name, int bonus)
        {
            if (Character.Protagonist.Vars[name] == 1)
            {
                Character.Protagonist.Vars[name] = 0;
                Character.Protagonist.Vars["силы соперников/СССР"] += bonus;
            }
        }
    }
}
