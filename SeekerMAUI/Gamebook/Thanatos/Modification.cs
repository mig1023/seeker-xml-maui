using System;

namespace SeekerMAUI.Gamebook.Thanatos
{
    class Modification : Prototypes.Modification, Abstract.IModification
    {
        public override void Do()
        {
            if (Name == "RestartGame")
            {
                Game.Data.Triggers = new List<string>();
                Character.Protagonist.Cycle += 1;
            }
            else
            {
                base.Do(Character.Protagonist);
            }
        }
    }
}
