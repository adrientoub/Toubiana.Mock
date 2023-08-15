using Toubiana.Mock.TimesMatchers;

namespace Toubiana.Mock
{
    public abstract class Times
    {
        public static Times Once()
        {
            return Exactly(1);
        }

        public static Times Never()
        {
            return Exactly(0);
        }

        public static Times AtLeast(int count)
        {
            return new TimesAtLeast(count);
        }

        public static Times AtLeastOnce()
        {
            return new TimesAtLeast(1);
        }

        public static Times AtMost(int count)
        {
            return new TimesAtMost(count);
        }

        public static Times AtMostOnce()
        {
            return new TimesAtMost(1);
        }

        public static Times Exactly(int count)
        {
            return new TimesExact(count);
        }

        public static Times Between(int min, int max)
        {
            return new TimesBetween(min, max);
        }

        internal abstract bool Match(int callCount);
    }
}
