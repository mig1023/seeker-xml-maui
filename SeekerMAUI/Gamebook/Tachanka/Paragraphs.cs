using SeekerMAUI.Game;
using System;
using System.Xml;

namespace SeekerMAUI.Gamebook.Tachanka
{
    class Paragraphs : Prototypes.Paragraphs, Abstract.IParagraphs
    {
        public override Paragraph Get(int id, XmlNode xmlParagraph)
        {
            Paragraph paragraph = ParagraphTemplate(xmlParagraph);

            foreach (XmlNode xmlOption in xmlParagraph.SelectNodes("Options/*"))
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
            Actions action = (Actions)base.ActionParse(xmlAction, new Actions(),
                GetProperties(new Actions()), new Modification());

            if (action.Type == "Option")
                action.Option = OptionParse(xmlAction);

            return action;
        }

        public override Abstract.IModification ModificationParse(XmlNode xmlModification) =>
           (Abstract.IModification)base.ModificationParse(xmlModification, new Modification());
    }
}
