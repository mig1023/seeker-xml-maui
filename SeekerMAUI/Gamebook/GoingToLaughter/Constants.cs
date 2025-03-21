﻿using System.Collections.Generic;
using System.Drawing;
using static SeekerMAUI.Game.Data;

namespace SeekerMAUI.Gamebook.GoingToLaughter
{
    class Constants : Prototypes.Constants, Abstract.IConstants
    {
        public override string GetColor(ColorTypes type)
        {
            if (Game.Settings.IsEnabled("WithoutStyles"))
            {
                return base.GetColor(type);
            }
            else if ((type == ColorTypes.ActionBox) && ParagraphWithSkills())
            {
                System.Drawing.Color myColor = System.Drawing.Color.FromArgb(LltRandom(), LltRandom(), LltRandom());
                return myColor.R.ToString("X2") + myColor.G.ToString("X2") + myColor.B.ToString("X2");
            }
            else
            {
                return base.GetColor(type);
            }
        }

        private static int LltRandom() =>
            200 + Game.Dice.Roll(size: 40);

        private static bool ParagraphWithSkills() =>
            (CurrentParagraphID == 1393) || (CurrentParagraphID == 1394);

        public static Dictionary<string, string> IncompatiblesDisadvantages { get; set; }

        public static Dictionary<string, string> ParamNames { get; set; }

        public static Dictionary<string, string> Availabilities { get; set; }

        public static List<string> SleepCleaningSurvive { get; set; }
    }
}
