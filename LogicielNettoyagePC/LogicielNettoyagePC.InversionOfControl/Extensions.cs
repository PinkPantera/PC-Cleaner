using System;
using System.Collections.Generic;
using System.Text;
using Unity.Lifetime;

namespace LogicielNettoyagePC.InversionOfControl
{
    public static class Extensions
    {
        public static ITypeLifetimeManager ToUnityLifetimeManager(this Lifetime lifetime)
        {
            switch (lifetime)
            {
                case Lifetime.PerResolve:
                    return new PerResolveLifetimeManager();

                case Lifetime.PerThread:
                    return new PerThreadLifetimeManager();

                case Lifetime.Singleton:
                    return new ContainerControlledLifetimeManager();

                default:
                    throw new NotSupportedException($"Value {lifetime} is not supported");
            }
        }
    }
}
