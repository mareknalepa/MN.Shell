using Ninject;
using System.Collections.Generic;

namespace MN.Shell.Tests.Mocks
{
    public class MockServiceConsumer
    {
        [Inject]
        public IEnumerable<IMockService> Services { get; set; }
    }
}
