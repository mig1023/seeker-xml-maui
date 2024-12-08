using System;

namespace SeekerMAUI.Gamebook.FIFA1966
{
    class Modification : Prototypes.Modification, Abstract.IModification
    {
        public string Path { get; set; }

        public override void Do()
        {
            if (!Character.Protagonist.Vars.ContainsKey(Path))
            {
                Character.Protagonist.Vars.Add(Path, 0);
            }

            if (Name == "Add")
            {
                Character.Protagonist.Vars[Path] += Value;
            }
            else if (Name == "Sub")
            {
                Character.Protagonist.Vars[Path] -= Value;
            }
            else if (Name == "Set")
            {
                Character.Protagonist.Vars[Path] = Value;
            }
            else if (Name == "Random")
            {
                Character.Protagonist.Vars[Path] = Game.Dice.Roll(size: Value);
            }
        }
    }
}
