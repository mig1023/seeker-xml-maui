using System.Xml;
using SeekerMAUI.Game;

namespace SeekerMAUI.Gamebook.Trap
{
    class Paragraphs : Prototypes.Paragraphs, Abstract.IParagraphs
    {
        public override Paragraph Get(int id, XmlNode xmlParagraph) =>
           base.Get(xmlParagraph, ParagraphTemplate(xmlParagraph));

        public override Abstract.IActions ActionParse(XmlNode xmlAction)
        {
            var action = (Actions)ActionTemplate(xmlAction, new Actions());
            action.Stat = Xml.StringParse(xmlAction["Stat"]);

            if (action.Type == "Option")
            {
                action.Option = OptionParse(xmlAction);
                return action;
            }

            if (action.Type != "Fight")
            {
                return action;
            }

            var enemy = xmlAction.SelectSingleNode("Enemy");
            action.EnemyName = Xml.StringParse(enemy.Attributes["Name"]);
            action.EnemyAttack = Xml.IntParse(enemy.Attributes["Attack"]);

            return action;
        }

        public override Option OptionParse(XmlNode xmlOption) =>
            OptionParseWithDo(xmlOption, new Modification());

        public override Abstract.IModification ModificationParse(XmlNode xmlModification) =>
            Xml.ModificationParse(xmlModification, new Modification());
    }
}