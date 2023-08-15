namespace Toubiana.Mock.TimesMatchers
{
    internal class TimesAtLeast : Times
    {
        private readonly int _count;

        public TimesAtLeast(int count)
        {
            _count = count;
        }

        public override string? ToString()
        {
            return $"at least {_count}";
        }

        internal override bool Match(int callCount)
        {
            return callCount >= _count;
        }
    }
}