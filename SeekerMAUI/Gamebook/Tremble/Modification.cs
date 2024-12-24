using System;

namespace SeekerMAUI.Gamebook.Tremble
{
    class Modification : Prototypes.Modification, Abstract.IModification
    {
        public override void Do() =>
            base.Do(Character.Protagonist);
    }
}
