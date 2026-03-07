using System;

namespace SeekerMAUI.Gamebook.NoYourGrace
{
    class Modification : Prototypes.Modification, Abstract.IModification
    {
        public string Condition { get; set; }

        public override void Do()
        {
            if (Name == "Set")
            {
                var values = ValueString
                    .Split('=')
                    .Select(x => x.Trim())
                    .ToList();

                var property = values[0];
                var value = 0;
                    
                if (values[1] == "Dice")
                {
                    value = Game.Dice.Roll();
                }
                else
                {
                    value = int.Parse(values[1]);
                }

                SetProperty(Character.Protagonist, property, value);
            }
            else if (Name == "ByCondition")
            {
                var isActual = new Actions().Availability(Condition);

                if (!isActual)
                    return;

                var modifications = ValueString
                    .Split(';')
                    .Select(x => x.Trim())
                    .ToList();

                foreach (var modification in modifications)
                {
                    var recursionMod = new Modification();

                    if (modification.Contains("="))
                    {
                        recursionMod.Name = "Set";
                        recursionMod.ValueString = modification;
                    }
                    else
                    {
                        var values = modification
                           .Split(',')
                           .Select(x => x.Trim())
                           .ToList();

                        recursionMod.Name = values[0];

                        if (values[0].EndsWith("rigger"))
                            recursionMod.ValueString = values[1];
                        else
                            recursionMod.Value = int.Parse(values[1]);
                    }

                    recursionMod.Do();
                }
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
