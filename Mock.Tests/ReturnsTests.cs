using System.Threading.Tasks;
using Toubiana.Mock.Tests.Interfaces;
using Xunit;

namespace Toubiana.Mock.Tests
{
    public class ReturnsTests
    {
        [Fact]
        public void BasicFuncReturnTest()
        {
            var mock = new Mock<IBasic>();
            mock.Setup(c => c.SimpleMethod())
                .Returns<string>(() => "toto");

            Assert.Equal("toto", mock.Object.SimpleMethod());
        }

        [Fact]
        public async Task AsyncFuncTest()
        {
            var mock = new Mock<IAsync>();
            mock.Setup(c => c.SimpleMethodAsync())
                .ReturnsAsync(() => "toto");

            Assert.Equal("toto", await mock.Object.SimpleMethodAsync());
        }

        [Fact]
        public async Task AsyncValueTest()
        {
            var mock = new Mock<IAsync>();
            mock.Setup(c => c.SimpleMethodAsync())
                .ReturnsAsync("toto");

            Assert.Equal("toto", await mock.Object.SimpleMethodAsync());
        }
    }
}
