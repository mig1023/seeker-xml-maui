using System;

namespace SeekerMAUI.Gamebook.LoneWolf
{
    class Modification : Prototypes.Modification, Abstract.IModification
    {
        public override void Do()
        {
            if (Name == "AvoidingCombat")
            {
                var maxWound = Character.Protagonist.Strength - 1;
                Character.Protagonist.Strength -= Game.Dice.Roll(size: maxWound);
            }
            else
            {
                base.Do(Character.Protagonist);
            }
        }
    }
}
