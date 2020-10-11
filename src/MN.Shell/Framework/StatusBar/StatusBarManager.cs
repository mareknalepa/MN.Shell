using MN.Shell.PluginContracts;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MN.Shell.Framework.StatusBar
{
    public class StatusBarManager : IStatusBarManager, IStatusBarBuilder
    {
        private readonly IEnumerable<IStatusBarProvider> _statusBarProviders;

        public StatusBarManager(IEnumerable<IStatusBarProvider> statusBarProviders)
        {
            _statusBarProviders = statusBarProviders;
            foreach (var statusBarProvider in _statusBarProviders)
                statusBarProvider.BuildStatusBar(this);
        }

        #region "Status bar builder"

        protected List<StatusBarItemDefinition> StatusBarItemDefinitions { get; } = new List<StatusBarItemDefinition>();

        public IStatusBarItemBuilder AddItem(string name)
        {
            var item = new StatusBarItemDefinition(name);
            StatusBarItemDefinitions.Add(item);

            return item;
        }

        public void RemoveItem(string name)
        {
            StatusBarItemDefinitions.RemoveAll(i => i.Name == name);
        }

        #endregion

        #region "StatusBar compiling"

        public ObservableCollection<StatusBarItemViewModel> StatusBarItems { get; }
            = new ObservableCollection<StatusBarItemViewModel>();

        public void CompileStatusBar()
        {
            StatusBarItems.Clear();
            var leftItems = StatusBarItemDefinitions.Where(d => !d.IsRightSide).OrderBy(d => d.Order);
            var rightItems = StatusBarItemDefinitions.Where(d => d.IsRightSide).OrderBy(d => d.Order);
            var items = leftItems.Concat(rightItems);

            foreach (var item in items)
            {
                StatusBarItems.Add(new StatusBarItemViewModel()
                {
                    Content = item.Content,
                    IsRightSide = item.IsRightSide,
                    Command = item.Command,
                });
            }
        }

        #endregion
    }
}
