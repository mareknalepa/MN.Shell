using MN.Shell.Framework.MessageBox;
using MN.Shell.MVVM;
using MN.Shell.PluginContracts;
using System;

namespace MN.Shell.Demo
{
    public class DemoMenuProvider : IMenuProvider
    {
        private readonly IMessageBoxManager _messageBoxManager;

        public DemoMenuProvider(IMessageBoxManager messageBoxManager)
        {
            _messageBoxManager = messageBoxManager;
        }

        public void BuildMenu(IMenuBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            builder
                .AddItem("/Edit/Select All", "")
                .SetPlacement(0, 10);

            builder
                .AddItem("/File/New Project...", "")
                .SetPlacement(0, 10);

            builder
                .AddItem("/File/Advanced Mode", "")
                .SetPlacement(10, 10)
                .SetCheckbox(false, true);

            builder
                .AddItem("Help/About...", "")
                .SetPlacement(100, 100)
                .SetCommand(new Command(() =>
                {
                    _messageBoxManager.Show("About",
                        "MN.Shell Demo Application" + Environment.NewLine + "Version 0.1.0", MessageBoxType.Info);
                }));
        }
    }
}
