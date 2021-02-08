using System;
using System.Collections.Generic;
using System.Linq;
using Unity;
using Unity.Lifetime;
using Unity.Resolution;

namespace LogicielNettoyagePC.InversionOfControl
{
    public class DependencyContainer
    {
        private readonly IUnityContainer unityContainer = new UnityContainer();

        public DependencyContainer Clear()
        {
            foreach (var registration in unityContainer.Registrations.Where (r=>r.LifetimeManager!=null ))
            {
                registration.LifetimeManager.RemoveValue();
            }
            return this;
        }


        #region Register<T>
        public DependencyContainer Register<T>()
        {
            return Register<T, T>();
        }

        public DependencyContainer Register<T>(Lifetime lifetime)
        {
            return Register<T, T>(lifetime);
        }

        #endregion Register<T>

        #region Register <TFrom, TTo>
        public DependencyContainer Register<TFrom, TTo>()
              where TTo : TFrom
        {
            unityContainer.RegisterType<TFrom, TTo>();

            return this;
        }

        public DependencyContainer Register<TFrom, TTo>(Lifetime lifetime)
            where TTo : TFrom
        {
            ITypeLifetimeManager lifetimeManager = lifetime.ToUnityLifetimeManager();

            unityContainer.RegisterType<TFrom, TTo>(lifetimeManager);

            return this;
        }

        public DependencyContainer Register<TFrom, TTo>(string name, Lifetime lifetime)
            where TTo : TFrom
        {
            ITypeLifetimeManager lifetimeManager = lifetime.ToUnityLifetimeManager();

            unityContainer.RegisterType<TFrom, TTo>(name, lifetimeManager);

            return this;
        }
        #endregion Register <TFrom, TTo>

        #region Resolve<T>
        public T Resolve<T>(params NameValueParameter[] ctorParameters)
        {
            var parameters = ctorParameters.Select(p => new ParameterOverride(p.Name, p.Value)).ToArray();
            return unityContainer.Resolve<T>(parameters);
        }

        public T Resolve<T>(string name, params NameValueParameter[] ctorParameters)
        {
            var parameters = ctorParameters.Select(p => new ParameterOverride(p.Name, p.Value)).ToArray();
            return unityContainer.Resolve<T>(name, parameters);
        }

        public IEnumerable<T> ResolveAll<T>()
        {
            return unityContainer.ResolveAll<T>();
        }
        #endregion // Resolve<T>

    }
}
