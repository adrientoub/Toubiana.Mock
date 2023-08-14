using Toubiana.Mock.Exceptions;
using Toubiana.Mock.Tests.Interfaces;
using Xunit;

namespace Toubiana.Mock.Tests
{
    public class ItTests
    {
        [Fact]
        public void ItIsAny_BasicTest()
        {
            var mock = new Mock<IConvert>();
            mock.Setup(c => c.Convert(It.IsAny<float>()))
                .Returns("toto");

            Assert.Equal("toto", mock.Object.Convert(5f));
        }

        [Fact]
        public void ItIsSetup_BasicTest()
        {
            var mock = new Mock<IConvert>();
            mock.Setup(c => c.Convert(5f))
                .Returns("5");
            mock.Setup(c => c.Convert(18f))
                .Returns("18");

            Assert.Equal("5", mock.Object.Convert(5f));
            Assert.Equal("18", mock.Object.Convert(18f));
        }

        [Fact]
        public void ItIsAny_CheckTypeIsCorrectTest()
        {
            var mock = new Mock<ICheckObject>();
            mock.Setup(c => c.CheckObject(It.IsAny<string>()))
                .Returns(true);
            mock.Setup(c => c.CheckObject(It.IsAny<float>()))
                .Returns(false);

            Assert.True(mock.Object.CheckObject("test"));
            Assert.False(mock.Object.CheckObject(5f));
            Assert.Throws<NoMatchingSetupException>(
                () => mock.Object.CheckObject(new object()));
        }

        [Fact]
        public void ItIs_CheckTypeIsCorrectTest()
        {
            var mock = new Mock<ICheckObject>();
            mock.Setup(c => c.CheckObject("toto"))
                .Returns(true);
            mock.Setup(c => c.CheckObject("test"))
                .Returns(false);
            mock.Setup(c => c.CheckObject(12))
                .Returns(false);

            Assert.True(mock.Object.CheckObject("toto"));
            Assert.False(mock.Object.CheckObject("test"));
            Assert.False(mock.Object.CheckObject(12));
            Assert.Throws<NoMatchingSetupException>(
                () => mock.Object.CheckObject(new object()));
        }
    }
}
