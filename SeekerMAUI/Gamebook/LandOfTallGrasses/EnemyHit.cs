using System;

namespace SeekerMAUI.Gamebook.LandOfTallGrasses
{
    class EnemyHit
    {
        public string Name { get; set; }

        public int Hit {  get; set; }

        public Character Link { get; set; }

        public EnemyHit(string name, int hit, Character link)
        {
            Name = name;
            Hit = hit;
            Link = link;
        }
    }
}
