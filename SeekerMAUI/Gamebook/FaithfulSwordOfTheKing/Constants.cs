using System.Collections.Generic;

namespace SeekerMAUI.Gamebook.FaithfulSwordOfTheKing
{
    class Constants : Prototypes.Constants, Abstract.IConstants
    {
        public static Dictionary<int, int> Skills { get; set; }

        public static Dictionary<int, int> Strengths { get; set; }

        public static Dictionary<string, string> Availabilities { get; set; }

        public static Dictionary<Character.MeritalArts, string> MeritalArtsNames = 
            new Dictionary<Character.MeritalArts, string>
        {
            [Character.MeritalArts.Nope] = "нет",
            [Character.MeritalArts.SecretBlow] = "тайный удар шпагой",
            [Character.MeritalArts.SwordAndDagger] = "бой со шпагой и кинжалом",
            [Character.MeritalArts.TwoPistols] = "стрельба из двух пистолетов",
            [Character.MeritalArts.LefthandFencing] = "фехтование левой рукой",
            [Character.MeritalArts.Swimming] = "плавание",
        };
    }
}
