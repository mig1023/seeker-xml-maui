using System;

namespace SeekerMAUI.Gamebook.OctopusIsland
{
    class Fights
    {     
        public static bool SetCurrentWarrior(ref List<string> fight, bool start = false)
        {
            if (!start && (Character.CurrentWarrior.Hitpoint > 3))
                return true;

            var pastWarrior = Character.CurrentWarrior?.Name ?? string.Empty;

            if (Character.Thibaut.Hitpoint > 3)
            {
                Character.CurrentWarrior = Character.Thibaut;
            }
            else if (Character.Xolotl.Hitpoint > 3)
            {
                Character.CurrentWarrior = Character.Xolotl;
            }
            else if (Character.Serge.Hitpoint > 3)
            {
                Character.CurrentWarrior = Character.Serge;
            }
            else if (Character.Souhi.Hitpoint > 3)
            {
                Character.CurrentWarrior = Character.Souhi;
            }
            else
            {
                fight.Add($"GRAY|Дело кажется безнадёжным, " +
                    $"друзья не могут позволить Суи продолжать бой!!");

                return false;
            }

            if (!start)
            {
                fight.Add($"GRAY|{pastWarrior} ранен слишком серьёзно " +
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
