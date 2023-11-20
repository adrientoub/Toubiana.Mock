using System.Threading.Tasks;
using Toubiana.Mock.Exceptions;
using Toubiana.Mock.Tests.Interfaces;
using Xunit;
using Xunit.Sdk;

namespace Toubiana.Mock.Tests
{
    public class MockBehaviorTests
    {
        [Fact]
        public void BehaviorStrict_NotSetupMethod()
        {
            var mock = new Mock<IBasic>(MockBehavior.Strict);

            Assert.Throws<MethodNotSetupException>(
                () => mock.Object.SimpleMethod());
        }

        [Fact]
        public void BehaviorStrict_NotSetupProp()
        {
            var mock = new Mock<IProperty>(MockBehavior.Strict);

            Assert.Throws<MethodNotSetupException>(
                () => mock.Object.Name);
        }

        [Fact]
        public async Task BehaviorStrict_NotSetupAsync()
        {
            var mock = new Mock<IAsync>(MockBehavior.Strict);

            await Assert.ThrowsAsync<MethodNotSetupException>(
                () => mock.Object.SimpleMethodAsync());
        }

        [Fact]
        public void BehaviorLoose_NotSetup()
        {
            var mock = new Mock<IBasic>(MockBehavior.Loose);

            Assert.Null(mock.Object.SimpleMethod());
        }

        [Fact]
        public void BehaviorLoose_NotSetupProp()
        {
            var mock = new Mock<IProperty>(MockBehavior.Loose);

            Assert.Null(mock.Object.Name);
        }

        [Fact(Skip = "Not yet implemented")]
        public async Task BehaviorLoose_NotSetupAsync()
        {
            var mock = new Mock<IAsync>(MockBehavior.Loose);

            Assert.Null(await mock.Object.SimpleMethodAsync());
        }

        [Fact(Skip = "Not yet implemented")]
        public void BehaviorLoose_NotSetupStruct()
        {
            var mock = new Mock<IRand>(MockBehavior.Loose);

            Assert.Equal(0, mock.Object.GetRandomNumber());
        }

        [Fact(Skip = "Not yet implemented")]
        public void BehaviorLoose_NotSetupEnumerable()
        {
            var mock = new Mock<IReturnCollection>(MockBehavior.Loose);

            Assert.Empty(mock.Object.GetInfiniteNumbers());
        }

        [Fact]
        public void BehaviorLoose_SetupVoidIsCalled()
        {
            var mock = new Mock<IAnimal>(MockBehavior.Loose);

            // TODO: add Callback when supported
            mock.Setup(mock => mock.Walk());

            mock.Object.Walk();
            mock.VerifyAll();
        }

        [Fact]
        public void BehaviorLoose_NotSetupVoid()
        {
            var mock = new Mock<IAnimal>(MockBehavior.Loose);

            mock.Object.Walk();
        }

        [Fact]
        public void BehaviorLoose_NotSetupOverload()
        {
            var mock = new Mock<IOverload>(MockBehavior.Loose);
            mock.Setup(c => c.OverloadedVoidMethod(It.IsAny<bool>()));

            mock.Object.OverloadedVoidMethod();
        }
    }
}
