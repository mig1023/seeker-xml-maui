using System.Xml;
using Microsoft.Maui.Platform;
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

            if (action.Type != "Fight")
            {
                return action;
            }

            var enemy = xmlAction.SelectSingleNode("Enemy");
            action.EnemyName = Xml.StringParse(enemy.Attributes["Name"]);
            action.ByExtrasensory = Xml.BoolParse(xmlAction["ByExtrasensory"]);
            action.RingEffect = Xml.BoolParse(xmlAction["RingEffect"]);

            var strength = enemy.Attributes["Strength"].InnerText;

            if (strength == "mirror")
            {
                action.EnemyStrength = Character.Protagonist.Strength;
            }
            else
            {
                action.EnemyStrength = Xml.IntParse(strength);
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
    }
}
