using SeekerMAUI.Game;
using System;
using System.Xml;

namespace SeekerMAUI.Gamebook.MurderAtColefaxManor
{
    class Paragraphs : Prototypes.Paragraphs, Abstract.IParagraphs
    {
        public override Paragraph Get(int id, XmlNode xmlParagraph) =>
            base.Get(xmlParagraph);

        public override Option OptionParse(XmlNode xmlOption) =>
            OptionParseWithDo(xmlOption, new Modification());
    }
}
