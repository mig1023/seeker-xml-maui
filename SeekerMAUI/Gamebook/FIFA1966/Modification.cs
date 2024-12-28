using System;

namespace SeekerMAUI.Gamebook.FIFA1966
{
    class Modification : Prototypes.Modification, Abstract.IModification
    {
        public string Path { get; set; }

        public override void Do()
        {
            if (!Character.Protagonist.Vars.ContainsKey(Path))
            {
                Character.Protagonist.Vars.Add(Path, 0);
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
                else  if ((chance < 4) && (stronger == 3))
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
                else  if ((chance < 4) && (stronger == 2))
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
        }
    }
}
