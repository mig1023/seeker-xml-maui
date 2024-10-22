using System.Xml;
using SeekerMAUI.Game;

namespace SeekerMAUI.Gamebook.PensionerSimulator
{
    class Paragraphs : Prototypes.Paragraphs, Abstract.IParagraphs
    {
        public override Paragraph Get(int id, XmlNode xmlParagraph)
        {
            Paragraph paragraph = ParagraphTemplate(xmlParagraph);

            foreach (XmlNode xmlOption in xmlParagraph.SelectNodes("Options/*"))
                paragraph.Options.Add(OptionParseWithDo(xmlOption, new Modification()));

            return paragraph;
        }
    }
}
