using System;
using System.Collections.Generic;
using System.Text;

namespace Gsemac.Core {

    public interface IFactory<T> {

        T Create();

    }

}