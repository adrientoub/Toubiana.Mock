using System;

namespace Toubiana.Mock
{
    public static class It
    {
        public static T? IsAny<T>()
        {
            return default(T);
        }

        public static T? Is<T>(Func<T, bool> matcher)
        {
            return default(T);
        }

        public static T? Is<T>(T? value)
        {
            return default(T);
        }
    }
}
