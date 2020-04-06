using System.Linq;

namespace MN.Shell.MVVM
{
    /// <summary>
    /// Base class for ViewModels having many children components and all active simultaneously
    /// </summary>
    /// <typeparam name="T">Type of managed items</typeparam>
    public abstract class ItemsConductorAllActive<T> : ItemsConductorBase<T>
        where T : class
    {
        /// <summary>
        /// Handler called when item activation was requested
        /// </summary>
        /// <param name="item">Item to activate</param>
        protected override void OnActivateItem(T item)
        {
            if (IsActive && item is ILifecycleAware lifecycleAware)
                lifecycleAware.Activate();
        }

        /// <summary>
        /// Handler called each time ViewModel is activated
        /// </summary>
        protected sealed override void OnActivated()
        {
            OnConductorActivated();

            foreach (var item in ItemsCollection.OfType<ILifecycleAware>())
                item.Activate();
        }

        /// <summary>
        /// Handler called each time ViewModel is deactivated
        /// </summary>
        protected sealed override void OnDeactivated()
        {
            foreach (var item in ItemsCollection.OfType<ILifecycleAware>())
                item.Deactivate();

            OnConductorDeactivated();
        }
    }
}
