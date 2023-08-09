using Toubiana.Mock.Tests.Interfaces;
using Xunit;

namespace Toubiana.Mock.Tests
{
    public class MockTests
    {
        [Fact]
        public void BasicTest()
        {
            var mock = new Mock<IBasic>();
            mock.Setup(c => c.SimpleMethod())
                .Returns("toto");

            Assert.Equal("toto", mock.Object.SimpleMethod());
        }

        [Fact]
        public void AnimalTest()
        {
            var mockDog = new Mock<IAnimal>();
            mockDog.Setup(c => c.Talk())
                   .Returns("woof");
            mockDog.Setup(c => c.Walk());

            Assert.Equal("woof", mockDog.Object.Talk());
            mockDog.Object.Walk();
        }

        [Fact]
        public void DoubleTest()
        {
            var mockDog = new Mock<IAnimal>();
            mockDog.Setup(c => c.Talk())
                   .Returns("woof");
            var mock = new Mock<IBasic>();
            mock.Setup(c => c.SimpleMethod())
                .Returns("toto");

            Assert.Equal("woof", mockDog.Object.Talk());
            Assert.Equal("toto", mock.Object.SimpleMethod());
        }
    }
}