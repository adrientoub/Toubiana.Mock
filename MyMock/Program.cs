namespace Toubiana.MyMock
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            var x = new Mock<ITest>();
            x.Setup(c => c.MyInterface())
             .Returns("toto");
            var s = x.Object;

            Console.WriteLine(s.MyInterface());
        }
    }
}