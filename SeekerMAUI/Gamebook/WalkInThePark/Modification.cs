﻿using System;

namespace SeekerMAUI.Gamebook.WalkInThePark
{
    class Modification : Prototypes.Modification, Abstract.IModification
    {
        public override void Do()
        {
            if (Name == "HalfMoney")
            {
                Character.Protagonist.Money /= 2;
            }
            else if (Name == "StartStrength")
            {
                Character.Protagonist.Strength = Character.Protagonist.StartStrength;
            }
            else if (Name == "Weapon")
            {
                Character.Protagonist.Weapon = ValueString;
            }
            else if (Name == "Damage")
            {
                Character.Protagonist.Damage = Value;
            }
            else if (Name == "MapsToRating")
            {
                Character.Protagonist.Rating += Character.Protagonist.MapParts * 10;
            }
            else if (Name == "BeerUsing")
            {
                foreach (string trigger in Game.Data.Triggers)
                {
                    if (!trigger.StartsWith("пиво"))
                        continue;

                    Game.Option.Trigger(trigger, remove: true);
                    return;
                }
            }
            else
            {
                base.Do(Character.Protagonist);
            }
        }
    }
}
