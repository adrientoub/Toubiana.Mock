namespace Toubiana.Mock
{
    internal class TimesBetween : Times
    {
        private readonly int _min;
        private readonly int _max;

        public TimesBetween(int min, int max)
        {
            _min = min;
            _max = max;
        }

        public override string? ToString()
        {
            return $"between {_min} and {_max}";
        }

        internal override bool Match(int callCount)
        {
            return callCount >= _min && callCount <= _max;
        }
    }
}
