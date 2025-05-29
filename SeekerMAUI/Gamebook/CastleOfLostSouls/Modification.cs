using System;

namespace SeekerMAUI.Gamebook.CastleOfLostSouls
{
    class Modification : Prototypes.Modification, Abstract.IModification
    {
        public override void Do()
        {
            if (Name == "Salakar")
            {
                Character.Protagonist.Combat = 8;
                Character.Protagonist.Constitution = 11;
                Character.Protagonist.Ingenuity = 7;
                Character.Protagonist.Resistence = 5;
                Character.Protagonist.Honor = 0;
                Character.Protagonist.Armour = 2;
                Character.Protagonist.Gold = 12;
            }
            else
            {
                base.Do(Character.Protagonist);
            }
        }
    }
}
