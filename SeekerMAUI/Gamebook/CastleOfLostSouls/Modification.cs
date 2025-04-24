using System;

namespace SeekerMAUI.Gamebook.CastleOfLostSouls
{
    class Modification : Prototypes.Modification, Abstract.IModification
    {
        public override void Do() =>
            base.Do(Character.Protagonist);
    }
}
