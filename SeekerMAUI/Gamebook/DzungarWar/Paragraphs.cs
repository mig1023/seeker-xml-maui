﻿using System.Xml;
using SeekerMAUI.Game;

namespace SeekerMAUI.Gamebook.DzungarWar
{
    class Paragraphs : Prototypes.Paragraphs, Abstract.IParagraphs
    {
        public override Paragraph Get(int id, XmlNode xmlParagraph) =>
            base.Get(xmlParagraph);

        public override Abstract.IActions ActionParse(XmlNode xmlAction)
        {
            Actions action = (Actions)ActionTemplate(xmlAction, new Actions());

            foreach (string param in GetProperties(action))
                SetProperty(action, param, xmlAction);

            action.Benefit = ModificationParse(xmlAction["Benefit"]);

            bool bargain = Xml.BoolParse(xmlAction["Bargain"]);

            if (bargain && Game.Option.IsTriggered("Bargain"))
                action.Price /= 2;

            if (action.Type == "Option")
                action.Option = OptionParse(xmlAction);

            return action;
        }


        public override Option OptionParse(XmlNode xmlOption) =>
            OptionParseWithDo(xmlOption, new Modification());

        public override Abstract.IModification ModificationParse(XmlNode xmlModification) =>
           (Abstract.IModification)base.ModificationParse(xmlModification, new Modification());
    }
}
