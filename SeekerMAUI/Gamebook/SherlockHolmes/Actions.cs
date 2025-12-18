using System;

namespace SeekerMAUI.Gamebook.SherlockHolmes
{
    class Actions : Prototypes.Actions, Abstract.IActions
    {
        public string Stat { get; set; }

        public override List<string> AdditionalStatus() => new List<string>
        {
            $"Ловкость: {Character.Protagonist.Dexterity}",
            $"Изобретательность: {Character.Protagonist.Ingenuity}",
            $"Интуиция: {Character.Protagonist.Intuition}",
            $"Красноречие: {Character.Protagonist.Eloquence}",
            $"Наблюдательность: {Character.Protagonist.Observation}",
            $"Эрудиция: {Character.Protagonist.Erudition}",
        };

        public List<string> Get() =>
            ChangeProtagonistParam(Stat, Character.Protagonist, "StatBonuses");

        public List<string> Decrease() =>
            ChangeProtagonistParam(Stat, Character.Protagonist, "StatBonuses", decrease: true);
    }
}
