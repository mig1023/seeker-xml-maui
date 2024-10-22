using System.Xml;
using SeekerMAUI.Game;

namespace SeekerMAUI.Gamebook.Quakers
{
    class Paragraphs : Prototypes.Paragraphs, Abstract.IParagraphs
    {
        public override Paragraph Get(int id, XmlNode xmlParagraph) =>
           base.Get(xmlParagraph, ParagraphTemplate(xmlParagraph));
    }
}