using MN.Shell.PluginContracts;
using System.Windows.Input;

namespace MN.Shell.Framework.StatusBar
{
    public class StatusBarItemDefinition : IStatusBarItemBuilder
    {
        public StatusBarItemDefinition(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public int MinWidth { get; private set; }

        public bool IsRightSide { get; private set; }

        public int Order { get; private set; }

        public string Content { get; private set; } = string.Empty;

        public ICommand Command { get; private set; }

        public IStatusBarItemBuilder SetSizeAndPlacement(int minWidth, bool isRightSide, int order)
        {
            MinWidth = minWidth;
            IsRightSide = isRightSide;
            Order = order;

            return this;
        }

        public IStatusBarItemBuilder SetContent(string content)
        {
            Content = content;

            return this;
        }

        public IStatusBarItemBuilder SetCommand(ICommand command)
        {
            Command = command;

            return this;
        }
    }
}
