using System;

namespace SeekerMAUI.Gamebook.PensionerSimulator
{
    class Modification : Prototypes.Modification, Abstract.IModification
    {
        public override void Do() =>
            base.Do(Character.Protagonist);
    }
}
