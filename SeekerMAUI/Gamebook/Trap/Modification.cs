using System;

namespace SeekerMAUI.Gamebook.Trap
{
    class Modification : Prototypes.Modification, Abstract.IModification
    {
        public override void Do()
        {
            if (Name == "Equipment")
            {
                Character.Protagonist.Equipment.Add(ValueString);
            }
            else
            {
                base.Do(Character.Protagonist);
            }
        }
    }
}
