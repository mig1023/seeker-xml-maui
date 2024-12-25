using System;

namespace SeekerMAUI.Gamebook.Tremble
{
    class Modification : Prototypes.Modification, Abstract.IModification
    {
        public override void Do()
        {
            if (Name == "Keys")
            {
                Character.Protagonist.Keys += $" {ValueString}";
            }
            else
            {
                base.Do(Character.Protagonist);
            }
        }
    }
}
