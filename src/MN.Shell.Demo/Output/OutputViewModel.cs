using Caliburn.Micro;
using MN.Shell.Framework;
using MN.Shell.Framework.Messages;
using System;

namespace MN.Shell.Demo.Output
{
    public class OutputViewModel : ToolBase, IHandle<FolderChangedMessage>
    {
        private readonly IEventAggregator _eventAggregator;

        public OutputViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            _eventAggregator.Subscribe(this);
        }

        public override string DisplayName => "Output";

        public override ToolPosition InitialPosition => ToolPosition.Bottom;

        private string _output;

        public string Output
        {
            get => _output;
            set { _output = value; NotifyOfPropertyChange(); }
        }

        public void Handle(FolderChangedMessage message)
        {
            Output += $"Previous folder: [{message.PreviousFolder?.FullName ?? "null"}], current folder: [{message.CurrentFolder?.FullName ?? "null"}]{Environment.NewLine}";
        }
    }
}
