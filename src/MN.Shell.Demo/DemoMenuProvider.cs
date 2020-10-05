using MN.Shell.Framework.Menu;
using MN.Shell.Framework.MessageBox;
using MN.Shell.MVVM;
using MN.Shell.PluginContracts;
using System;
using System.Collections.Generic;

namespace MN.Shell.Demo
{
    public class DemoMenuProvider : IMenuProvider
    {
        private readonly IMessageBoxManager _messageBoxManager;

        public DemoMenuProvider(IMessageBoxManager messageBoxManager)
        {
            _messageBoxManager = messageBoxManager;
        }

        public IEnumerable<IMenuItem> GetMenuItems()
        {
            yield return new MenuItem()
            {
                Name = "Select All",
                Path = new[] { "Edit" },
                GroupId = 20,
                GroupOrder = 10,
            };

            yield return new MenuItem()
            {
                Name = "MessageBox",
                Path = new[] { "Edit" },
                GroupId = 0,
                GroupOrder = 36,
            };

            yield return new MenuItem()
            {
                Name = "Cut",
                Path = new[] { "Edit" },
                GroupId = 10,
                GroupOrder = 10,
            };

            yield return new MenuItem()
            {
                Name = "Copy",
                Path = new[] { "Edit" },
                GroupId = 10,
                GroupOrder = 20,
            };

            yield return new MenuItem()
            {
                Name = "Paste",
                Path = new[] { "Edit" },
                GroupId = 10,
                GroupOrder = 30,
            };

            yield return new MenuItem()
            {
                Name = "Output",
                Path = new[] { "View" },
            };

            yield return new MenuItem()
            {
                Name = "Preferences...",
                Path = new[] { "Edit" },
                GroupId = 30,
                GroupOrder = 10,
            };

            yield return new MenuItem()
            {
                Name = "MessageBox without type",
                Path = new[] { "Edit", "MessageBox" },
                GroupId = 10,
                GroupOrder = 10,
                Command = new Command(() =>
                {
                    _messageBoxManager.Show("MessageBox Demo",
                        "This is an example of MessageBox without specific type.",
                        MessageBoxType.None);
                }),
            };

            yield return new MenuItem()
            {
                Name = "Info MessageBox",
                Path = new[] { "Edit", "MessageBox" },
                GroupId = 20,
                GroupOrder = 10,
                Command = new Command(() =>
                {
                    _messageBoxManager.Show("MessageBox Demo",
                        "This is an example of Info MessageBox.",
                        MessageBoxType.Info);
                }),
            };

            yield return new MenuItem()
            {
                Name = "Warning MessageBox",
                Path = new[] { "Edit", "MessageBox" },
                GroupId = 20,
                GroupOrder = 20,
                Command = new Command(() =>
                {
                    _messageBoxManager.Show("MessageBox Demo",
                        "This is an example of Warning MessageBox.",
                        MessageBoxType.Warning);
                }),
            };

            yield return new MenuItem()
            {
                Name = "Error MessageBox",
                Path = new[] { "Edit", "MessageBox" },
                GroupId = 20,
                GroupOrder = 30,
                Command = new Command(() =>
                {
                    _messageBoxManager.Show("MessageBox Demo",
                        "This is an example of Error MessageBox.",
                        MessageBoxType.Error);
                }),
            };

            yield return new MenuItem()
            {
                Name = "MessageBox - OK",
                Path = new[] { "Edit", "MessageBox" },
                GroupId = 30,
                GroupOrder = 10,
                Command = new Command(() =>
                {
                    _messageBoxManager.Show("MessageBox Demo",
                        "This is an example of MessageBox with OK button.",
                        MessageBoxType.None, MessageBoxButtons.Ok);
                }),
            };

            yield return new MenuItem()
            {
                Name = "MessageBox - OK, Cancel",
                Path = new[] { "Edit", "MessageBox" },
                GroupId = 30,
                GroupOrder = 20,
                Command = new Command(() =>
                {
                    _messageBoxManager.Show("MessageBox Demo",
                        "This is an example of MessageBox with OK and Cancel buttons.",
                        MessageBoxType.None, MessageBoxButtons.OkCancel);
                }),
            };

            yield return new MenuItem()
            {
                Name = "MessageBox - Yes, No",
                Path = new[] { "Edit", "MessageBox" },
                GroupId = 30,
                GroupOrder = 30,
                Command = new Command(() =>
                {
                    _messageBoxManager.Show("MessageBox Demo",
                        "This is an example of MessageBox with Yes and No buttons.",
                        MessageBoxType.None, MessageBoxButtons.YesNo);
                }),
            };

            yield return new MenuItem()
            {
                Name = "MessageBox - Yes, No, Cancel",
                Path = new[] { "Edit", "MessageBox" },
                GroupId = 30,
                GroupOrder = 40,
                Command = new Command(() =>
                {
                    _messageBoxManager.Show("MessageBox Demo",
                        "This is an example of MessageBox with Yes, No and Cancel buttons.",
                        MessageBoxType.None, MessageBoxButtons.YesNoCancel);
                }),
            };

            yield return new MenuItem()
            {
                Name = "MessageBox with very long message",
                Path = new[] { "Edit", "MessageBox" },
                GroupId = 40,
                GroupOrder = 10,
                Command = new Command(() =>
                {
                    _messageBoxManager.Show("MessageBox Demo",
                        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt " +
                        "ut labore et dolore magna aliqua. Lorem ipsum dolor sit amet consectetur adipiscing elit " +
                        "duis. Enim neque volutpat ac tincidunt vitae semper quis. Sit amet nulla facilisi morbi " +
                        "tempus. Lacus vel facilisis volutpat est velit egestas dui. Non quam lacus suspendisse " +
                        "faucibus interdum. Vestibulum morbi blandit cursus risus at ultrices mi tempus. " +
                        "Sollicitudin aliquam ultrices sagittis orci a scelerisque purus. Lacus vel facilisis " +
                        "volutpat est velit egestas dui. Amet justo donec enim diam vulputate ut pharetra.",
                        MessageBoxType.Info, MessageBoxButtons.Ok);
                }),
            };

            yield return new MenuItem()
            {
                Name = "Tools",
                GroupOrder = 36,
            };

            yield return new MenuItem()
            {
                Name = "Settings",
                Path = new[] { "Tools" },
            };

            yield return new MenuItem()
            {
                Name = "About",
                Path = new[] { "Help" },
                Command = new Command(() =>
                {
                    _messageBoxManager.Show("About",
                        "MN.Shell Demo Application" + Environment.NewLine + "Version 0.1.0", MessageBoxType.Info);
                }),
            };
        }
    }
}
