using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace MN.Shell.MVVM
{
    public static class Binder
    {
        /// <summary>
        /// Helper property to check if application is running in Design Time
        /// </summary>
        public static bool IsDesignTime => (bool)DependencyPropertyDescriptor.
            FromProperty(DesignerProperties.IsInDesignModeProperty, typeof(FrameworkElement)).Metadata.DefaultValue;

        /// <summary>
        /// ViewManager dependency, should be externally set prior to using ViewModel attached property
        /// (preferably by bootstrapper)
        /// </summary>
        public static IViewManager ViewManager { get; set; }

        private static object ViewModel { get; }

        public static object GetViewModel(DependencyObject obj) => obj.GetValue(ViewModelProperty);

        public static void SetViewModel(DependencyObject obj, object value) => obj.SetValue(ViewModelProperty, value);

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.RegisterAttached(nameof(ViewModel), typeof(object), typeof(Binder),
                new PropertyMetadata(null, OnViewModelChanged));

        private static void OnViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (ViewManager != null)
            {
                if (e.NewValue != null)
                    SetContentView(d, ViewManager.GetViewFor(e.NewValue));
                else
                    SetContentView(d, null);
            }
            else if (IsDesignTime)
                ShowBindingInfo(d);
            else
                throw new InvalidOperationException("Binder doesn't have ViewManager instance set by the bootstrapper");
        }

        private static void ShowBindingInfo(DependencyObject d)
        {
            string bindingInfo;
            var expr = BindingOperations.GetBindingExpression(d, ViewModelProperty);

            if (expr == null)
                bindingInfo = $"{nameof(Binder)}.{nameof(ViewModel)} binding not set correctly";
            else if (expr.ResolvedSourcePropertyName != null)
                bindingInfo = $"View for {expr.DataItem.GetType()}.{expr.ResolvedSourcePropertyName}";
            else
                bindingInfo = $"View for child ViewModel on {expr.DataItem.GetType()}";

            SetContentView(d, new TextBlock() { Text = bindingInfo });
        }

        /// <summary>
        /// Helper method to set view as a content on a given element
        /// </summary>
        /// <param name="element">Element to set content on</param>
        /// <param name="view">View to set as a content</param>
        public static void SetContentView(DependencyObject element, FrameworkElement view)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            var type = element.GetType();
            var contentAttribute = type.GetCustomAttribute<ContentPropertyAttribute>();
            var contentProperty = type.GetProperty(contentAttribute?.Name ?? "Content");
            if (contentProperty != null)
                contentProperty.SetValue(element, view);
            else
                throw new InvalidOperationException($"Cannot set view as content of {type} instance");
        }
    }
}
