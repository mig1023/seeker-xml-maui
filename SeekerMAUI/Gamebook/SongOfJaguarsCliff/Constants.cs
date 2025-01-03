﻿using System.Collections.Generic;

namespace SeekerMAUI.Gamebook.SongOfJaguarsCliff
{
    class Constants : Prototypes.Constants, Abstract.IConstants
    {
        public static List<string> PriorityNames { get; set; }

        public static Dictionary<string, string> Availabilities { get; set; }
    }
}
