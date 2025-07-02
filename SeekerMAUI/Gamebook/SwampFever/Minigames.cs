using System;

namespace SeekerMAUI.Gamebook.SwampFever
{
    class Minigames
    {
        public static List<string> TrackPull(Actions actions)
        {
            List<string> pullReport = new List<string>();

            int thrust = 0;

            for (int i = 0; i < 4; i++)
            {
                int pull = Game.Dice.Roll();

                pullReport.Add($"Тяга гусениц: {Game.Dice.Symbol(pull)}");

                thrust += pull;
            }

            pullReport.Add($"Итого, вы развили тягу: {thrust}");
            pullReport.Add(actions.Result(thrust >= 14, "Вы вытащили ялик", "Трос оборвался и ялик утонул"));

            return pullReport;
        }

        public static List<string> PropellersPull(Actions actions)
        {
            List<string> pullReport = new List<string>();

            int thrust = 0;

            for (int i = 0; i < 4; i++)
            {
                int pull = Game.Dice.Roll();

                if (pull > 2)
                {
                    pullReport.Add($"Тяга гребных винтов: " +
                        $"{Game.Dice.Symbol(pull)}, -2 за винты, итого {pull - 2}");

                    thrust += (pull - 2);
                }
                else
                {
                    pullReport.Add($"Тяга гребных винтов: " +
                        $"{Game.Dice.Symbol(pull)}, +1 бонусный бросок");

                    thrust += pull;
                    i -= 1;
                }

                if (thrust >= 14)
                    break;
            }

            pullReport.Add($"Итого, вы развили тягу: {thrust}");
            pullReport.Add(actions.Result(thrust >= 14, "Вы вытащили ялик", "Трос оборвался и ялик утонул"));

            return pullReport;
        }

        public static List<string> TugOfWar(Actions actions)
        {
            List<string> warReport = new List<string>();

            int position = 0;
            bool battleCry = false;

            while ((position > -3) && (position < 3))
            {
                if (position != 0)
                {
                    string positionType = (position > 0 ? "побеждаете" : "проигрываете");
                    warReport.Add($"BOLD|ПОЛОЖЕНИЕ: вы {positionType} на {Math.Abs(position)} шаг");
                }
                else
                {
                    warReport.Add("BOLD|ПОЛОЖЕНИЕ: на исходной точке");
                }

                bool twoStep = false;
                int myСhoice = 0;

                int yatiForce = 10 + (Math.Abs(position) * 2);
                warReport.Add($"Яти тянет: {yatiForce}");

                int erikForce = Game.Dice.Roll();
                warReport.Add($"Эрик тянет: {Game.Dice.Symbol(erikForce)}");

                int jonyForce = Game.Dice.Roll();
                warReport.Add($"Джонни тянет: {Game.Dice.Symbol(jonyForce)}");

                int myForce = Game.Dice.Roll();
                warReport.Add($"Вы тянете: {Game.Dice.Symbol(myForce)}");

                int totalForce = erikForce + jonyForce + myForce;

                if (battleCry)
                {
                    totalForce += 1;
                    warReport.Add($"+1 к тяге за боевой клич на прошлом этапе, " +
                        $"итого тяга: {totalForce}");
                }

                battleCry = false;

                do
                {
                    myСhoice = Game.Dice.Roll();
                }
                while (myСhoice > 4);

                switch (myСhoice)
                {
                    case 1:
                        warReport.Add("Ваша тактика: «Силовой отход»");
                        twoStep = true;
                        break;

                    case 2:
                        warReport.Add("Ваша тактика: «Боевой клич»");
                        battleCry = true;
                        break;

                    case 3:
                        totalForce += 2;

                        warReport.Add("Ваша тактика: «Резкий рывок»");
                        warReport.Add($"+2 к вашей тяге за рывок, итого тяга: {totalForce}");
                        break;

                    case 4:
                        warReport.Add("Ваша тактика: «Синергия»");

                        if ((myForce == erikForce) || (myForce == jonyForce))
                        {
                            string force = myForce == erikForce ? "Эрика" : "Джонни";
                            string coincidence = erikForce == jonyForce ?
                                "у всех разом" : $"со значением {force}";

                            warReport.Add($"Значения тяги совпало {coincidence}, " +
                                $"общая тяга умножается вдвое!");

                            totalForce *= 2;
                        }
                        else
                        {
                            warReport.Add("Значения тяги не совпали, общая тяга не изменилась...");
                        }

                        break;
                }

                warReport.Add($"Общая тяга: {totalForce}");

                if (totalForce > yatiForce)
                {
                    string twoLine = twoStep ? " дважды" : String.Empty;
                    warReport.Add($"GOOD|Вы пересилили яти! Он шагнул вперёд{twoLine}!");
                    position += (twoStep ? 2 : 1);
                }
                else if (totalForce < yatiForce)
                {
                    warReport.Add("BAD|Яти вас пересилил! Вы шагнул вперёд!");
                    position -= 1;
                }
                else
                {
                    warReport.Add("BOLD|Ничья на этом этапе.");
                }

                warReport.Add(String.Empty);
            }

            warReport.Add(actions.Result(position > 0, "Вы выиграли", "Вы проиграли"));

            return warReport;
        }

