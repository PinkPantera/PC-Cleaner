namespace LogicielNettoyagePC.InversionOfControl
{
    public static class IoC
    {
        public static DependencyContainer Container { get; private set; }

        static IoC()
        {
            Container = new DependencyContainer();
        }
    }
}
