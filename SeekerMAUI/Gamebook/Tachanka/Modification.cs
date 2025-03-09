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
                else if (ValueString == "Last")
                {
                    if (Character.Protagonist.Team.Count > 0)
                        Character.Protagonist.Team.RemoveAt(Character.Protagonist.Team.Count - 1);

                    return;
                }

                var except = ValueString.Contains("!");

                for (var i = 0; i < Character.Protagonist.Team.Count; i++)
                {
                    var crew = Character.Protagonist.Team[i];

                    var inList = ValueString
                        .Split(',')
                        .Where(x => x.Replace("!", String.Empty) == crew.Name)
                        .Count() > 0;

                    if (!inList && except)
                        Character.Protagonist.Team.RemoveAt(i);

                    if (inList && !except)
                        Character.Protagonist.Team.RemoveAt(i);
                }
            }
            else if (Name == "Repair")
            {
                if (Character.Protagonist.Wheels < 5)
                {
                    Character.Protagonist.Wheels += 1;
                }
                else if (Character.Protagonist.Carriage < 5)
                {
                    Character.Protagonist.Carriage += 1;
                }
                else if (Character.Protagonist.Harness < 5)
                {
                    Character.Protagonist.Harness += 1;
                }
                else if (Character.Protagonist.Springs < 5)
                {
                    Character.Protagonist.Springs += 1;
                }
                else
                {
                    Character.Protagonist.Money += 1;
                }
            }
            else
            {
                base.Do(Character.Protagonist);
            }
        }
    }
}
