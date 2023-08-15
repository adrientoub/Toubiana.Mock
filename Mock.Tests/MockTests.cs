using Toubiana.Mock.Exceptions;
using Toubiana.Mock.Tests.Interfaces;
using Toubiana.Mock.Tests.Types;
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
        public void SimpleConvertTest()
        {
            var mock = new Mock<IConvert>();
            mock.Setup(c => c.Convert(1f))
                .Returns("1");

            Assert.Equal("1", mock.Object.Convert(1f));
        }

        [Fact]
        public void MockGetTest()
        {
            var mock = new Mock<IConvert>();
            var obj = mock.Object;

            var retrievedMock = Mock.Get(obj);
            Assert.Equal(mock, retrievedMock);
        }

        [Fact]
        public void ToFloatConvertWithNewInSetupTest()
        {
            var mock = new Mock<IConvert>();
            var x = new object();

            // Uses new matcher
            mock.Setup(c => c.ToFloat(50, "", new MyValueType(50)))
                .Returns(72f);

            Assert.Equal(72f, mock.Object.ToFloat(50, "", new MyValueType(50)));
        }

        [Fact]
        public void ToFloatConvertTest()
        {
            var mock = new Mock<IConvert>();
            var x = new object();
            mock.Setup(c => c.ToFloat(50, "", x))
                .Returns(72f);

            Assert.Equal(72f, mock.Object.ToFloat(50, "", x));
        }

        [Fact]
        public void ConvertVariableTest()
        {
            var mock = new Mock<IConvert>();
            var x = 1f;
            mock.Setup(c => c.Convert(x))
                .Returns("1");

            Assert.Equal("1", mock.Object.Convert(1f));
        }

        [Fact]
        public void MockBigParameterList()
        {
            var mock = new Mock<IBigParameterList>();
            mock.Setup(c => c.MethodWithBigParameterList(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18))
                .Returns(18);
            mock.Object.MethodWithBigParameterList(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18);
        }

        [Fact]
        public void RandTest()
        {
            var mock = new Mock<IRand>();
            mock.Setup(r => r.GetRandomNumber())
                .Returns(8);

            Assert.Equal(8, mock.Object.GetRandomNumber());
        }

        [Fact]
        public void IncompleteSetupTest()
        {
            var mock = new Mock<IRand>();
            mock.Setup(r => r.GetRandomNumber());

            Assert.Throws<IncompleteSetupException>(
                () => mock.Object.GetRandomNumber());
        }
    }
}
