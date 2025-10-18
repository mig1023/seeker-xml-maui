using System;

namespace SeekerMAUI.Gamebook.KnightOfTheLivingDead
{
    class Modification : Prototypes.Modification, Abstract.IModification
    {
        public override void Do()
        {
            if (Name == "MemoryA")
            {
                Character.Protagonist.Memory = "A";
                Character.Protagonist.Attack = 8;
                Character.Protagonist.Damage = 7;
                Character.Protagonist.Hitpoints = 40;
            }
            else if (Name == "MemoryB")
            {
                Character.Protagonist.Memory = "B";
                Character.Protagonist.Attack = 7;
                Character.Protagonist.Damage = 8;
                Character.Protagonist.Hitpoints = 50;
            }
            else
            {
                base.Do(Character.Protagonist);
            }
        }
    }
}
