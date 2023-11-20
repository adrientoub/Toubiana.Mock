namespace Toubiana.Mock.Tests.Interfaces
{
    public interface IOverload
    {
        void OverloadedVoidMethod();

        void OverloadedVoidMethod(bool parameter);

        int OverloadedIntMethod();

        int OverloadedIntMethod(bool parameter);
    }
}
