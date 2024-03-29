﻿using System.Collections.Generic;

namespace Gsemac.Text.Ini {

    public interface IIniPropertyCollection :
        ICollection<IIniProperty> {

        IIniProperty this[string name] { get; }

        void Add(string name, string value);
        bool Remove(string name);

        bool ContainsKey(string name);

    }

}