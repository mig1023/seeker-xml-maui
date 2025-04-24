using System;

namespace SeekerMAUI.Gamebook.CastleOfLostSouls
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public List<Character> Enemies { get; set; }

        public override List<string> Status() => new List<string>
        {
            $"Честь: {Character.Protagonist.Honor}",
            $"Доспехи: {Character.Protagonist.Armor}",
            $"Золото: {Character.Protagonist.Gold}",
        };

        public override List<string> AdditionalStatus() => new List<string>
        {
            $"Боевая доблесть: {Character.Protagonist.Combat}",
            $"Телосложение: {Character.Protagonist.Constitution}",
            $"Сообразительность: {Character.Protagonist.Ingenuity}",
            $"Магическая стойкость: {Character.Protagonist.Resistence}",
        };
    }
}
