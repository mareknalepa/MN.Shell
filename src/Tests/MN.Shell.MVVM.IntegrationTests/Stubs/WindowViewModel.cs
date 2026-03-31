using System.Windows;

namespace MN.Shell.MVVM.IntegrationTests.Stubs
{
    internal sealed class WindowViewModel : IHaveTitle
    {
        public string Title => "Window Title";
        public Action<Window>? OnLoadedAction { get; set; }

    }
}
