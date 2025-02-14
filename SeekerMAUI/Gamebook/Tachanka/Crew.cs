using System;

namespace SeekerMAUI.Gamebook.Tachanka
{
    class Crew
    {
        public string Name { get; set; }

        public string Skill { get; set; }

        public bool Wounded { get; set; }

        public Crew(string name, string skill)
        {
            this.Name = name;
            this.Skill = skill;
            this.Wounded = false;
        }

        public string Serialize()
        {
            string wounded = this.Wounded ? "1" : "0";
            return $"{this.Name}^{this.Skill}^{wounded}";
        }
        
        public static Crew Deserialize(string line)
        {
            string[] data = line.Split('^');

            var crew = new Crew(data[0], data[1]);
            crew.Wounded = data[2] == "1";

            return crew;
        }
    }
}
