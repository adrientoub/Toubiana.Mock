namespace Toubiana.Mock.Tests.Interfaces
{
    public interface IConvert
    {
        public string Convert(float input);

        public float ToFloat(int input, string str, object obj);
    }
}
