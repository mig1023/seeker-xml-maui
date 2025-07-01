using System;
using System.Runtime.Intrinsics.X86;

namespace SeekerMAUI.Gamebook.StarshipTraveller
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public bool SpaceCombat { get; set; }
        public bool HandToHandCombat { get; set; }
        public bool BlasterCombat { get; set; }
        public string Crew { get; set; }
        public int Max { get; set; }

        public int Count { get; set; }
        public bool ByOne { get; set; }
        public bool ByOneAndTwo { get; set; }
        public bool ByFourAndMore { get; set; }
        public bool ByLuck { get; set; }
        public bool BySkill { get; set; }
        public bool ByScienceSkill { get; set; }
        public bool ByMedicalSkill { get; set; }
        public bool ByEngineeringSkill { get; set; }
        public bool ByShields { get; set; }

        public List<Character> Enemies { get; set; }

        public override List<string> Status() => new List<string>
        {
            $"Вооружение: {Character.Protagonist.Weapons}",
            $"Защита: {Character.Protagonist.Shields}/{Character.Protagonist.MaxShields}",
            $"Удача: {Character.Protagonist.Luck}"
        };

        public override List<string> AdditionalStatus()
        {
            var statusLines = new List<string>();

            foreach (var team in Constants.Team)
            {
                var crew = Character.Team[team];
                var name = Constants.Names[team];
                string isAlive = crew.Stamina > 0 ? string.Empty : "CROSSEDOUT|";
                statusLines.Add($"{isAlive}{name}");
            }

            return statusLines;
        }

        public override bool GameOver(out int toEndParagraph, out string toEndText) =>
            GameOverBy(Character.Protagonist.Shields, out toEndParagraph, out toEndText);

        public override List<string> Representer()
        {
            List<string> enemies = new List<string>();

            if (Enemies == null)
                return enemies;

            if (SpaceCombat)
            {
                var enemy = Enemies.First();
                enemies.Add($"{enemy.Name}\nвооружение {enemy.Weapons}  защита {enemy.Shields}");
            }

            return enemies;
        }

        public override bool IsButtonEnabled(bool secondButton = false)
        {
            if (Type == "Select")
            {
                var count = Character.Team.Where(x => x.Value.Selected).Count();
                var isAlive = Character.Team[Crew].Stamina > 0;
                var already = Character.Team[Crew].Selected;

                return isAlive && (count < Max) && !already;
            }
            else
            {
                return true;
            }
        }

        public List<string> Select()
        {
            Character.Team[Crew].Selected = true;

            return new List<string> { "RELOAD" };
        }

        public List<string> Fight()
        {
            if (SpaceCombat)
            {
                return Fights.SpaceCombat(this, Enemies);
            }
            else if (HandToHandCombat)
            {
                return Fights.HandToHandCombat(this, Enemies);
            }
            else
            {
                return Fights.BlasterCombat(this, Enemies);
            }
        }
        
        private List<string> DicesResultLine(List<string> lines, string text, string tag)
        {
            lines.Add($"BIG|BOLD|{text}!");
            Game.Buttons.Disable(tag);

            return lines;
        }

        private List<string> DicesResult(List<string> lines, bool first,
            string firstText, string firstTag, string secondText, string secondTag)
        {
            return first ? DicesResultLine(lines, firstText, firstTag) : DicesResultLine(lines, secondText, secondTag);
        }

        public List<string> Dices()
        {
            List<string> diceCheck = new List<string> { };

            int firstDice = Game.Dice.Roll();
            int dicesResult = firstDice;

            if (Count > 1)
            {
                int secondDice = Game.Dice.Roll();
                dicesResult += secondDice;

                diceCheck.Add($"BIG|На кубиках выпало: " +
                    $"{Game.Dice.Symbol(firstDice)} + " +
                    $"{Game.Dice.Symbol(secondDice)} = {dicesResult}");
            }
            else
            {
                diceCheck.Add($"BIG|На кубикe выпало: {Game.Dice.Symbol(firstDice)}");
            }

            if (ByOne)
            {
                return DicesResult(diceCheck, dicesResult == 1,
                    "Выпала единица", "notONE", "Выпала НЕ единица!", "ONE");
            }
            else if (ByOneAndTwo)
            {
                return DicesResult(diceCheck, dicesResult >= 3,
                    "Выпало значение больше двух!", "ONEorTWO", "Выпала единица или двойка!", "notONEorTWO");
            }
            else if (ByFourAndMore)
            {
                return DicesResult(diceCheck, dicesResult >= 4,
                    "Выпало значение больше трёх!", "FOURandMORE", "Выпало значение меньше трёх!", "notFOURandMORE");
            }
            else if (ByLuck)
            {
                int luck = Character.Team["Captain"].Luck;
                diceCheck.Add($"BIG|Удачливость капитана: {luck}");

                return DicesResult(diceCheck, luck <= dicesResult,
                    "Выпало значение не превышающее Удачливости!", "goodLuck",
                    "Выпало значение превышающее Удачливость!", "badLuck");
            }
            else if (BySkill)
            {
                int skill = Character.Team["Captain"].Skill;
                diceCheck.Add($"BIG|Мастерство капитана: {skill}");

                return DicesResult(diceCheck, skill <= dicesResult,
                    "Выпало значение не превышающее Мастерства!", "goodSkill",
                    "Выпало значение превышающее Мастерство!", "badSkill");
            }
            else if (ByScienceSkill)
            {
                int skill = Character.Team["ScienseOfficer"].Skill;
                diceCheck.Add($"BIG|Мастерство Офицера по науке: {skill}");

                return DicesResult(diceCheck, skill <= dicesResult,
                    "Выпало значение не превышающее Мастерства!", "goodSkill",
                    "Выпало значение превышающее Мастерство!", "badSkill");
            }
            else if (ByMedicalSkill)
            {
                int skill = Character.Team["MedicalOfficer"].Skill;
                diceCheck.Add($"BIG|Мастерство Судовой врача: {skill}");

                return DicesResult(diceCheck, skill <= dicesResult,
                    "Выпало значение не превышающее Мастерства!", "goodSkill",
                    "Выпало значение превышающее Мастерство!", "badSkill");
            }
            else if (ByEngineeringSkill)
            {
                int skill = Character.Team["EngineeringOfficer"].Skill;
                diceCheck.Add($"BIG|Мастерство Старшешл инженера: {skill}");

                return DicesResult(diceCheck, skill <= dicesResult,
                    "Выпало значение не превышающее Мастерства!", "goodSkill",
                    "Выпало значение превышающее Мастерство!", "badSkill");
            }
            else if (ByShields)
            {
                int shields = Character.Protagonist.Shields;
                diceCheck.Add($"BIG|Защита корабля: {shields}");

                return DicesResult(diceCheck, shields <= dicesResult,
                    "Выпало значение не превышающее Защиту корабля!", "goodShields",
                    "Выпало значение превышающее Удачливость!", "badShields");
            }

            return diceCheck;
        }
    }
}
