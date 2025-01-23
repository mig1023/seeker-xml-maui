using System;

namespace SeekerMAUI.Gamebook.Tank
{
    class Modification : Prototypes.Modification, Abstract.IModification
    {
        public override void Do()
        {
            if (Name == "NewTankCrew")
            {
                Character.Protagonist.Driver = 4;
                Character.Protagonist.Shooter = 2;
                Character.Protagonist.Gunner = 3;

                Character.Protagonist.Dead = 0;
                Character.Protagonist.Immobilized = 0;

                Game.Data.Triggers.Clear();
            }
            else
            {
                base.Do(Character.Protagonist);
            }
        }
    }
}
