using Toubiana.Mock.Exceptions;
using Toubiana.Mock.Tests.Interfaces;
using Xunit;

namespace Toubiana.Mock.Tests
{
    public class VerifyTests
    {
        [Fact]
        public void TestZeroCallExpectVerifyZero()
        {
            var mock = new Mock<IBasic>();
            mock.Setup(c => c.SimpleMethod())
                .Returns("toto");

            mock.Verify(c => c.SimpleMethod(), 0);
        }

        [Fact]
        public void TestNoSetup()
        {
            var mock = new Mock<IBasic>();

            Assert.Throws<MethodNotSetupException>(
                () => mock.Verify(c => c.SimpleMethod(), 0));
        }

        [Fact]
        public void TestOneCallExpectVerifyOne()
        {
            var mock = new Mock<IBasic>();
            mock.Setup(c => c.SimpleMethod())
                .Returns("toto");

            mock.Object.SimpleMethod();

            mock.Verify(c => c.SimpleMethod(), 1);
        }

        [Fact]
        public void TestOneCallExpectedZero()
        {
            var mock = new Mock<IBasic>();
            mock.Setup(c => c.SimpleMethod())
                .Returns("toto");

            mock.Object.SimpleMethod();

            Assert.Throws<VerifyFailedException>(
                () => mock.Verify(c => c.SimpleMethod(), 0));
        }


        [Fact]
        public void TestOneCallExpectVerifyOne_Void()
        {
            var mock = new Mock<IAnimal>();
            mock.Setup(c => c.Walk());

            mock.Object.Walk();

            mock.Verify(c => c.Walk(), 1);
        }

        [Fact]
        public void TestOneCallExpectedZero_Void()
        {
            var mock = new Mock<IAnimal>();
            mock.Setup(c => c.Walk());

            mock.Object.Walk();

            Assert.Throws<VerifyFailedException>(
                () => mock.Verify(c => c.Walk(), 0));
        }
    }
}
