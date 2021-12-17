using System;
using System.Collections.Generic;
using System.Windows;

namespace MN.Shell.MVVM
{
    /// <summary>
    /// ViewManager locates and creates Views for given ViewModels and then binds resolved Views to their ViewModels
    /// </summary>
    public class ViewManager : IViewManager
    {
        private readonly Dictionary<Type, Type> _mappingsCache = new();

        /// <summary>
        /// Factory method used to create instances of views.
        /// Uses reflection by default, but can be configured to use IoC container instead.
        /// </summary>
        public Func<Type, object> ViewFactory { get; set; } = type => Activator.CreateInstance(type);

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

            if (_mappingsCache.TryGetValue(viewModelType, out var cachedViewType))
                return cachedViewType;
            var viewTypeName = viewModelType.FullName.Replace("ViewModel", "View");
            if (viewModelType.FullName == viewTypeName)
                throw new InvalidOperationException($"Cannot transform ViewModel type [{viewModelType.FullName}] " +
                    "into matching View type");

            try
            {
                var viewType = viewModelType.Assembly.GetType(viewTypeName, throwOnError: true);
                _mappingsCache.Add(viewModelType, viewType);
                return viewType;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Cannot load View type [{viewTypeName}]", e);
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
                createdInstance = ViewFactory?.Invoke(viewType);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Cannot create View for ViewModel [{viewType.FullName}]", e);
            }

            if (createdInstance is FrameworkElement viewInstance)
                return viewInstance;

            throw new InvalidOperationException($"Created instance for View type [{viewType.FullName}] " +
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

            view.DataContext = viewModel ?? throw new ArgumentNullException(nameof(viewModel));

            if (viewModel is IViewAware viewAware)
                viewAware.AttachView(view);
        }
    }
}
