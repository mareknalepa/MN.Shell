using System;
using System.Windows;

namespace MN.Shell.MVVM
{
    /// <summary>
    /// ViewManager locates and creates Views for given ViewModels and then binds resolved Views to their ViewModels
    /// </summary>
    public class ViewManager : IViewManager
    {
        /// <summary>
        /// Creates or reuses instance of View for given ViewModel, binds them together and returns it
        /// </summary>
        /// <param name="viewModel">ViewModel to create or reuse View for</param>
        /// <returns>View instance binded to given ViewModel, ready to use</returns>
        public FrameworkElement GetViewFor(object viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            if (viewModel is IViewAware viewAware && viewAware.View != null)
                return viewAware.View;

            var viewType = TransformViewModelTypeToViewType(viewModel.GetType());
            var view = CreateView(viewType);

            BindViewToViewModel(view, viewModel);

            return view;
        }

        /// <summary>
        ///  Transforms ViewModel type into corresponding View type
        /// </summary>
        /// <param name="viewModelType">Type of ViewModel</param>
        /// <returns>Type of corresponding View</returns>
        protected virtual Type TransformViewModelTypeToViewType(Type viewModelType)
        {
            if (viewModelType == null)
                throw new ArgumentNullException(nameof(viewModelType));

#pragma warning disable CA1307 // Specify StringComparison
            var viewType = viewModelType.FullName.Replace("ViewModel", "View");
#pragma warning restore CA1307 // Specify StringComparison

            if (viewModelType.FullName == viewType)
                throw new ViewManagerException($"Cannot transform ViewModel type [{viewModelType.FullName}] " +
                    "into matching View type");

            try
            {
                return viewModelType.Assembly.GetType(viewType, throwOnError: true);
            }
            catch (Exception e)
            {
                throw new ViewManagerException($"Cannot load View type [{viewType}]", e);
            }
        }

        /// <summary>
        /// Creates instance of View for given View type
        /// </summary>
        /// <param name="viewType">View type</param>
        /// <returns>Instance of View</returns>
        protected virtual FrameworkElement CreateView(Type viewType)
        {
            if (viewType == null)
                throw new ArgumentNullException(nameof(viewType));

            object createdInstance;
            try
            {
                createdInstance = Activator.CreateInstance(viewType);
            }
            catch (Exception e)
            {
                throw new ViewManagerException($"Cannot create View for ViewModel [{viewType.FullName}]", e);
            }

            if (createdInstance is FrameworkElement viewInstance)
                return viewInstance;

            throw new ViewManagerException($"Created instance for View type [{viewType.FullName}] " +
                "is not a FrameworkElement");
        }

        /// <summary>
        /// Bind View to ViewModel by setting the DataContext
        /// </summary>
        /// <param name="view">View to bind</param>
        /// <param name="viewModel">ViewModel to bind to</param>
        protected virtual void BindViewToViewModel(FrameworkElement view, object viewModel)
        {
            if (view == null)
                throw new ArgumentNullException(nameof(view));

            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            view.DataContext = viewModel;

            if (viewModel is IViewAware viewAware)
                viewAware.AttachView(view);
        }
    }
}
