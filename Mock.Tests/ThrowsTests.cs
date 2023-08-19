using System;
using System.Threading.Tasks;
using Toubiana.Mock.Tests.Interfaces;
using Xunit;

namespace Toubiana.Mock.Tests
{
    public class ThrowsTests
    {
        [Fact]
        public void BasicNoReturnThrowsTest()
        {
            var mock = new Mock<IAnimal>();
            mock.Setup(c => c.Walk())
                .Throws(new ArgumentException(nameof(BasicNoReturnThrowsTest)));

            var ex = Assert.Throws<ArgumentException>(
                () => mock.Object.Walk());
            Assert.Equal(nameof(BasicNoReturnThrowsTest), ex.Message);
        }

        [Fact]
        public void BasicThrowsTest()
        {
            var mock = new Mock<IBasic>();
            mock.Setup(c => c.SimpleMethod())
                .Throws(new ArgumentException(nameof(BasicThrowsTest)));

            var ex = Assert.Throws<ArgumentException>(
                () => mock.Object.SimpleMethod());
            Assert.Equal(nameof(BasicThrowsTest), ex.Message);
        }

        [Fact]
        public async Task AsyncThrowsTest()
        {
            var mock = new Mock<IAsync>();
            mock.Setup(c => c.SimpleMethodAsync())
                .Throws(new ArgumentException(nameof(AsyncThrowsTest)));

            var ex = await Assert.ThrowsAsync<ArgumentException>(mock.Object.SimpleMethodAsync);
            Assert.Equal(nameof(AsyncThrowsTest), ex.Message);
        }
    }
}
