using System;

namespace SeekerMAUI.Gamebook.FIFA1966
{
    class Vars
    {
        private readonly IDictionary<string, int> _vars = new Dictionary<string, int>();

        public int this[string key]
        {
            get
            {
                return _vars[key];
            }

            set
            {
                _vars[key] = value;
            }
        }

        public List<string> Keys() =>
            _vars.Keys.ToList();

        public bool ContainsKey(string name) =>
            _vars.ContainsKey(name);

        public Dictionary<string, int> ToDictionary() =>
            _vars.ToDictionary<string, int>();
    }
}