        public static List<string> Hunt()
        {
            List<string> huntReport = new List<string>();

            int myPosition = 0, targetPosition = 0;
            bool skipStepAfterShot = false;

            while ((myPosition < 18) && (targetPosition < 18))
            {
                targetPosition += 3;
                huntReport.Add($"BOLD|Зверь убежал на клетку {targetPosition}");

                if (skipStepAfterShot)
                {
                    huntReport.Add($"Вы остаётесь на клетке {myPosition}, т.к. стреляли");
                }
                else if (targetPosition <= myPosition)
                {
                    huntReport.Add($"Вы остаётесь на клетке {myPosition}, чтобы подстеречь зверя");
                }
                else
                {
                    int forwarding = Game.Dice.Roll();
                    myPosition += forwarding;

                    huntReport.Add($"Вы догоняете и проезжаете " +
                        $"{Game.Dice.Symbol(forwarding)} до клетки {myPosition}");
                }

                skipStepAfterShot = false;

                int distance = Math.Abs(myPosition - targetPosition);

                if (distance <= 1)
                {
                    string distanceLine = distance == 0 ? "4, 5 или 6" : "5 или 6";
                    huntReport.Add("Зверь рядом и вы принимаете решение стрелять.");
                    huntReport.Add($"Для попадания необходимо выкинуть {distanceLine}");

                    int shot = Game.Dice.Roll();
                    huntReport.Add($"Ваш выстрел: {Game.Dice.Symbol(shot)}");

                    if (((distance == 0) && (shot > 3)) || ((distance > 0) && (shot > 4)))
                    {
                        Character.Protagonist.Stigon += 1;

                        huntReport.Add("BIG|GOOD|Вы подстрелили зверя :)");
                        return huntReport;
                    }
                    else
                    {
                        huntReport.Add("BAD|Вы промахнулись");
                        skipStepAfterShot = true;
                    }
                }

                huntReport.Add(String.Empty);
            }

            huntReport.Add("BIG|BAD|Вы упустили зверя :(");

            return huntReport;
        }

        public static List<string> Pursuit()
        {
            List<string> pursuitReport = new List<string>();

            while (true)
            {
                bool reRoll = false;

                Game.Dice.DoubleRoll(out int tumbleweedDirection, out int tumbleweedSpeed);

                pursuitReport.Add($"BOLD|Направление движения куста: " +
                    $"{Game.Dice.Symbol(tumbleweedDirection)}, " +
                    $"скорость: {Game.Dice.Symbol(tumbleweedSpeed)}");

                int myDirection = Game.Dice.Roll();
                int mySpeed = Game.Dice.Roll();

                pursuitReport.Add($"Ваше направление: " +
                    $"{Game.Dice.Symbol(myDirection)}, " +
                    $"скорость: {Game.Dice.Symbol(mySpeed)}");

                if ((myDirection == tumbleweedDirection) && (mySpeed == tumbleweedSpeed))
                    return Details.PursuitWin(pursuitReport);

                if (myDirection == tumbleweedDirection)
                {
                    reRoll = true;

                    mySpeed = Game.Dice.Roll();

                    pursuitReport.Add($"Вы почти настигли куст и меняете скорость: " +
                        $"{Game.Dice.Symbol(mySpeed)}");

                    if (mySpeed == tumbleweedSpeed)
                        return Details.PursuitWin(pursuitReport);
                }
                else if (mySpeed == tumbleweedSpeed)
                {
                    reRoll = true;

                    myDirection = Game.Dice.Roll();
                    pursuitReport.Add($"Вы почти настигли куст и меняете направление: " +
                        $"{Game.Dice.Symbol(myDirection)}");

                    if (myDirection == tumbleweedDirection)
                        return Details.PursuitWin(pursuitReport);
                }

                pursuitReport.Add("BAD|Настигнуть куст не удалось");

                if ((tumbleweedDirection + tumbleweedSpeed) <= (myDirection + mySpeed))
                {
                    pursuitReport.Add("Преследование продолжается");
                }
                else if (reRoll)
                {
                    return Details.PursuitFail(pursuitReport);
                }
                else
                {
                    if (myDirection > mySpeed)
                    {
                        mySpeed = Game.Dice.Roll();
                        pursuitReport.Add($"Вы пытаетесь резко ускориться: " +
                            $"{Game.Dice.Symbol(mySpeed)}");
                    }
                    else
                    {
                        myDirection = Game.Dice.Roll();
                        pursuitReport.Add($"Вы пытаетесь резко сменить курс: " +
                            $"{Game.Dice.Symbol(myDirection)}");
                    }

                    if ((tumbleweedDirection + tumbleweedSpeed) <= (myDirection + mySpeed))
                    {
                        pursuitReport.Add("Преследование продолжается");
                    }
                    else
                    {
                        return Details.PursuitFail(pursuitReport);
                    }
                }

                pursuitReport.Add(String.Empty);
            }
        }

