using System.Xml;
using SeekerMAUI.Game;
using SeekerMAUI.Output;

namespace SeekerMAUI.Gamebook.Usurper
{
    class Paragraphs : Prototypes.Paragraphs, Abstract.IParagraphs
    {
        public override Paragraph Get(int id, XmlNode xmlParagraph) =>
            base.Get(xmlParagraph);

        public override List<Text> TextsParse(XmlNode xmlNode, bool main = false)
        {
            if (xmlNode["Text"] != null)
            {
                return new List<Text> { Xml.TextLineParse(xmlNode["Text"]) };
            }
            else
            {
                List<Text> texts = new List<Text>();

                foreach (XmlNode text in xmlNode.SelectNodes("Texts/Text"))
                {
                    string option = text.Attributes["Availability"]?.Value ?? String.Empty;

                    if (String.IsNullOrEmpty(option) || Data.Actions.Availability(option))
                        texts.Add(Xml.TextLineParse(text));
                }

                return texts;
            }
        }

        public override Option OptionParse(XmlNode xmlOption)
        {
            List<Abstract.IModification> modifications = new List<Abstract.IModification>();

            foreach (XmlNode optionMod in xmlOption.SelectNodes("*"))
            {
                if (!optionMod.Name.StartsWith("Text"))
                    modifications.Add(new Modification());
            }

            return OptionParseWithDo(xmlOption, modifications);
        }

        public override Abstract.IModification ModificationParse(XmlNode xmlModification) =>
            Xml.ModificationParse(xmlModification, new Modification());
    }
}
