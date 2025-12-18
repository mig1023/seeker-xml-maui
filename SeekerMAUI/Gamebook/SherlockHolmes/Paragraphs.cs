using SeekerMAUI.Game;
using System;
using System.Xml;

namespace SeekerMAUI.Gamebook.SherlockHolmes
{
    class Paragraphs : Prototypes.Paragraphs, Abstract.IParagraphs
    {
        public override Paragraph Get(int id, XmlNode xmlParagraph) =>
            base.Get(xmlParagraph);

        public override Abstract.IActions ActionParse(XmlNode xmlAction) =>
            (Actions)ActionTemplate(xmlAction, new Actions());

        public override Option OptionParse(XmlNode xmlOption)
        {
            Option option = OptionsTemplate(xmlOption);

            if (Constants.Buttons.ContainsKey(option.Goto))
                option.Style = Constants.Buttons[option.Goto];

            return option;
        }

        public override Abstract.IModification ModificationParse(XmlNode xmlModification) =>
           (Abstract.IModification)base.ModificationParse(xmlModification, new Modification());
    }
}
