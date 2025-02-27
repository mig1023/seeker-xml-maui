﻿using System.Collections.Generic;

namespace SeekerMAUI.Gamebook.Moonrunner
{
    class Constants : Prototypes.Constants, Abstract.IConstants
    {
        public static Dictionary<int, string> SpellsList { get; set; }

        public static Dictionary<string, string> Availabilities { get; set; }

        public static List<string> Skills { get; set; }
    }
}
