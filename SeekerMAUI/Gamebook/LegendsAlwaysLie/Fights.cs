using System;
using System.Collections.Generic;
using System.Linq;

namespace SeekerMAUI.Gamebook.LegendsAlwaysLie
{
    class Fights
    {
        public static bool EnemyLost(List<Character> FightEnemies, ref List<string> fight, bool connery = false)
        {
            if (FightEnemies.Where(x => x.Hitpoints > 0).Count() > 0)
            {
                return false;
            }
            else
            {
                fight.Add(String.Empty);

                if (connery)
                {
                    fight.Add("BIG|GOOD|Коннери его добил, вы ПОБЕДИЛИ :)");
                }
                else
                {
                    fight.Add("BIG|GOOD|Вы ПОБЕДИЛИ :)");
                }

                return true;
            }
        }
    }
}
