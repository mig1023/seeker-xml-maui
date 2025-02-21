using System;

namespace SeekerMAUI.Gamebook.Tachanka
{
    class Modification : Prototypes.Modification, Abstract.IModification
    {
        public bool AddWithReplace { get; set; }

        public override void Do()
        {
            if (Name == "Crew")
            {
                if (Character.Protagonist.Team.Count >= 3)
                {
                    if (AddWithReplace)
                        Character.Protagonist.Team[1] = Character.Protagonist.Team[2];

                    Character.Protagonist.Team.RemoveAt(2);
                }

                var crew = ValueString.Split(',');
                Character.Protagonist.Team.Add(new Crew(crew[0], crew[1]));
            }
            else if (Name == "Lost")
            {
                if (ValueString == "All")
                {
                    Character.Protagonist.Team = new List<Crew>();
                    return;
                }

                foreach (var name in ValueString.Split(','))
                {
                    var not = name.StartsWith("!");

                    for (var i = 0; i < Character.Protagonist.Team.Count; i++)
                    {
                        var crew = Character.Protagonist.Team[i];
                        if (!name.Contains(crew.Name) && not)
                            Character.Protagonist.Team.RemoveAt(i);

                        if (name.Contains(crew.Name) && !not)
                            Character.Protagonist.Team.Remove(crew);
                    }
                }
            }
            else
            {
                base.Do(Character.Protagonist);
            }
        }
    }
}
