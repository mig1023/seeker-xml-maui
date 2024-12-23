using System;

namespace SeekerMAUI.Gamebook.Tremble
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public Character Enemy { get; set; }

        public int RoundsToFight { get; set; }

        public override List<string> Status() => new List<string>
        {
            $"Ловкость: {Character.Protagonist.Skill}/{Character.Protagonist.MaxSkill}",
            $"Выносливость: {Character.Protagonist.Endurance}/{Character.Protagonist.MaxEndurance}",
            $"Счастье: {Character.Protagonist.Luck}/{Character.Protagonist.MaxLuck}",
        };

        public override List<string> Representer()
        {
            List<string> enemies = new List<string>();

            if (Enemy != null)
            {
                enemies.Add($"{Enemy.Name}\nмастерство {Enemy.Skill}  выносливость {Enemy.Endurance}");
            }

            return enemies;
        }

        public override bool GameOver(out int toEndParagraph, out string toEndText) =>
            GameOverBy(Character.Protagonist.Endurance, out toEndParagraph, out toEndText);

        public override bool Availability(string option) =>
            AvailabilityTrigger(option);

        public List<string> GoodLuck()
        {
            Game.Dice.DoubleRoll(out int firstDice, out int secondDice);
            var goodLuck = (firstDice + secondDice) < Character.Protagonist.Luck;
            var luckLine = goodLuck ? "<=" : ">";

            List<string> luckCheck = new List<string> {
                $"Проверка удачи: {Game.Dice.Symbol(firstDice)} + " +
                $"{Game.Dice.Symbol(secondDice)} {luckLine} {Character.Protagonist.Luck}" };

            if (firstDice == secondDice)
            {
                luckCheck.Add("На кубиках выпал дубль - это всегда удача!");
                goodLuck = true;
            }

            luckCheck.Add(goodLuck ? "BIG|GOOD|УСПЕХ :)" : "BIG|BAD|НЕУДАЧА :(");
            Game.Buttons.Disable(goodLuck, "Win", "Fail");

            if (Character.Protagonist.Luck > 1)
            {
                Character.Protagonist.Luck -= 1;
                luckCheck.Add("Уровень удачи снижен на единицу");
            }

            return luckCheck;
        }

        public List<string> Fight()
        {
            List<string> fight = new List<string>();

            int round = 1;

            while (true)
            {
                fight.Add($"HEAD|BOLD|Раунд: {round}");
                fight.Add($"{Enemy.Name} (выносливость {Enemy.Endurance})");

                Game.Dice.DoubleRoll(out int protagonistRollFirst, out int protagonistRollSecond);

                var protagonistAttack = protagonistRollFirst + 
                    protagonistRollSecond + Character.Protagonist.Skill;

                fight.Add($"Твоя атака: " +
                    $"{Game.Dice.Symbol(protagonistRollFirst)} + " +
                    $"{Game.Dice.Symbol(protagonistRollSecond)} + " +
                    $"{Character.Protagonist.Skill} = {protagonistAttack}");

                Game.Dice.DoubleRoll(out int enemyRollFirst, out int enemyRollSecond);
                int enemyAttack = enemyRollFirst + enemyRollSecond + Enemy.Skill;

                fight.Add($"Атака чудовища: " +
                    $"{Game.Dice.Symbol(enemyRollFirst)} + " +
                    $"{Game.Dice.Symbol(enemyRollSecond)} + " +
                    $"{Enemy.Skill} = {enemyAttack}");

                if (protagonistAttack > enemyAttack)
                {
                    fight.Add($"GOOD|{Enemy.Name} ранен");

                    Enemy.Endurance -= 2;

                    if (Enemy.Endurance <= 0)
                        return Win(fight, you: true);
                }
                else if (protagonistAttack < enemyAttack)
                {
                    fight.Add($"BAD|{Enemy.Name} ранил тебя");

                    Character.Protagonist.Endurance -= 2;

                    if (Character.Protagonist.Endurance <= 0)
                        return Fail(fight, you: true);
                }
                else
                {
                    fight.Add("BOLD|Ничья в раунде");
                }

                if ((RoundsToFight > 0) && (RoundsToFight <= round))
                {
                    fight.Add("BAD|Отведённые на битву раунды истекли.");
                    return Fail(fight, you: true);
                }

                fight.Add(String.Empty);

                round += 1;
            }
        }
    }
}
