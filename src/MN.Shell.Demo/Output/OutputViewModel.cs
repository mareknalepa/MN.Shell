using MN.Shell.Framework.Messages;
using MN.Shell.MVVM;
using MN.Shell.PluginContracts;
using System;

namespace MN.Shell.Demo.Output
{
    public class OutputViewModel : ToolBase, IListener<FolderChangedMessage>
    {
        private readonly IMessageBus _messageBus;

        public OutputViewModel(IMessageBus messageBus)
        {
            Title = "Output";

            _messageBus = messageBus;

            _messageBus.Subscribe(this);
        }

        public override ToolPosition InitialPosition => ToolPosition.Bottom;

        private string _output;

        public string Output
        {
            get => _output;
            set => Set(ref _output, value);
        }

        public void Process(FolderChangedMessage message)
        {
            Output += $"Previous folder: [{message?.PreviousFolder?.FullName ?? "null"}], " +
                $"current folder: [{message?.CurrentFolder?.FullName ?? "null"}]{Environment.NewLine}";
        }
    }
}
