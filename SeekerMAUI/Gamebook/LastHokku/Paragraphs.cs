﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using SeekerMAUI.Game;
using SeekerMAUI.Output;

namespace SeekerMAUI.Gamebook.LastHokku
{
    class Paragraphs : Prototypes.Paragraphs, Abstract.IParagraphs
    {
        public override Paragraph Get(int id, XmlNode xmlParagraph) =>
            base.Get(xmlParagraph);

        private List<string> HokkuFormat(List<string> text)
        {
            List<string> oldHokku = Character.Protagonist.Hokku;

            oldHokku[2] = new System.Globalization
                .CultureInfo("ru-RU", false)
                .TextInfo
                .ToTitleCase(oldHokku[2]);

            List<string> newHokku = new List<string>
            {
                $"{oldHokku[0]} {oldHokku[1]}",
                $"{oldHokku[2]} {oldHokku[3]}.",
                $"{oldHokku[4]} - {oldHokku[5]} {oldHokku[6]}...",
            };

            return newHokku;
        }

        private string TextByOptions(string option)
        {
            if (Character.Protagonist.Hokku == null)
            {
                return String.Empty;
            }

            bool wihoutHokku = Constants.GetParagraphsWithoutHokkuCreation
                .Contains(Game.Data.CurrentParagraphID);

            string last = Character.Protagonist.Hokku.LastOrDefault() ?? String.Empty;

            if (!wihoutHokku && (last != option) && !last.Contains('.'))
            {
                Character.Protagonist.Hokku.Add(option);

                if (Character.Protagonist.Hokku.Count >= 7)
                {
                    Character.Protagonist.Hokku = HokkuFormat(Character.Protagonist.Hokku);
                }
            }

            return String.Join("\n", Character.Protagonist.Hokku);
        }

        public override List<Text> TextsParse(XmlNode xmlNode, bool main = false)
        {
            string textByOption = TextByOptions(Data.CurrentSelectedOption);

            List<Text> text = new List<Text>();

            if (main && !String.IsNullOrEmpty(textByOption))
            {
                text.Add(Xml.TextLine(textByOption));
            }
            else if (xmlNode["Text"] != null)
            {
                text.Add(Xml.TextLineParse(xmlNode["Text"]));
            }

            return text;
        }
    }
}
