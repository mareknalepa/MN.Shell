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
        /// Finds type of View for given ViewModel
        /// </summary>
        /// <param name="viewModel">ViewModel to find View for</param>
        /// <returns>View type</returns>
        public virtual Type LocateViewFor(object viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            var viewModelType = viewModel.GetType();
            var targetViewTypeName = viewModelType.FullName.Replace("ViewModel", "View");

            if (viewModelType.FullName == targetViewTypeName)
                throw new ViewManagerException($"Cannot transform ViewModel type [{viewModelType.FullName}] " +
                    "into matching View type");

            try
            {
                return viewModelType.Assembly.GetType(targetViewTypeName, throwOnError: true);
            }
            catch (Exception e)
            {
                throw new ViewManagerException($"Cannot load View type [{targetViewTypeName}]", e);
            }
        }

        /// <summary>
        /// Creates instance of View for given View type
        /// </summary>
        /// <param name="viewType">View type</param>
        /// <returns>Instance of View</returns>
        public virtual FrameworkElement CreateViewFor(Type viewType)
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
        public virtual void BindViewToViewModel(FrameworkElement view, object viewModel)
        {
            if (view == null)
                throw new ArgumentNullException(nameof(view));

            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            view.DataContext = viewModel;
        }

        /// <summary>
        /// Returns ready to use View binded to given ViewModel
        /// </summary>
        /// <param name="viewModel">ViewModel to get View for</param>
        /// <returns>Ready to use View</returns>
        public FrameworkElement GetViewFor(object viewModel)
        {
            var viewType = LocateViewFor(viewModel);
            var view = CreateViewFor(viewType);
            BindViewToViewModel(view, viewModel);

            return view;
        }
    }
}
