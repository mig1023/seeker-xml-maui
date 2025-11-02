using System;

namespace SeekerMAUI.Gamebook.Thanatos
{
    class Modification : Prototypes.Modification, Abstract.IModification
    {
        public override void Do()
        {
            if (Name == "RestartGame")
            {
                Game.Data.Triggers = Game.Data.Triggers
                    .Where(x => x.StartsWith("КОНЦОВКА"))
                    .ToList();

                Character.Protagonist.Cycle += 1;
            }
            else if(Name == "KeyOfDestiny")
            {
                var endsCount = Constants.EndPoints
                    .Where(x => Game.Option.IsTriggered(x))
                    .Count();

                if (endsCount >= 19)
                    Game.Option.Trigger("Ключ Судьбы");
            }
            else
            {
                base.Do(Character.Protagonist);
            }
        }
    }
}
