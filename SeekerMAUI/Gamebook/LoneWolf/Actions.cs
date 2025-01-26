using SeekerMAUI.Game;
using System;
using System.Text.RegularExpressions;

namespace SeekerMAUI.Gamebook.LoneWolf
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public bool Disciplines { get; set; }
        public bool ImunneToPsychology { get; set; }

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
                return new List<string>
                {
                    $"{Enemy.Name}\nбоевой навык {Enemy.Skill}  выносливость {Enemy.Strength}"
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

        private void CoefficientBonus(string reason, ref int coefficient, ref string coefficientLine)
        {
            coefficient += 2;
            coefficientLine += $"\n+ 2 за Дисциплину {reason}";
        }

        public List<string> Fight()
        {
            List<string> fight = new List<string>();

            var round = 1;
            var coefficient = Character.Protagonist.Skill;
            var coefficientLine = $"Считаем Боевой коэффициент:\n+ {coefficient} Боевой навык";

            if (Game.Option.IsTriggered("Владение оружием"))
            {
                CoefficientBonus("Владение оружием", ref coefficient, ref coefficientLine);
            }

            if (Game.Option.IsTriggered("Удар разума") && !ImunneToPsychology)
            {
                CoefficientBonus("Удар разума", ref coefficient, ref coefficientLine);
            }

            coefficient -= Enemy.Skill;
            coefficientLine += $"\n- {Enemy.Skill} Боевой навык врага\nИТОГО: {coefficient}";

            fight.Add($"GRAY|{coefficientLine}");

            BattleTable.Init(coefficient);

            while (true)
            {
                fight.Add($"HEAD|BOLD|Раунд: {round}");

                var dice = Game.Dice.Roll(size: 10) - 1;
                fight.Add($"Случайное число: {dice}");

                BattleTable.Get(dice, out int heroDamage, out int enemyDamage);
             
                if (heroDamage < 0)
                {
                    fight.Add($"BAD|BOLD|Вы убиты наповал...");
                    return Fail(fight);
                }
                else
                {
                    fight.Add($"BAD|BOLD|Вы теряете: {heroDamage}");
                    Character.Protagonist.Strength -= heroDamage;

                    if (Character.Protagonist.Strength <= 0)
                        return Fail(fight);

                    fight.Add($"Ваша выносливость: {Character.Protagonist.Strength}");
                }

                if (enemyDamage < 0)
                {
                    fight.Add($"GOOD|BOLD|Вы прикончили противника одним ударом!");
                    return Win(fight);
                }
                else
                {
                    fight.Add($"GOOD|BOLD|Противник теряет: {enemyDamage}");
                    Enemy.Strength -= enemyDamage;

                    if (Enemy.Strength <= 0)
                        return Win(fight);
                }
                
                
                fight.Add($"Выносливость противника: {Enemy.Strength}");
                fight.Add(String.Empty);

                round += 1;
            }
        }
    }
}
