using System;

namespace aoc.Internal
{
    abstract class Singleton<TSelf>
        where TSelf : Singleton<TSelf>
    {
        private static readonly Lazy<TSelf> _instance = new(CreateInstance);

        public static TSelf Instance => _instance.Value;

        private static TSelf CreateInstance() =>
            (TSelf)Activator.CreateInstance(typeof(TSelf), true);
    }
}
