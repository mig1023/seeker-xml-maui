using System.Xml;
using SeekerMAUI.Game;

namespace SeekerMAUI.Gamebook.MasterOfTaiga
{
    class Paragraphs : Prototypes.Paragraphs, Abstract.IParagraphs
    {
        public override Paragraph Get(int id, XmlNode xmlParagraph) =>
            base.Get(xmlParagraph);
    }
}
