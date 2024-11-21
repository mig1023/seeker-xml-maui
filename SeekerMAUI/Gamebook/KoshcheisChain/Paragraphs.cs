using System.Xml;
using SeekerMAUI.Game;

namespace SeekerMAUI.Gamebook.KoshcheisChain
{
    class Paragraphs : Prototypes.Paragraphs, Abstract.IParagraphs
    {
        public override Paragraph Get(int id, XmlNode xmlParagraph) =>
            base.Get(xmlParagraph);

        public override Abstract.IActions ActionParse(XmlNode xmlAction)
        {
            var action = (Actions)ActionTemplate(xmlAction, new Actions());

            if (action.Type == "Option")
            {
                action.Option = OptionParse(xmlAction);
                return action;
            }

            action.ByExtrasensory = Xml.BoolParse(xmlAction["ByExtrasensory"]);
            action.RingEffect = Xml.BoolParse(xmlAction["RingEffect"]);

            if (action.Type != "Fight")
            {
                return action;
            }

            action.Forward = Xml.BoolParse(xmlAction["Forward"]);

            if (!action.Forward)
            {
                var enemy = xmlAction.SelectSingleNode("Enemy");
                action.EnemyName = Xml.StringParse(enemy.Attributes["Name"]);
                action.StrengthLimit = Xml.IntParse(xmlAction["StrengthLimit"]);

                var strength = enemy.Attributes["Strength"]?.InnerText ?? String.Empty;

                action.EnemyStrength = strength == "mirror" ?
                    Character.Protagonist.Strength : Xml.IntParse(strength);
            }

            action.Fights = new List<Fight>();

            foreach (XmlNode xmlDice in xmlAction.SelectNodes("Dice"))
            {
                var fight = new Fight();

                foreach (string param in GetProperties(fight))
                    SetPropertyByAttr(fight, param, xmlDice);

                action.Fights.Add(fight);
            }

            return action;
        }

        public override Option OptionParse(XmlNode xmlOption) =>
            OptionParseWithDo(xmlOption, new Modification());

        public override Abstract.IModification ModificationParse(XmlNode xmlModification) =>
            Xml.ModificationParse(xmlModification, new Modification());
    }
}
