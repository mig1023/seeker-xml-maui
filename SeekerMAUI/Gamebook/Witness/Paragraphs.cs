using SeekerMAUI.Game;
using System.Xml;

namespace SeekerMAUI.Gamebook.Witness
{
    class Paragraphs : Prototypes.Paragraphs, Abstract.IParagraphs
    {
        public override Paragraph Get(int id, XmlNode xmlParagraph) =>
            base.Get(xmlParagraph);
    }
}
