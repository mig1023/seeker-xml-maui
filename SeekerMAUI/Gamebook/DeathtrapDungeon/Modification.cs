using System;

namespace SeekerMAUI.Gamebook.DeathtrapDungeon
{
    class Modification : Prototypes.Modification, Abstract.IModification
    {
        public override void Do() =>
            base.Do(Character.Protagonist);
    }
}
