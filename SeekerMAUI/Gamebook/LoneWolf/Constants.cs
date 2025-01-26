using System.Collections.Generic;

namespace SeekerMAUI.Gamebook.LoneWolf
{
    class Constants : Prototypes.Constants, Abstract.IConstants
    {
        public static Dictionary<int, string> CoefficientN12 { get; set; }
        public static Dictionary<int, string> CoefficientN10 { get; set; }
        public static Dictionary<int, string> CoefficientN8 { get; set; }
        public static Dictionary<int, string> CoefficientN6 { get; set; }
        public static Dictionary<int, string> CoefficientN4 { get; set; }
        public static Dictionary<int, string> CoefficientN2 { get; set; }
        public static Dictionary<int, string> Coefficient0 { get; set; }
        public static Dictionary<int, string> Coefficient2 { get; set; }
        public static Dictionary<int, string> Coefficient4 { get; set; }
        public static Dictionary<int, string> Coefficient6 { get; set; }
        public static Dictionary<int, string> Coefficient8 { get; set; }
        public static Dictionary<int, string> Coefficient10 { get; set; }
        public static Dictionary<int, string> Coefficient12 { get; set; }
    }
}
