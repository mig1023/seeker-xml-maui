using System;

namespace SeekerMAUI.Gamebook.KnightOfTheLivingDead
{
    class Modification : Prototypes.Modification, Abstract.IModification
    {
        public override void Do()
        {
            if ((Name == "Memory") && (ValueString == "A"))
            {
                Character.Protagonist.LateInit("A", 8, 7, 40);
            }
            else if (Name == "Memory")
            {
                Character.Protagonist.LateInit("B", 7, 8, 50);
            }
            else
            {
                base.Do(Character.Protagonist);
            }
        }
    }
}
