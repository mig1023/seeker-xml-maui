using System;

namespace SeekerMAUI.Gamebook.WrongWayGoBack
{
    class Modification : Prototypes.Modification, Abstract.IModification
    {
        public override void Do() =>
            base.Do(Character.Protagonist);
    }
}
