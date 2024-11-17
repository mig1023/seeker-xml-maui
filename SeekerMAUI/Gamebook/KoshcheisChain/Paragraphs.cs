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

            var enemy = xmlAction.SelectSingleNode("Enemy");
            action.EnemyName = Xml.StringParse(enemy.Attributes["Name"]);

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

            if (action.Type == "Option")
                action.Option = OptionParse(xmlAction);

            return action;
        }
    }
}
