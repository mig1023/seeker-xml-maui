﻿using SeekerMAUI.Game;
using System;
using System.Xml;

namespace SeekerMAUI.Gamebook.ChooseYourOwnAdventure
{
    class Paragraphs : Prototypes.Paragraphs, Abstract.IParagraphs
    {
        public override Paragraph Get(int id, XmlNode xmlParagraph) =>
           base.Get(xmlParagraph);

        public override Option OptionParse(XmlNode xmlOption)
        {
            Option option = OptionsTemplate(xmlOption);

            if (Constants.Buttons.ContainsKey(option.Goto))
                option.Style = Constants.Buttons[option.Goto];

            if (Constants.GetCurrentStartParagraph(Constants.Buttons) == Constants.MillionerStartParagraph)
            {
                if (String.IsNullOrEmpty(option.Text))
                    option.Text = (option.Goto == 0 ? "Начать сначала" : "Далее");

                option.Text = $"$$$  {option.Text}  $$$";
            }

            return option;
        }
    }
}
