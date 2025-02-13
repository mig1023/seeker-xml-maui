using System.Xml;
using SeekerMAUI.Output;
using SeekerMAUI.Game;

namespace SeekerMAUI.Gamebook.FIFA1966
{
    class Paragraphs : Prototypes.Paragraphs, Abstract.IParagraphs
    {
        public override Paragraph Get(int id, XmlNode xmlParagraph)
        {
            Paragraph paragraph = new Paragraph
            {
                Options = new List<Option>(),
                Modification = new List<Abstract.IModification>(),
            };

            foreach (XmlNode xmlOption in xmlParagraph.SelectNodes("Options/*"))
                paragraph.Options.Add(OptionParse(xmlOption));

            foreach (XmlNode xmlModification in xmlParagraph.SelectNodes("Modifications/*"))
                ModificationParse(xmlModification).Do();

            paragraph.Texts = TextsParse(xmlParagraph, main: true);

            return paragraph;
        }

        public override List<Text> TextsParse(XmlNode xmlNode, bool main = false)
        {
            if (xmlNode["Text"] != null)
            {
                return new List<Text> { Xml.TextLineParse(xmlNode["Text"]) };
            }
            else
            {
                List<Text> texts = new List<Text>();

                foreach (XmlNode text in xmlNode.SelectNodes("Texts/*"))
                {
                    string option = text.Attributes["Availability"]?.Value ?? String.Empty;

                    if (!String.IsNullOrEmpty(option) && !Data.Actions.Availability(option))
                        continue;

                    if (text.Name == "Text")
                    {
                        texts.Add(Xml.TextLineParse(text));
                    }
                    else if (text.Name == "Image")
                    {
                        texts.Add(Xml.ImageLineParse(text));
                    }
                }

                return texts;
            }
        }

        public override Option OptionParse(XmlNode xmlOption)
        {
            Option option = OptionsTemplate(xmlOption);

            foreach (XmlNode optionMod in xmlOption.SelectNodes("*"))
            {
                if (!optionMod.Name.StartsWith("Text"))
                    option.Do.Add(ModificationParse(optionMod));
            }

            return option;
        }

        public override Abstract.IModification ModificationParse(XmlNode xmlModification)
        {
            var modifaction = (Modification)base.ModificationParse(xmlModification, new Modification());

            modifaction.Path = Xml.StringParse(xmlModification.Attributes["Path"]);

            return modifaction;
        }
           
    }
}
