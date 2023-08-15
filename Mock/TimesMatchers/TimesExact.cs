namespace Toubiana.Mock.TimesMatchers
{
    internal class TimesExact : Times
    {
        private readonly int _count;

        public TimesExact(int count)
        {
            _count = count;
        }

        public override string? ToString()
        {
            return $"exactly {_count}";
        }

        internal override bool Match(int callCount)
        {
            return callCount == _count;
        }
    }
}
