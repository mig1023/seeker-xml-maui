﻿using System;

namespace SeekerMAUI.Abstract
{
    public interface IModification
    {
        string Name { get; set; }
        int Value { get; set; }
        string ValueString { get; set; }
        bool Empty { get; set; }
        bool Restore { get; set; }

        void Do();

        void Do(Abstract.ICharacter Character);
    }
}
