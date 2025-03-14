﻿using System.Collections.Generic;

namespace SeekerMAUI.Gamebook.Moria
{
    class Constants : Prototypes.Constants, Abstract.IConstants
    {
        public static Dictionary<string, int> Fellowship { get; set; }

        public static Dictionary<string, int> Enemies { get; set; }

        public static Dictionary<string, string> Declination { get; set; }
    }
}
