using System;

namespace SeekerMAUI.Gamebook.PensionerSimulator
{
    class Character : Prototypes.Character, Abstract.ICharacter
    {
        public static Character Protagonist { get; set; }
        public override void Set(object character) =>
            Protagonist = (Character)character;
    }
}
