using System;

namespace Toubiana.Mock
{
    internal abstract class ItMatcher
    {
        public abstract bool IsMatch(object? value);
    }

    internal class ItAnyMatcher : ItMatcher
    {
        private readonly Type _type;

        internal ItAnyMatcher(Type type)
        {
            _type = type;
        }

        public override bool IsMatch(object? value)
        {
            return value == null || _type.IsAssignableFrom(value.GetType());
        }

        public override string ToString()
        {
            return "It.IsAny<" + _type.Name + ">()";
        }
    }

    internal class ItValueMatcher : ItMatcher
    {
        private readonly object? _value;

        internal ItValueMatcher(object? value)
        {
            _value = value;
        }

        public override bool IsMatch(object? value)
        {
            return Equals(value, _value);
        }

        public override string ToString()
        {
            return _value?.ToString() ?? "null";
        }
    }

    internal class ItFuncMatcher<T> : ItMatcher
    {
        private readonly Delegate _matcher;

        public ItFuncMatcher(Delegate match)
        {
            _matcher = match;
        }

        public override bool IsMatch(object? value)
        {
            return value is T v && (bool)_matcher.DynamicInvoke(v)!;
        }

        public override string ToString()
        {
            return _matcher?.ToString() ?? "null";
        }
    }
}
