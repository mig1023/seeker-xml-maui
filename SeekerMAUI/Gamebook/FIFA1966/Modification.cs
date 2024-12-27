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

            if (Name == "Mod")
            {
                Character.Protagonist.Vars[Path] += Value;
            }
            else if (Name == "Set")
            {
                Character.Protagonist.Vars[Path] = Value;
            }
            else if (Name == "Random")
            {
                Character.Protagonist.Vars[Path] = Game.Dice.Roll(size: Value);
            }
            else if (Name == "Enemy")
            {
                Character.Protagonist.Vars["силы соперников/сила соперника"] =
                    Character.Protagonist.Vars[$"силы соперников/{ValueString}"];
            }
        }
    }
}
