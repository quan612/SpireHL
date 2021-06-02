namespace SpireHL.Core
{
    public sealed class SharedInformation
    {
        private static readonly SharedInformation instance = new SharedInformation();
        static SharedInformation() { }

        private SharedInformation() { }

        // public static SharedInformation Instance => instance ?? (instance = new SharedInformation());
        public static SharedInformation Instance
        {
            get { return instance; }
        }
    }
}
