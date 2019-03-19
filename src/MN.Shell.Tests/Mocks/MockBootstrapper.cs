using MN.Shell.Core;
using System;
using System.Collections.Generic;

namespace MN.Shell.Tests.Mocks
{
    public class MockBootstrapper : Bootstrapper
    {
        public MockBootstrapper() : base(false)
        {
        }

        public new object GetInstance(Type service, string key) => base.GetInstance(service, key);

        public new IEnumerable<object> GetAllInstances(Type service) => base.GetAllInstances(service);

        public new void BuildUp(object instance) => base.BuildUp(instance);
    }
}
