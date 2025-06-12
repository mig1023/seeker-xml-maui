using SeekerMAUI.Game;
using System.Xml;

namespace SeekerMAUI.Gamebook.StarshipTraveller
{
    class Paragraphs : Prototypes.Paragraphs, Abstract.IParagraphs
    {
        public override Paragraph Get(int id, XmlNode xmlParagraph) =>
            base.Get(xmlParagraph);

        public override Option OptionParse(XmlNode xmlOption) =>
            OptionParseWithDo(xmlOption, new Modification());

        public override Abstract.IActions ActionParse(XmlNode xmlAction)
        {
            Actions action = (Actions)ActionTemplate(xmlAction, new Actions());

            foreach (string param in GetProperties(action))
                SetProperty(action, param, xmlAction);

            if (!string.IsNullOrEmpty(xmlAction.Attributes["Crew"].InnerText))
            {
                action.Crew = xmlAction.Attributes["Crew"].InnerText;
                action.Max = int.Parse(xmlAction.Attributes["Max"].InnerText);
                action.Button = Constants.FullNames[action.Crew];
            }

            if (xmlAction["Enemy"] != null)
            {
                action.Enemies = new List<Character> { ParseEnemy(xmlAction["Enemy"]) };
            }
            else if (xmlAction["Enemies"] != null)
            {
                action.Enemies = new List<Character>();

                foreach (XmlNode xmlEnemy in xmlAction.SelectNodes("Enemies/Enemy"))
                    action.Enemies.Add(ParseEnemy(xmlEnemy));
            }

            if (action.Type == "Option")
                action.Option = OptionParse(xmlAction);

            return action;
        }

        public override Abstract.IModification ModificationParse(XmlNode xmlModification) =>
            Xml.ModificationParse(xmlModification, new Modification());

        private Character ParseEnemy(XmlNode xmlEnemy)
        {
            Character enemy = new Character();

            foreach (string param in GetProperties(enemy))
                SetPropertyByAttr(enemy, param, xmlEnemy, maxPrefix: true);

            enemy.Shields = enemy.MaxShields;
            enemy.Stamina = enemy.MaxStamina;

            return enemy;
        }
    }
}
