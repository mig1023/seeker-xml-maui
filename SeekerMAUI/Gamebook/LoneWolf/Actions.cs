using SeekerMAUI.Game;
using System;
using System.Text.RegularExpressions;

namespace SeekerMAUI.Gamebook.LoneWolf
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public bool Disciplines { get; set; }
        public bool ImmuneToPsychology { get; set; }
        public bool Undead { get; set; }
        public string SkillBonus { get; set; }
        public string SkillPenalty { get; set; }

        public Character Enemy { get; set; }

        public override List<string> Status() => new List<string>
        {
            $"Боевой навык: {Character.Protagonist.Skill}",
            $"Выносливость: {Character.Protagonist.Strength}/{Character.Protagonist.MaxStrength}",
            $"Монеты: {Character.Protagonist.Gold}",
        };

        public override List<string> Representer()
        {
            if (Disciplines)
            {
                return new List<string> { Head };
            }

            if (Enemy != null)
            {
                var additional = string.Empty;

                if (ImmuneToPsychology)
                    additional += "\nиммунитет к Удару Разума";

                if (Undead)
                    additional += (string.IsNullOrEmpty(additional) ? "\n" : "   ") + "нежить";

                return new List<string>
                {
                    $"{Enemy.Name}\nбоевой навык {Enemy.Skill}   выносливость {Enemy.Strength}{additional}"
                };
            }

            return new List<string>();
        }

        public override bool GameOver(out int toEndParagraph, out string toEndText) =>
            GameOverBy(Character.Protagonist.Strength, out toEndParagraph, out toEndText);

        public override bool IsButtonEnabled(bool secondButton = false)
        {
            if (Disciplines && (Game.Option.IsTriggered(Head) || Character.Protagonist.Disciplines <= 0))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public override bool Availability(string option) =>
            AvailabilityTrigger(option);

        public List<string> Get()
        {
            if (Disciplines)
            {
                Game.Option.Trigger(Head);
                Character.Protagonist.Disciplines -= 1;
            }

            return new List<string> { "RELOAD" };
        }

        public List<string> Random()
        {
            var dice = Game.Dice.Roll(size: 10) - 1;

            foreach (var option in Game.Option.GetTexts())
            {
                if (!option.Contains("—"))
                    continue;

                var regex = new Regex(@"(\d+)\s*—\s*(\d+)");
                var matches = regex.Match(option);
                var parts = matches.Value.Split('—');

                if ((dice < int.Parse(parts[0])) || (dice > int.Parse(parts[1])))
                {
                    Game.Buttons.Disable(option);
                }
            }

            return new List<string> { "BOLD|Таблица случайных чисел", $"BIG|Случайное число: {dice}" };
        }

        private void CoefficientBonus(string reason, int bonus,
            ref int coefficient, ref string coefficientLine)
        {
            coefficient += bonus;
            coefficientLine += $"\n+ {bonus} за {reason}";
        }

        private void SkillMod(string line, ref int coefficient, ref string coefficientLine, bool penalty = false)
        {
            var bonus = line.Split(';');

            if (!string.IsNullOrEmpty(bonus[1].Trim()) && !Game.Option.IsTriggered(bonus[1].Trim()))
                return;

            coefficient += int.Parse(bonus[0]) * (penalty ? -1 : 1);

            var negative = penalty ? "-" : "+";
            var reason = string.Empty;

            if (!string.IsNullOrEmpty(bonus[2]))
            {
                reason = $" за {bonus[2].Trim()}";
            }
            else if (string.IsNullOrEmpty(bonus[1]))
            {
                reason = $" за отсутствие Дисциплины {bonus[1].Trim()}";
            }

            coefficientLine += $"\n{negative} {bonus[0].Trim()} {reason}";
        }

        public List<string> Fight()
        {
            List<string> fight = new List<string>();

            var round = 1;
            var coefficient = Character.Protagonist.Skill;
            var coefficientLine = $"Считаем Боевой коэффициент:\n+ {coefficient} Боевой навык";

            if (Game.Option.IsTriggered("Владение оружием"))
            {
                CoefficientBonus("Дисциплину Владение оружием", bonus: 2,
                    ref coefficient, ref coefficientLine);
            }

            if (Game.Option.IsTriggered("Удар разума") && !ImmuneToPsychology)
            {
                CoefficientBonus("Дисциплину Удар разума", bonus: 2,
                    ref coefficient, ref coefficientLine);
            }

            if (Game.Option.IsTriggered("Соммерсверд"))
            {
                CoefficientBonus("Соммерсверд", bonus: 8,
                    ref coefficient, ref coefficientLine);
            }

            if (!String.IsNullOrEmpty(SkillBonus))
            {
                SkillMod(SkillBonus, ref coefficient, ref coefficientLine);
            }

            coefficient -= Enemy.Skill;
            coefficientLine += $"\n- {Enemy.Skill} Боевой навык врага";

            if (!String.IsNullOrEmpty(SkillPenalty))
            {
                SkillMod(SkillPenalty, ref coefficient, ref coefficientLine, penalty: true);
            }

            coefficientLine += $"\nИТОГО: {coefficient}";

            fight.Add($"GRAY|{coefficientLine}");

            var table = BattleTable.Init(coefficient);
            var tableLine = $"Таблица результатов битв:";

            foreach (var line in table)
            {
                tableLine += $"\n{line.Key} \t ---> \t [ {line.Value} ]";
            }

            fight.Add($"GRAY|{tableLine}\n");

            while (true)
            {
                fight.Add($"HEAD|BOLD|РАУНД: {round}");

                var dice = Game.Dice.Roll(size: 10) - 1;
                fight.Add($"Случайное число: {dice}");

                BattleTable.Get(dice, out int heroDamage, out int enemyDamage);
                fight.Add($"GRAY|Табличные значения: {enemyDamage}/{heroDamage}");

                if (heroDamage < 0)
                {
                    fight.Add($"BAD|BOLD|Вы убиты наповал...");
                    return Fail(fight);
                }
                else
                {
                    var color = heroDamage > 0 ? "BAD" : "GOOD";
                    fight.Add($"{color}|BOLD|Вы теряете: {heroDamage}");

                    Character.Protagonist.Strength -= heroDamage;

                    if (Character.Protagonist.Strength <= 0)
                    {
                        fight.Add("Ваша выносливость исчерпана...");
                        return Fail(fight);
                    }

                    fight.Add($"Ваша выносливость: {Character.Protagonist.Strength}");
                }

                if (enemyDamage < 0)
                {
                    fight.Add($"GOOD|BOLD|Вы прикончили противника одним ударом!");
                    return Win(fight);
                }
                else
                {
                    var damaged = enemyDamage > 0;
                    var color = damaged ? "GOOD|" : String.Empty;
                    fight.Add($"{color}BOLD|Противник теряет: {enemyDamage}");

                    if (Game.Option.IsTriggered("Соммерсверд") && Undead && damaged)
                    {
                        enemyDamage *= 2;

                        fight.Add("BOLD|Соммерсверд удваивает нанесённый нежети урон!");
                        fight.Add($"BOLD|В итоге противник теряет: {enemyDamage}");
                    }

                    Enemy.Strength -= enemyDamage;

                    if (Enemy.Strength <= 0)
                    {
                        fight.Add("Выносливость противника исчерпана!");
                        return Win(fight);
                    }
                       

                    fight.Add($"Выносливость противника: {Enemy.Strength}");
                }
                
                fight.Add(String.Empty);

                round += 1;
            }
        }
    }
}
