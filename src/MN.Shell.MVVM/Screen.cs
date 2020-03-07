using System;
using System.Windows;

namespace MN.Shell.MVVM
{
    /// <summary>
    /// Base class for ViewModels which are logical screens (e.g. Window ViewModel or Tab content ViewModel)
    /// </summary>
    public abstract class Screen : PropertyChangedBase, IScreen
    {
        #region "IViewAware implementation"

        /// <summary>
        /// Reference to the View associated with this ViewModel
        /// </summary>
        public FrameworkElement View { get; private set; }

        /// <summary>
        /// Attaches View to this ViewModel, should be used only internally
        /// </summary>
        /// <param name="view">View to attach to ViewModel</param>
        public void AttachView(FrameworkElement view)
        {
            if (view == null)
                throw new ArgumentNullException(nameof(view));

            if (View != null)
                throw new InvalidOperationException($"Cannot attach View [{view.GetType().FullName}]: " +
                    $"ViewModel [{GetType().FullName}] already has View [{View.GetType().FullName}] attached");

            View = view;
        }

        #endregion

        #region "IChild implementation"

        /// <summary>
        /// Logical parent component
        /// </summary>
        public IParent Parent { get; set; }

        #endregion

        #region "IHaveTitle implementation"

        private string _title;

        /// <summary>
        /// Title property of component displayed in UI
        /// </summary>
        public virtual string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        #endregion

        #region "ILifecycleAware implementation"

        /// <summary>
        /// Specifies the lifecycle state of ViewModel
        /// </summary>
        public LifecycleState State { get; private set; } = LifecycleState.Undefined;

        /// <summary>
        /// Checks if ViewModel is active
        /// </summary>
        public bool IsActive => State == LifecycleState.Active;

        /// <summary>
        /// Activates ViewModel
        /// </summary>
        public void Activate()
        {
            if (State == LifecycleState.Undefined)
                OnInitialized();

            if (State == LifecycleState.Undefined || State == LifecycleState.Inactive)
            {
                State = LifecycleState.Active;
                OnActivated();
            }
        }

        /// <summary>
        /// Deactivates ViewModel
        /// </summary>
        public void Deactivate()
        {
            if (State == LifecycleState.Active)
            {
                State = LifecycleState.Inactive;
                OnDeactivated();
            }
        }

        /// <summary>
        /// Closes ViewModel
        /// </summary>
        public void Close()
        {
            if (State == LifecycleState.Active || State == LifecycleState.Inactive)
            {
                State = LifecycleState.Closed;
                OnClosed();
            }
        }

        /// <summary>
        /// Handler called when ViewModel is activated for the first time
        /// </summary>
        protected virtual void OnInitialized() { }

        /// <summary>
        /// Handler called each time ViewModel is activated
        /// </summary>
        protected virtual void OnActivated() { }

        /// <summary>
        /// Handler called each time ViewModel is deactivated
        /// </summary>
        protected virtual void OnDeactivated() { }

        /// <summary>
        /// Handler called when ViewModel is closed, provided that it was initialized before
        /// </summary>
        protected virtual void OnClosed() { }

        #endregion

        #region "IClosable implementation"

        /// <summary>
        /// Asks parent to close this component instance
        /// </summary>
        /// <param name="dialogResult">In case of dialog ViewModel, the result should be passed as an argument</param>
        public virtual void RequestClose(bool? dialogResult = null)
        {
            if (Parent != null)
                Parent.CloseChild(this);
        }

        /// <summary>
        /// Checks if current component can be closed
        /// </summary>
        /// <returns>False if closing process should be cancelled, true otherwise</returns>
        public virtual bool CanBeClosed() => true;

        #endregion
    }
}
