using System;

namespace SeekerMAUI.Gamebook.OctopusIsland
{
    class Fights
    {
        //public static void SaveCurrentWarriorHitPoints()
        //{
        //    if (String.IsNullOrEmpty(Character.Protagonist.Name))
        //        return;

        //    if (Character.Protagonist.Name == "Тибо")
        //    {
        //        Character.Protagonist.ThibautHitpoint = Character.Protagonist.Hitpoint;
        //    }
        //    else if (Character.Protagonist.Name == "Ксолотл")
        //    {
        //        Character.Protagonist.XolotlHitpoint = Character.Protagonist.Hitpoint;
        //    }
        //    else if (Character.Protagonist.Name == "Серж")
        //    {
        //        Character.Protagonist.SergeHitpoint = Character.Protagonist.Hitpoint;
        //    }
        //    else
        //    {
        //        Character.Protagonist.SouhiHitpoint = Character.Protagonist.Hitpoint;
        //    }
        //}

        //private static void ShowCurrentWarrior(ref List<string> fight, bool start)
        //{
            
        //}
        
        public static bool SetCurrentWarrior(ref List<string> fight, bool start = false)
        {
            if ((Character.CurrentWarrior.Hitpoint > 3) && !start)
                return true;

            var pastWarrior = Character.CurrentWarrior.Name;

            //SaveCurrentWarriorHitPoints();

            if (Character.Thibaut.Hitpoint > 3)
            {
                Character.CurrentWarrior = Character.Thibaut;
                //Character.Protagonist.Name = "Тибо";
                //Character.Protagonist.Skill = Character.Protagonist.ThibautSkill;
                //Character.Protagonist.Hitpoint = Character.Protagonist.ThibautHitpoint;
            }
            else if (Character.Xolotl.Hitpoint > 3)
            {
                Character.CurrentWarrior = Character.Xolotl;
                //Character.Protagonist.Name = "Ксолотл";
                //Character.Protagonist.Skill = Character.Protagonist.XolotlSkill;
                //Character.Protagonist.Hitpoint = Character.Protagonist.XolotlHitpoint;
            }
            else if (Character.Serge.Hitpoint > 3)
            {
                Character.CurrentWarrior = Character.Serge;
                //Character.Protagonist.Name = "Серж";
                //Character.Protagonist.Skill = Character.Protagonist.SergeSkill;
                //Character.Protagonist.Hitpoint = Character.Protagonist.SergeHitpoint;
            }
            else if (Character.Souhi.Hitpoint > 3)
            {
                Character.CurrentWarrior = Character.Souhi;
                //Character.Protagonist.Name = "Суи";
                //Character.Protagonist.Skill = Character.Protagonist.SouhiSkill;
                //Character.Protagonist.Hitpoint = Character.Protagonist.SouhiHitpoint;
            }
            else
            {
                fight.Add($"GRAY|Дело кажется безнадёжным, " +
                    $"друзья не могут позволить Суи продолжать бой!!");

                return false;
            }

            if (!start)
            {
                fight.Add($"GRAY|{Character.CurrentWarrior.Name} ранен слишком серьёзно " +
                    $"и не может продолжать бой! Товарищ должен принять меч из израненных рук!");

                fight.Add(String.Empty);
            }

            fight.Add($"HEAD|BOLD|*** В БОЙ ВСТУПАЕТ {Character.CurrentWarrior.Name.ToUpper()} ***");

            if (start)
            {
                fight.Add(String.Empty);
            }

            return true;
        }
    }
}
