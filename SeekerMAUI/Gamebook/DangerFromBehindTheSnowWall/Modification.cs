using System;

namespace SeekerMAUI.Gamebook.DangerFromBehindTheSnowWall
{
    class Modification : Prototypes.Modification, Abstract.IModification
    {
        public override void Do() =>
            base.Do(Character.Protagonist);
    }
}
