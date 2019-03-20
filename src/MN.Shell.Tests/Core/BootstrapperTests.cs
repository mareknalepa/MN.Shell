using Caliburn.Micro;
using MN.Shell.Core;
using MN.Shell.Tests.Mocks;
using NUnit.Framework;
using System;
using System.Linq;
using System.Reflection;

namespace MN.Shell.Tests.Core
{
    [TestFixture]
    public class BootstrapperTests
    {
        [SetUp]
        [TearDown]
        public void SetUpTearDown()
        {
            AssemblySource.Instance.Clear();
        }

        [Test]
        public void BootstrapperInitTest()
        {
            Bootstrapper bootstrapper = new Bootstrapper(false);
            Assert.NotNull(bootstrapper);

            Assert.That(AssemblySource.Instance, Contains.Item(typeof(Bootstrapper).Assembly));
            Assert.That(AssemblySource.Instance, Contains.Item(Assembly.GetExecutingAssembly()));
        }

        [Test]
        public void BootstrapperGetInstanceTest()
        {
            MockBootstrapper bootstrapper = new MockBootstrapper();
            var instance = bootstrapper.GetInstance(typeof(IShell), string.Empty);

            Assert.NotNull(instance);
            Assert.AreEqual(typeof(MockShell), instance.GetType());
        }

        [Test]
        public void BootstrapperGetInstanceNullServiceTest()
        {
            MockBootstrapper bootstrapper = new MockBootstrapper();
            Assert.Throws<ArgumentNullException>(() => bootstrapper.GetInstance(null, string.Empty));
        }

        [Test]
        public void BootstrapperGetAllInstancesTest()
        {
            MockBootstrapper bootstrapper = new MockBootstrapper();
            var instances = bootstrapper.GetAllInstances(typeof(IMockService));

            Assert.IsNotEmpty(instances);
            Assert.AreEqual(typeof(MockService1), instances.First().GetType());
            Assert.AreEqual(typeof(MockService2), instances.Skip(1).First().GetType());
        }

        [Test]
        public void BootstrapperGetAllInstancesNullServiceTest()
        {
            MockBootstrapper bootstrapper = new MockBootstrapper();
            Assert.Throws<ArgumentNullException>(() => bootstrapper.GetAllInstances(null));
        }

        [Test]
        public void BootstrapperBuildUpTest()
        {
            MockBootstrapper bootstrapper = new MockBootstrapper();
            MockServiceConsumer mockServiceConsumer = new MockServiceConsumer();
            Assert.Null(mockServiceConsumer.Services);

            bootstrapper.BuildUp(mockServiceConsumer);

            Assert.IsNotEmpty(mockServiceConsumer.Services);
            Assert.AreEqual(typeof(MockService1), mockServiceConsumer.Services.First().GetType());
            Assert.AreEqual(typeof(MockService2), mockServiceConsumer.Services.Skip(1).First().GetType());
        }
    }
}
