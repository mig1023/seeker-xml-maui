using SeekerMAUI.Game;
using System.Xml;

namespace SeekerMAUI.Gamebook.Tremble
{
    class Paragraphs : Prototypes.Paragraphs, Abstract.IParagraphs
    {
        public override Paragraph Get(int id, XmlNode xmlParagraph)
        {
            Paragraph paragraph = ParagraphTemplate(xmlParagraph);

            foreach (XmlNode xmlOption in xmlParagraph.SelectNodes("Options/*"))
            {
                Option option = OptionsTemplateWithoutGoto(xmlOption);

                if (ThisIsGameover(xmlOption) || ThisIsBack(xmlOption))
                {
                    option.Goto = GetGoto(xmlOption, wayBack: Character.Protagonist.WayBack);
                }
                else if (int.TryParse(xmlOption.Attributes["Goto"].Value, out int _))
                {
                    option.Goto = Xml.IntParse(xmlOption.Attributes["Goto"]);
                }
                else
                {
                    List<string> link = xmlOption.Attributes["Goto"].Value.Split(',').ToList<string>();
                    option.Goto = int.Parse(link[random.Next(link.Count())]);
                }

                XmlNode optionMod = xmlOption.SelectSingleNode("*");

                if ((optionMod != null) && (optionMod["Value"] != null))
                    option.Do.Add(Xml.ModificationParse(optionMod, new Modification()));

                paragraph.Options.Add(option);
            }

            foreach (XmlNode xmlAction in xmlParagraph.SelectNodes("Actions/*"))
                paragraph.Actions.Add(ActionParse(xmlAction));

            foreach (XmlNode xmlModification in xmlParagraph.SelectNodes("Modifications/*"))
                paragraph.Modification.Add(ModificationParse(xmlModification));

            return paragraph;
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

            return action;
        }

        public override Abstract.IModification ModificationParse(XmlNode xmlModification) =>
            (Abstract.IModification)base.ModificationParse(xmlModification, new Modification());

        public override Option OptionParse(XmlNode xmlOption) =>
            OptionParseWithDo(xmlOption, new Modification());

        private Character EnemyParse(XmlNode xmlEnemy)
        {
            var enemy = new Character();

            foreach (string param in GetProperties(enemy))
                SetPropertyByAttr(enemy, param, xmlEnemy, maxPrefix: true);

            enemy.Skill = enemy.MaxSkill;
            enemy.Endurance = enemy.MaxEndurance;

            return enemy;
        }
    }
}
