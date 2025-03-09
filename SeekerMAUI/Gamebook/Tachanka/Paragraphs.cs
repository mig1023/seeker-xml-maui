using SeekerMAUI.Game;
using System;
using System.Xml;

namespace SeekerMAUI.Gamebook.Tachanka
{
    class Paragraphs : Prototypes.Paragraphs, Abstract.IParagraphs
    {
        public override Paragraph Get(int id, XmlNode xmlParagraph) =>
            base.Get(xmlParagraph);

        public override Abstract.IActions ActionParse(XmlNode xmlAction)
        {
            Actions action = (Actions)base.ActionParse(xmlAction, new Actions(),
                GetProperties(new Actions()), new Modification());

            if (action.Type == "Option")
                action.Option = OptionParse(xmlAction);

            action.Benefit = Xml.ModificationParse(xmlAction["Benefit"], new Modification());

            return action;
        }

        public override Option OptionParse(XmlNode xmlOption)
        {
            Option option = OptionsTemplateWithoutGoto(xmlOption);
            option.Aftertexts.Clear();

            if (ThisIsGameover(xmlOption))
            {
                option.Goto = GetGoto(xmlOption);
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

            XmlNodeList optionMods = xmlOption.SelectNodes("*");

            foreach (XmlNode optionMod in optionMods)
            {
                if (optionMod == null)
                {
                    continue;
                }
                else if (optionMod.Name == "Text")
                {
                    option.Aftertexts.Add(Xml.TextLineParse(optionMod));
                }
                else if (optionMod.Name == "Image")
                {
                    option.Aftertexts.Add(Xml.ImageLineParse(optionMod));
                }
                else if (optionMod.Attributes["Value"] != null)
                {
                    option.Do.Add(ModificationParse(optionMod));
                }
            }

            return option;
        }

        public override Abstract.IModification ModificationParse(XmlNode xmlModification) =>
           (Abstract.IModification)base.ModificationParse(xmlModification, new Modification());
    }
}
