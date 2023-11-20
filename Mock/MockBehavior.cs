namespace Toubiana.Mock
{
    public enum MockBehavior
    {
        /// <summary>
        /// Calling a not setup method will return the default value.
        /// </summary>
        Loose,

        /// <summary>
        /// Calling a not setup method will throw an exception.
        /// </summary>
        Strict,
    }
}