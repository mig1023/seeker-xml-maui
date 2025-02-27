using System;

namespace SeekerMAUI.Gamebook.MurderAtColefaxManor
{
    class Modification : Prototypes.Modification, Abstract.IModification
    {
        public override void Do() =>
            base.Do(new Character());
    }
}
