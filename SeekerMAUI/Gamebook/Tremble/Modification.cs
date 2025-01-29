using System;

namespace SeekerMAUI.Gamebook.Tremble
{
    class Modification : Prototypes.Modification, Abstract.IModification
    {
        public override void Do()
        {
            if (Name == "WayBack")
            {
                Character.Protagonist.WayBack = Value;
            }
            else if (Name == "Keys")
            {
                if (String.IsNullOrEmpty(Character.Protagonist.Keys))
                {
                    Character.Protagonist.Keys = ValueString;
                }
                else
                {
                    Character.Protagonist.Keys += $" {ValueString}";
                }
            }
            else
            {
                base.Do(Character.Protagonist);
            }
        }
    }
}
