using System;

namespace SeekerMAUI.Gamebook.Usurper
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public override List<string> Status() => new List<string>
        {
            $"Влияние: {Character.Protagonist.Influence}",
            $"Здоровье: {Character.Protagonist.Health}",
            $"Лояльность: {Character.Protagonist.Loyalty}",
        };
    }
}
