using System;

namespace SeekerMAUI.Gamebook.NoYourGrace
{
    class Modification : Prototypes.Modification, Abstract.IModification
    {
        public override void Do()
        {
            return base.Do(Character.Protagonist);
        }
    }
}
