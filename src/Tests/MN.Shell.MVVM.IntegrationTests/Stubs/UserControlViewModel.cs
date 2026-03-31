using System.Windows.Controls;

namespace MN.Shell.MVVM.IntegrationTests.Stubs
{
    internal sealed class UserControlViewModel : IHaveTitle
    {
        public string Title => "UserControl Title";
        public Action<UserControl>? OnLoadedAction { get; set; }
    }
}
