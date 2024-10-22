using System;

namespace SeekerMAUI.Abstract
{
    public interface ICharacter
    {
        void Init();

        string Save();

        void Load(string saveLine);

        bool ThisIsProtagonist();

        void Set(object character);

        string Debug();
    }
}
