namespace Toubiana.Mock.TimesMatchers
{
    internal class TimesAtMost : Times
    {
        private readonly int _count;

        public TimesAtMost(int count)
        {
            _count = count;
        }

        public override string? ToString()
        {
            return $"at most {_count}";
        }

        internal override bool Match(int callCount)
        {
            return callCount <= _count;
        }
    }
}