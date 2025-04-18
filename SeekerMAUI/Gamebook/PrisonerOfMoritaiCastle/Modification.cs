﻿using System;

namespace SeekerMAUI.Gamebook.PrisonerOfMoritaiCastle
{
    class Modification : Prototypes.Modification, Abstract.IModification
    {
        public override void Do()
        {
            if (Name == "Signal")
            {
                if (Game.Option.IsTriggered("сигнал"))
                    Game.Option.Trigger("пожар");
            }
            else if (Name == "CasuisticWound")
            {
                if (Character.Protagonist.Hitpoints > 3)
                    Character.Protagonist.Hitpoints -= 1;
            }
            else if (Name == "ColdMedicine")
            {
                if (Game.Option.IsTriggered("противопростудное"))
                    Game.Option.Trigger("противопростудное", remove: true);
                else
                    Character.Protagonist.Hitpoints -= 1;
            }
            else
            {
                base.Do(Character.Protagonist);
            }
        }
    }
}
