using Toubiana.Mock.Exceptions;
using Toubiana.Mock.Tests.Interfaces;
using Xunit;

namespace Toubiana.Mock.Tests
{
    public class TimesTests
    {
        [Fact]
        public void BasicTest()
        {
            Mock<IBasic> mock = GetSetupMock();
            mock.Object.SimpleMethod();

            mock.Verify(c => c.SimpleMethod(), Times.Once());
            mock.Verify(c => c.SimpleMethod(), Times.Exactly(1));
            Assert.Throws<VerifyFailedException>(
                () => mock.Verify(c => c.SimpleMethod(), Times.Never()));
            Assert.Throws<VerifyFailedException>(
                () => mock.Verify(c => c.SimpleMethod(), Times.Exactly(2)));
        }

        [Fact]
        public void AtMostTest()
        {
            Mock<IBasic> mock = GetSetupMock();

            mock.Verify(c => c.SimpleMethod(), Times.AtMost(3));
            mock.Object.SimpleMethod();
            mock.Verify(c => c.SimpleMethod(), Times.AtMost(3));
            mock.Object.SimpleMethod();
            mock.Verify(c => c.SimpleMethod(), Times.AtMost(3));
            mock.Object.SimpleMethod();
            mock.Verify(c => c.SimpleMethod(), Times.AtMost(3));

            mock.Object.SimpleMethod();
            Assert.Throws<VerifyFailedException>(
                () => mock.Verify(c => c.SimpleMethod(), Times.AtMost(3)));
            mock.Object.SimpleMethod();
            Assert.Throws<VerifyFailedException>(
                () => mock.Verify(c => c.SimpleMethod(), Times.AtMost(3)));
        }

        [Fact]
        public void AtLeastTest()
        {
            Mock<IBasic> mock = GetSetupMock();

            Assert.Throws<VerifyFailedException>(
                () => mock.Verify(c => c.SimpleMethod(), Times.AtLeast(3)));
            mock.Object.SimpleMethod();
            Assert.Throws<VerifyFailedException>(
                () => mock.Verify(c => c.SimpleMethod(), Times.AtLeast(3)));
            mock.Object.SimpleMethod();
            Assert.Throws<VerifyFailedException>(
                () => mock.Verify(c => c.SimpleMethod(), Times.AtLeast(3)));
            mock.Object.SimpleMethod();
            mock.Verify(c => c.SimpleMethod(), Times.AtLeast(3));

            mock.Object.SimpleMethod();
            mock.Verify(c => c.SimpleMethod(), Times.AtLeast(3));
            mock.Object.SimpleMethod();
            mock.Verify(c => c.SimpleMethod(), Times.AtLeast(3));
        }

        [Fact]
        public void AtLeastOnceTest()
        {
            Mock<IBasic> mock = GetSetupMock();

            Assert.Throws<VerifyFailedException>(
                () => mock.Verify(c => c.SimpleMethod(), Times.AtLeastOnce()));
            mock.Object.SimpleMethod();

            mock.Verify(c => c.SimpleMethod(), Times.AtLeastOnce());
            mock.Object.SimpleMethod();
            mock.Verify(c => c.SimpleMethod(), Times.AtLeastOnce());
            mock.Object.SimpleMethod();
            mock.Verify(c => c.SimpleMethod(), Times.AtLeastOnce());
        }

        [Fact]
        public void AtMostOnceTest()
        {
            Mock<IBasic> mock = GetSetupMock();

            mock.Verify(c => c.SimpleMethod(), Times.AtMostOnce());
            mock.Object.SimpleMethod();
            mock.Verify(c => c.SimpleMethod(), Times.AtMostOnce());

            mock.Object.SimpleMethod();
            Assert.Throws<VerifyFailedException>(
                () => mock.Verify(c => c.SimpleMethod(), Times.AtMostOnce()));
            mock.Object.SimpleMethod();
            Assert.Throws<VerifyFailedException>(
                () => mock.Verify(c => c.SimpleMethod(), Times.AtMostOnce()));
        }

        [Fact]
        public void BetweenTest()
        {
            Mock<IBasic> mock = GetSetupMock();

            Assert.Throws<VerifyFailedException>(
                () => mock.Verify(c => c.SimpleMethod(), Times.Between(3, 5)));
            mock.Object.SimpleMethod();
            Assert.Throws<VerifyFailedException>(
                () => mock.Verify(c => c.SimpleMethod(), Times.Between(3, 5)));
            mock.Object.SimpleMethod();
            Assert.Throws<VerifyFailedException>(
                () => mock.Verify(c => c.SimpleMethod(), Times.Between(3, 5)));
            mock.Object.SimpleMethod();

            mock.Verify(c => c.SimpleMethod(), Times.Between(3, 5));
            mock.Object.SimpleMethod();
            mock.Verify(c => c.SimpleMethod(), Times.Between(3, 5));
            mock.Object.SimpleMethod();
            mock.Verify(c => c.SimpleMethod(), Times.Between(3, 5));

            mock.Object.SimpleMethod();
            Assert.Throws<VerifyFailedException>(
                () => mock.Verify(c => c.SimpleMethod(), Times.Between(3, 5)));
        }

        private static Mock<IBasic> GetSetupMock()
        {
            var mock = new Mock<IBasic>();
            mock.Setup(c => c.SimpleMethod())
                .Returns("toto");
            return mock;
        }
    }
}
