using System;

namespace SeekerMAUI.Gamebook.BehindTheThrone
{
    class Modification : Prototypes.Modification, Abstract.IModification
    {
        public override void Do() =>
            base.Do(Character.Protagonist);
    }
}
