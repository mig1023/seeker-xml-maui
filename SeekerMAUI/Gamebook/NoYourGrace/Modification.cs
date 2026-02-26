using System;

namespace SeekerMAUI.Gamebook.NoYourGrace
{
    class Modification : Prototypes.Modification, Abstract.IModification
    {
        public override void Do()
        {
            if (Name == "Set")
            {
                var values = ValueString
                    .Split(',')
                    .Select(x => x.Trim())
                    .ToList();

                var property = values[0];
                var value = int.Parse(values[1]);

                SetProperty(Character.Protagonist, property, value);
            }
            else if (Constants.Availabilities.ContainsKey(Name))
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
