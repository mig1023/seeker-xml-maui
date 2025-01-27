using SeekerMAUI.Game;
using System;
using System.Xml;

namespace SeekerMAUI.Gamebook.LoneWolf
{
    class Paragraphs : Prototypes.Paragraphs, Abstract.IParagraphs
    {
        public override Paragraph Get(int id, XmlNode xmlParagraph)
        {
            var regeneration = Game.Option.IsTriggered("Регенерация");
            var injured = Character.Protagonist.Strength < Character.Protagonist.MaxStrength;

            if (regeneration && injured)
            {
                Character.Protagonist.Strength += 1;
            }

            return base.Get(xmlParagraph, ParagraphTemplate(xmlParagraph));
        }
          
        public override Abstract.IActions ActionParse(XmlNode xmlAction)
        {
            Actions action = (Actions)ActionTemplate(xmlAction, new Actions());

            foreach (string param in GetProperties(action))
                SetProperty(action, param, xmlAction);

            if (xmlAction["Enemy"] != null)
            {
                action.Enemy = EnemyParse(xmlAction["Enemy"]);
            }

            if (action.Type == "Option")
                action.Option = OptionParse(xmlAction);

            action.Benefit = Xml.ModificationParse(xmlAction["Benefit"], new Modification());

            return action;
        }

        public override Abstract.IModification ModificationParse(XmlNode xmlModification) =>
            Xml.ModificationParse(xmlModification, new Modification());

        private Character EnemyParse(XmlNode xmlEnemy)
        {
            Character enemy = new Character();

            foreach (string param in GetProperties(enemy))
                SetPropertyByAttr(enemy, param, xmlEnemy, maxPrefix: true);

            enemy.Strength = enemy.MaxStrength;

            return enemy;
        }
    }
}
