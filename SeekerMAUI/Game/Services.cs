﻿using Microsoft.Maui.Controls.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace SeekerMAUI.Game
{
    public class Services
    {
        public static string Сomparison(int a, int b)
        {
            if (a > b)
            {
                return "больше";
            }
            else if (a < b)
            {
                return "меньше";
            }
            else
            {
                return "равно";
            }
        }

        public static string CoinsNoun(int value, string one, string two, string five)
        {
            int absValue = Math.Abs(value);

            if (absValue % 10 == 5)
                return five;

            absValue %= 100;

            if ((absValue >= 5) && (absValue <= 20))
                return five;

            absValue %= 10;

            if (absValue == 1)
                return one;

            if ((absValue >= 2) && (absValue <= 5))
                return two;

            return five;
        }

        public static string NegativeMeaning(int value) =>
            value < 0 ? $"минус {Math.Abs(value)}" : value.ToString(); 

        public static int LevelParse(string option) =>
            int.Parse(option.Contains("=") ? option.Split('=')[1] : option.Split('>', '<')[1]);

        public static bool LevelAvailability(string option, string line, int value, int level)
        {
            if (!line.Contains(option))
                return true;

            else if (line.Contains("<="))
                return value <= level;

            else if (line.Contains(">="))
                return value >= level;

            else if (line.Contains("<"))
                return value < level;

            else if (line.Contains(">"))
                return value > level;

            else if (line.Contains("!="))
                return value != level;

            else if (line.Contains("="))
                return value == level;

            else
                return true;
        }

        public static bool AvailabilityByСomparison(string option) =>
            option.Contains(">") || option.Contains("<") || option.Contains("=");

        public static bool AvailabilityByProperty(object protagonist, string option,
            Dictionary<string, string> properties, bool onlyFailTrueReturn = false)
        {
            foreach (var property in properties)
            {
                if (!option.Contains(property.Key))
                    continue;

                var value = (int)protagonist
                    .GetType()
                    .GetProperty(property.Value)
                    .GetValue(protagonist, null);

                var level = LevelParse(option);

                var result = LevelAvailability(property.Key, option, value, level);

                if (onlyFailTrueReturn)
                {
                    if (!result)
                        return true;
                }
                else
                {
                    return result;
                }
            }

            return !onlyFailTrueReturn;
        }

        public static bool DoNothing() =>
            true;

        public static string ValueStringFuse(string value) =>
            value == "ValueString" ? "Value" : value;

        public static bool ParagraphsWithoutStatuses(List<string> statuses) =>
            statuses == null || Data.Constants.GetParagraphsWithoutStatuses().Contains(Data.CurrentParagraphID);

        public static void BookmarkName(Dictionary<string, string> bookmarks,
            string bookmarkIn, out string bookmarkOut, out string saveName)
        {
            int bookmarkIndex = 0;
            bookmarkOut = bookmarkIn;

            while (bookmarks.Keys.Contains(bookmarkOut))
            {
                bookmarkIndex += 1;
                bookmarkOut = Regex.Replace(bookmarkOut, @"\s+\(\d\)$", String.Empty);
                bookmarkOut += $" ({bookmarkIndex})";
            };

            int nextSaveGameIndex = 0;

            do
            {
                nextSaveGameIndex += 1;
                saveName = $"SAVE{nextSaveGameIndex}";
            }
            while (bookmarks.Values.Contains(saveName));
        }
    }
}