        private static int ThinkAboutMovement(int myPosition, int step, List<int> bombs, ref List<string> cavityReport)
        {
            int myMovementType = 0;

            if (!bombs.Contains(myPosition + 4) && !bombs.Contains(myPosition + 3) && !bombs.Contains(myPosition + 5))
            {
                cavityReport.Add("Думаем: попробуем рвануть на гусеницах");
                myMovementType = 6;
            }
            else if (!bombs.Contains(myPosition + 2) && (!bombs.Contains(myPosition + 1) || !bombs.Contains(myPosition + 3)))
            {
                cavityReport.Add("Думаем: попробуем тихонечко, на гребных винтах");
                myMovementType = 1;
            }
            else if ((step > 2))
            {
                cavityReport.Add("Думаем: опасно, но нужно срочно прорываться, иначе накроет лава!");
                myMovementType = 6;
            }
            else
            {
                cavityReport.Add("Думаем: лучше постоим нафиг");
            }

            if (bombs.Contains(myPosition) && (myMovementType == 0))
            {
                cavityReport.Add("Думаем: сейчас на вас упадёт вулканическая бомба - нужно рвать когти!");
                myMovementType = Game.Dice.Roll();
            }

            return myMovementType;
        }

        public static List<string> SulfurCavity()
        {
            List<string> cavityReport = new List<string>();

            int myPosition = 0;

            for (int step = 1; step <= 4; step++)
            {
                cavityReport.Add($"BOLD|Ход № {step}");

                List<int> bombs = new List<int>();

                for (int bomb = 0; bomb < 3; bomb++)
                    bombs.Add(Game.Dice.Roll());

                cavityReport.Add($"Вулканические бомбы бьют по клеткам: " +
                    $"{Game.Dice.Symbol(bombs[0])}, " +
                    $"{Game.Dice.Symbol(bombs[1])} и " +
                    $"{Game.Dice.Symbol(bombs[2])}");

                int myMovementType = ThinkAboutMovement(myPosition, step, bombs, ref cavityReport);
                int myMove = 0;

                if (myMovementType > 3)
                {
                    myMove = Game.Dice.Roll();
                    cavityReport.Add($"Движение на гусеницах, дальность: {Game.Dice.Symbol(myMove)}");
                }
                else if (myMovementType > 0)
                {
                    myMove = Game.Dice.Roll();

                    if (myMove > 2)
                    {
                        cavityReport.Add($"Движение на гребных винтах, " +
                            $"дальность: {Game.Dice.Symbol(myMove)}, " +
                            $"-2 за винты, итого {myMove - 2}");

                        myMove -= 2;
                    }
                    else
                    {
                        int propBonus = Game.Dice.Roll();

                        if (propBonus > 2)
                            propBonus -= 2;

                        cavityReport.Add($"Движение на гребных винтах, " +
                            $"дальность: {Game.Dice.Symbol(myMove)}, " +
                            $"+бонусный бросок: {Game.Dice.Symbol(propBonus)}, " +
                            $"итого {myMove + propBonus}");

                        myMove += propBonus;
                    }
                }

                myPosition += myMove;

                string move = myMovementType == 0 ? "остаётесь" : "останавливаетесь";
                cavityReport.Add($"Вы {move} на клетке {myPosition}");

                if (bombs.Contains(myPosition))
                {
                    cavityReport.Add("BIG|BAD|Вы уничтожены вулканической бомбой :(");
                    return cavityReport;
                }

                if (myPosition > 6)
                {
                    cavityReport.Add("BIG|GOOD|Вы прорвались :)");
                    return cavityReport;
                }

                cavityReport.Add(String.Empty);
            }

            cavityReport.Add("BIG|BAD|Вас накрыло потоком лавы :(");
            return cavityReport;
        }
    }
}
