using System;

namespace SeekerMAUI.Gamebook.Trap
{
    class Modification : Prototypes.Modification, Abstract.IModification
    {
        public override void Do()
        {
            if (Name == "Equipment")
            {
                Character.Protagonist.Equipment.Add(ValueString);
            }
            else if (Name == "StrengthAndSkill")
            {
                Character.Protagonist.Strength += 2;
                Character.Protagonist.Skill += 2;
            }
            else
            {
                base.Do(Character.Protagonist);
            }
        }
    }
}
