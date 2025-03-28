﻿using System.Xml;
using SeekerMAUI.Game;

namespace SeekerMAUI.Gamebook.RockOfTerror
{
    class Paragraphs : Prototypes.Paragraphs, Abstract.IParagraphs
    {
        public override Paragraph Get(int id, XmlNode xmlParagraph) =>
            base.Get(xmlParagraph);

        public override Abstract.IActions ActionParse(XmlNode xmlAction) =>
            base.ActionParse(xmlAction, new Actions(), GetProperties(new Actions()), new Modification());

        public override Abstract.IModification ModificationParse(XmlNode xmlModification) => new Modification
        {
            Name = xmlModification.Name,
            Value = Xml.IntParse(xmlModification.Attributes["Value"]),
            Init = Xml.BoolParse(xmlModification.Attributes["Init"]),
        };
    }
}
