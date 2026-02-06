using System;

namespace SeekerMAUI.Gamebook.NoYourGrace
{
    class Modification : Prototypes.Modification, Abstract.IModification
    {
        public override void Do()
        {
            if (Constants.Availabilities.ContainsKey(Name))
            {
                int currentValue = GetProperty(Character.Protagonist, Constants.Availabilities[Name]);
                SetProperty(Character.Protagonist, Constants.Availabilities[Name], currentValue + Value);
            }
            else
            {
                base.Do(Character.Protagonist);
            }
        }
    }
}
