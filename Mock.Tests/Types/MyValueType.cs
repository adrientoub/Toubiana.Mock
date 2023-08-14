namespace Toubiana.Mock.Tests.Types
{
    internal class MyValueType
    {
        private readonly int _value;

        public MyValueType(int value)
        {
            _value = value;
        }

        public override bool Equals(object? obj)
        {
            return obj is MyValueType other && _value == other._value;
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }
    }
}
