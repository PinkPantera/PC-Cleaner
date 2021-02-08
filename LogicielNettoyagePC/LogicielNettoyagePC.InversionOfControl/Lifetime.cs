using System;
using System.Collections.Generic;
using System.Text;

namespace LogicielNettoyagePC.InversionOfControl
{
    public enum Lifetime
    {
        PerResolve,
        PerThread,
        Singleton
    }
}
