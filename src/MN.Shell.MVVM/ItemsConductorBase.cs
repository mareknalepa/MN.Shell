using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MN.Shell.MVVM
{
    /// <summary>
    /// Base class for ViewModels having many children components and managing their lifecycle
    /// </summary>
    /// <typeparam name="T">Type of managed items</typeparam>
    public abstract class ItemsConductorBase<T> : Screen, IConductor<T>
        where T : class
    {
        /// <summary>
        /// ObservableCollection holding items conducted by this component
        /// </summary>
        protected ObservableCollection<T> ItemsCollection { get; } = new ObservableCollection<T>();

        /// <summary>
        /// Items conducted by this component
        /// </summary>
        public IEnumerable<T> Items => ItemsCollection;

        /// <summary>
        /// Make given item owned by current component and activate it
        /// </summary>
        /// <param name="item">Item to activate</param>
        public void ActivateItem(T item)
        {
            ActivateItemInternal(item);
        }

        /// <summary>
        /// Internal handler to activate item
        /// </summary>
        /// <param name="item">Item to activate</param>
        protected void ActivateItemInternal(T item)
        {
            if (!ItemsCollection.Contains(item))
            {
                ItemsCollection.Add(item);
                if (item is IClosable closable)
                    closable.CloseRequested += OnItemCloseRequested;
            }

            OnActivateItem(item);
        }

        /// <summary>
        /// Handler called when item activation was requested
        /// </summary>
        /// <param name="item">Item to activate</param>
        protected abstract void OnActivateItem(T item);

        /// <summary>
        /// Handler called when child item requests to be closed
        /// </summary>
        /// <param name="sender">Source of close request</param>
        /// <param name="result">Result (for dialogs only)</param>
        protected void OnItemCloseRequested(object sender, bool? result)
        {
            if (sender is T item)
                CloseItemInternal(item);
        }

        /// <summary>
        /// Close given item and disown it from current component
        /// </summary>
        /// <param name="item">Item to close</param>
        public void CloseItem(T item)
        {
            CloseItemInternal(item);
        }

        /// <summary>
        /// Internal handler to activate item
        /// </summary>
        /// <param name="item">Item to close</param>
        protected void CloseItemInternal(T item)
        {
            if (ItemsCollection.Contains(item))
            {
                int index = ItemsCollection.IndexOf(item);

                if (item is IClosable closable)
                {
                    if (!closable.CanBeClosed())
                        return;

                    closable.CloseRequested -= OnItemCloseRequested;
                }

                if (item is ILifecycleAware lifecycleAware)
                {
                    lifecycleAware.Deactivate();
                    lifecycleAware.Close();
                }

                ItemsCollection.Remove(item);
                OnAfterItemClosed(item, index);
            }
        }

        /// <summary>
        /// Handler called after specific item was closed
        /// </summary>
        /// <param name="item">Item which was closed</param>
        /// <param name="formerIndex">Index of closed item within ItemsCollection before closing</param>
        protected virtual void OnAfterItemClosed(T item, int formerIndex) { }

        /// <summary>
        /// Handler called when ViewModel is activated for the first time
        /// </summary>
        protected sealed override void OnInitialized()
        {
            OnConductorInitialized();
        }

        /// <summary>
        /// Handler called when ViewModel is closed, provided that it was initialized before
        /// </summary>
        protected sealed override void OnClosed()
        {
            foreach (var item in ItemsCollection.ToList())
            {
                if (item is ILifecycleAware lifecycleAwareItem)
                    lifecycleAwareItem.Close();

                ItemsCollection.Remove(item);
            }

            OnConductorClosed();
        }

        /// <summary>
        /// Checks if current component can be closed
        /// </summary>
        /// <returns>False if closing process should be cancelled, true otherwise</returns>
        public sealed override bool CanBeClosed()
        {
            bool canBeClosed = ConductorCanBeClosed();

            foreach (var closableItem in Items.OfType<IClosable>())
                canBeClosed &= closableItem.CanBeClosed();

            return canBeClosed;
        }

        /// <summary>
        /// Checks if Conductor can be closed (not taking items into account)
        /// </summary>
        /// <returns>False if closing process should be cancelled, true otherwise</returns>
        protected virtual bool ConductorCanBeClosed() => true;

        /// <summary>
        /// Handler called when Conductor is activated for the first time
        /// </summary>
        protected virtual void OnConductorInitialized() { }

        /// <summary>
        /// Handler called each time Conductor is activated
        /// </summary>
        protected virtual void OnConductorActivated() { }

        /// <summary>
        /// Handler called each time Conductor is deactivated
        /// </summary>
        protected virtual void OnConductorDeactivated() { }

        /// <summary>
        /// Handler called when Conductor is closed, provided that it was initialized before
        /// </summary>
        protected virtual void OnConductorClosed() { }
    }
}
