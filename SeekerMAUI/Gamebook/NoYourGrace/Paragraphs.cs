using SeekerMAUI.Game;
using System.Xml;

namespace SeekerMAUI.Gamebook.NoYourGrace
{
    class Paragraphs : Prototypes.Paragraphs, Abstract.IParagraphs
    {
        public override Paragraph Get(int id, XmlNode xmlParagraph) =>
            base.Get(xmlParagraph);

        public override Abstract.IModification ModificationParse(XmlNode xmlModification) =>
           (Abstract.IModification)base.ModificationParse(xmlModification, new Modification());

        public override Option OptionParse(XmlNode xmlOption)
        {
            Option option = OptionsTemplate(xmlOption);

            int modIndex = 0;

            foreach (XmlNode optionMod in xmlOption.SelectNodes("*"))
            {
                if (optionMod.Name.StartsWith("Text"))
                    continue;

                option.Do.Add((Abstract.IModification)base.ModificationParse(optionMod, new Modification()));
                modIndex += 1;
            }

            return option;
        }

        public override Abstract.IActions ActionParse(XmlNode xmlAction) =>
            ActionTemplate(xmlAction, new Actions());
    }
}
