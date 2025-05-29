using Microsoft.Maui.Controls.Shapes;
using System;

namespace SeekerMAUI.Gamebook.CastleOfLostSouls
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public Character Enemy { get; set; }
        public int Dices { get; set; }
        public string Stat { get; set; }
        public bool Wound { get; set; }

        public override List<string> Status() => new List<string>
        {
            $"Честь: {Character.Protagonist.Honor}",
            $"Доспехи: {Character.Protagonist.Armour}",
            $"Золото: {Character.Protagonist.Gold}",
        };

        public override List<string> AdditionalStatus() => new List<string>
        {
            $"Боевая доблесть: {Character.Protagonist.Combat}",
            $"Телосложение: {Character.Protagonist.Constitution}",
            $"Сообразительность: {Character.Protagonist.Ingenuity}",
            $"Магическая стойкость: {Character.Protagonist.Resistence}",
        };

        public override bool GameOver(out int toEndParagraph, out string toEndText) =>
            GameOverBy(Character.Protagonist.Constitution, out toEndParagraph, out toEndText);

        public override List<string> Representer()
        {
            List<string> enemies = new List<string>();

            if (Enemy == null)
                return enemies;

            string line = $"{Enemy.Name}\nБоевая доблесть " +
                $"{Enemy.Combat}  Телосложение {Enemy.Constitution}";

            if (Enemy.Armour > 0)
                line += $"  Доспехи {Enemy.Armour}";

            enemies.Add(line);

            return enemies;
        }

        public List<string> Fight()
        {
            List<string> fight = new List<string>();

            int round = 1;

            while (true)
            {
                fight.Add($"HEAD|BOLD|Раунд: {round}");

                Game.Dice.DoubleRoll(out int firstDice, out int secondDice);

                var dices = firstDice + secondDice;

                fight.Add("BOLD|ВЫ АТАКУЕТЕ:");
                fight.Add($"Бросок на попадание: {Game.Dice.Symbol(firstDice)} + " +
                    $"{Game.Dice.Symbol(secondDice)} = {dices}");

                if (dices <= Character.Protagonist.Combat)
                {
                    fight.Add($"GOOD|Сумма {dices} не превышает Доблесть " +
                        $"{Character.Protagonist.Combat}, значит вы попали по противнику!");

                    var wound = Game.Dice.Roll();

                    fight.Add($"Бросок на повреждение: {Game.Dice.Symbol(wound)}");

                    if (Enemy.Armour > 0)
                    {
                        wound -= Enemy.Armour;

                        fight.Add($"Из броска на повреждение вычитается {Enemy.Armour} " +
                            $"за доспехи противника, в результате бросок теперь равен {wound}");
                    }

                    if (wound > 0)
                    {
                        fight.Add("GOOD|BOLD|Противник ранен!");

                        Enemy.Constitution -= wound;

                        fight.Add($"GOOD|Противник теряет {wound} ед. Телосложения! " +
                            $"В результате, у него осталось {Enemy.Constitution} ед. Телосложения.");

                        if (Enemy.Constitution <= 0)
                            return Win(fight);
                    }
                }
                else
                {
                    fight.Add($"BAD|BOLD|Сумма {dices} превышает Доблесть " +
                        $"{Character.Protagonist.Combat}, значит вы промахнулись");
                }

                Game.Dice.DoubleRoll(out firstDice, out secondDice);

                dices = firstDice + secondDice;

                fight.Add("BOLD|ПРОТИВНИК АТАКУЕТ:");
                fight.Add($"Бросок на попадание: {Game.Dice.Symbol(firstDice)} + " +
                    $"{Game.Dice.Symbol(secondDice)} = {dices}");

                if (dices <= Enemy.Combat)
                {
                    fight.Add($"BAD|Сумма {dices} не превышает его Доблесть " +
                        $"{Enemy.Combat}, значит он попали по вам!");

                    var wound = Game.Dice.Roll();

                    fight.Add($"Бросок на повреждение: {Game.Dice.Symbol(wound)}");

                    if (Character.Protagonist.Armour > 0)
                    {
                        wound -= Character.Protagonist.Armour;

                        fight.Add($"Из броска на повреждение вычитается {Character.Protagonist.Armour} " +
                            $"за ваши доспехи, в результате его бросок теперь равен {wound}");
                    }

                    if (wound > 0)
                    {
                        fight.Add("BAD|BOLD|Вы ранены!");

                        Character.Protagonist.Constitution -= wound;

                        fight.Add($"BAD|Вы теряете {wound} ед. Телосложения! " +
                            $"В результате, у вас осталось {Character.Protagonist.Constitution} ед. Телосложения.");

                        if (Character.Protagonist.Constitution <= 0)
                            return Fail(fight);
                    }
                }
                else
                {
                    fight.Add($"GOOD|BOLD|Сумма {dices} превышает его Доблесть " +
                        $"{Character.Protagonist.Combat}, значит он промахнулся");
                }

                round += 1;
                fight.Add(String.Empty);
            }
        }

        private int AttacksCount(int dice)
        {
            if (dice <= 4)
                return 1;

            if (dice <= 6)
                return 2;

            return 3;
        }

        public List<string> Ambush()
        {
            var ambush = new List<string>();
            var dice = Game.Dice.Roll();
            var attacks = AttacksCount(dice);
            var count = Game.Services.CoinsNoun(attacks, "раз", "раза", "раз");

            ambush.Add($"BOLD|ОПРЕДЕЛЯЕМ КОЛИЧЕСТВО ПОПАДАНИЙ:");
            ambush.Add($"На кубике выпало: {Game.Dice.Symbol(dice)}");
            ambush.Add($"Это значит, что в вас попали {attacks} {count}");
            ambush.Add(string.Empty);
            ambush.Add($"BOLD|СЧИТАЕМ ПОПАДАНИЯ:");

            var wounds = 0;

            for (int i = 1; i <= attacks; i++)
            {
                var attack = Game.Dice.Roll();
                var line = attacks > 1 ? $"{i} " : string.Empty;
                ambush.Add($"На {line}кубике выпало: {Game.Dice.Symbol(attack)}");

                if (Character.Protagonist.Armour > 0)
                {
                    attack -= Character.Protagonist.Armour;

                    ambush.Add($"Из значения кубика вычиается значение доспехов, " +
                        $"равное {Character.Protagonist.Armour}");
                }

                if (attack > 0)
                {
                    wounds += attack;
                    ambush.Add($"BAD|Вы получаете {attack} ед. повреждений");
                }
                else
                {
                    ambush.Add("GOOD|Доспехи вас защитили!");
                }

                ambush.Add(string.Empty);
            }

            if (wounds <= 0)
            {
                ambush.Add("GOOD|BOLD|ИТОГО ВЫ ОТДЕЛАЛИСЬ ЛЁГКИМ ИСПУГОМ! :)");
            }
            else
            {
                ambush.Add($"BAD|BOLD|ИТОГО ВЫ ПОТЕРЯЛИ {wounds} ед. ТЕЛОСЛОЖЕНИЯ :(");
                Character.Protagonist.Constitution -= wounds;
            }
                
            return ambush;
        }

        public List<string> Test()
        {
            var test = new List<string>();
            var dices = 0;

            for (int i = 1; i <= Dices; i++)
            {
                var dice = Game.Dice.Roll();
                var line = Dices > 1 ? $"{i} " : string.Empty;
                dices += dice;
                test.Add($"На {line}кубике выпало: {Game.Dice.Symbol(dice)}");
            }

            if (Dices > 1)
            {
                test.Add($"BOLD|Итого на кубиках выпало: {dices}");
            }

            if (Wound)
            {
                Character.Protagonist.Constitution -= dices;
                test.Add($"BIG|BAD|BOLD|Вы потеряли {dices} ед. Телосложения!");
            }
            else
            {
                var currentStat = GetProperty(Character.Protagonist, Stat);
                
                if (dices > currentStat)
                {
                    test.Add($"Это больше показателя {Constants.StatNames[Stat]}, " +
                        $"который равен {currentStat}!");

                    test.Add("BIG|BAD|BOLD|Это ПРОВАЛ :(");
                }
                else
                {
                    test.Add($"Это не превышает показателя {Constants.StatNames[Stat]}, " +
                        $"который равен {currentStat}!");

                    test.Add("BIG|GOOD|BOLD|Это УСПЕХ :)");
                }
            }

            return test;
        }
    }
}
