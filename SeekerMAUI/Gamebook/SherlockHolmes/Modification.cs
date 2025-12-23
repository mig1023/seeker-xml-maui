using System;

namespace SeekerMAUI.Gamebook.SherlockHolmes
{
    class Modification : Prototypes.Modification, Abstract.IModification
    {
        public override void Do()
        {
            if (Name == "ReGameCleaning")
            {
                Character.Protagonist.EvidenceCount = 0;
                Game.Data.Clean(reStart: true);
            }
            else if (Name == "TimeCleaning")
            {
                Character.Protagonist.Time = -1;
            }
            else
            {
                base.Do(Character.Protagonist);
            }
        }
    }
}
