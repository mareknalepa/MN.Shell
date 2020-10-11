using MN.Shell.PluginContracts;
using System;

namespace MN.Shell.Demo
{
    public class DemoStatusBarProvider : IStatusBarProvider
    {
        public void BuildStatusBar(IStatusBarBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            builder
                .AddItem("status")
                .SetSizeAndPlacement(200, false, 10)
                .SetContent("Ready");
        }
    }
}
