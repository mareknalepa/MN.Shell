using System.Linq;

namespace MN.Shell.MVVM
{
    public abstract class ItemsConductorOneActive<T> : ItemsConductorBase<T>
        where T : class
    {
        private T _activeItem;

        /// <summary>
        /// Currently active item
        /// </summary>
        public T ActiveItem
        {
            get => _activeItem;
            set => ActivateItemInternal(value);
        }

        /// <summary>
        /// Handler called when item activation was requested
        /// </summary>
        /// <param name="item">Item to activate</param>
        protected override void OnActivateItem(T item)
        {
            if (IsActive && ActiveItem is ILifecycleAware oldLifecycleAware)
                oldLifecycleAware.Deactivate();

            _activeItem = item;
            NotifyPropertyChanged(nameof(ActiveItem));

            if (IsActive && ActiveItem is ILifecycleAware newLifecycleAware)
                newLifecycleAware.Activate();
        }

        /// <summary>
        /// Handler called each time ViewModel is activated
        /// </summary>
        protected sealed override void OnActivated()
        {
            OnConductorActivated();

            if (ActiveItem is ILifecycleAware lifecycleAwareActiveItem)
                lifecycleAwareActiveItem.Activate();
        }

        /// <summary>
        /// Handler called each time ViewModel is deactivated
        /// </summary>
        protected sealed override void OnDeactivated()
        {
            if (ActiveItem is ILifecycleAware lifecycleAwareActiveItem)
                lifecycleAwareActiveItem.Deactivate();

            OnConductorDeactivated();
        }

        /// <summary>
        /// Handler called after specific item was closed
        /// </summary>
        /// <param name="item">Item which was closed</param>
        /// <param name="formerIndex">Index of closed item within ItemsCollection before closing</param>
        protected override void OnAfterItemClosed(T item, int formerIndex)
        {
            if (ActiveItem == item)
            {
                if (formerIndex < ItemsCollection.Count)
                    ActiveItem = ItemsCollection[formerIndex];
                else
                    ActiveItem = ItemsCollection.LastOrDefault();
            }
        }
    }
}
