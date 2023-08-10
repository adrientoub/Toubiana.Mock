using Toubiana.Mock.Exceptions;
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
        public void MockPropertyTest()
        {
            var propertyMock = new Mock<IProperty>();
            propertyMock.Setup(c => c.Name)
                   .Returns("Toto");

            Assert.Equal("Toto", propertyMock.Object.Name);
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

        [Fact]
        public void ComplexSetupTest()
        {
            var mock = new Mock<IBasic>();
            Assert.Throws<ExpressionTooComplexException>(
                () => mock.Setup(c => c.SimpleMethod() + c.SimpleMethod()));
        }

        [Fact]
        public void CallNotSetupMethodWithReturnTest()
        {
            var mockDog = new Mock<IAnimal>();

            Assert.Throws<MethodNotSetupException>(
                () => mockDog.Object.Talk());
        }

        [Fact]
        public void CallNotSetupMethodTest()
        {
            var mockDog = new Mock<IAnimal>();

            Assert.Throws<MethodNotSetupException>(
                () => mockDog.Object.Walk());
        }

        [Fact]
        public void ConvertTest()
        {
            var mock = new Mock<IConvert>();
            mock.Setup(c => c.Convert(1f))
                .Returns("1");
            mock.Setup(c => c.ToFloat(50, "", new object()))
                .Returns(72f);

            Assert.Equal("1", mock.Object.Convert(1f));
            Assert.Equal(72f, mock.Object.ToFloat(50, "", new object()));
        }

        [Fact]
        public void RandTest()
        {
            var mock = new Mock<IRand>();
            mock.Setup(r => r.GetRandomNumber())
                .Returns(8);

            Assert.Equal(8, mock.Object.GetRandomNumber());
        }
    }
}
