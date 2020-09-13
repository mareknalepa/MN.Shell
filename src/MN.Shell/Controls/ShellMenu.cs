using MN.Shell.Framework.Menu;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MN.Shell.Controls
{
    public class ShellMenu : Menu
    {
        static ShellMenu()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ShellMenu),
                new FrameworkPropertyMetadata(typeof(ShellMenu)));

            ItemsPanelProperty.OverrideMetadata(typeof(ShellMenu),
                new FrameworkPropertyMetadata(GetDefaultItemsPanelTemplate()));
        }

        private static ItemsPanelTemplate GetDefaultItemsPanelTemplate()
        {
            ItemsPanelTemplate template = new ItemsPanelTemplate(
                new FrameworkElementFactory(typeof(ShellMenuItemsPanel)));
            template.Seal();
            return template;
        }

        public ShellMenu()
        {
            BindingOperations.SetBinding(this, ItemsSourceProperty, new Binding()
            {
                Path = new PropertyPath("MenuItems"),
                RelativeSource = new RelativeSource(RelativeSourceMode.Self),
                Converter = new ShellMenuItemsConverter(),
            });
        }

        #region "Dependency Properties"

        public IEnumerable<MenuItemViewModel> MenuItems
        {
            get { return (IEnumerable<MenuItemViewModel>)GetValue(MenuItemsProperty); }
            set { SetValue(MenuItemsProperty, value); }
        }

        public static readonly DependencyProperty MenuItemsProperty =
            DependencyProperty.Register(nameof(MenuItems), typeof(IEnumerable<MenuItemViewModel>), typeof(ShellMenu),
                new FrameworkPropertyMetadata(Enumerable.Empty<MenuItemViewModel>()));

        #endregion
    }

    internal class ShellMenuItemsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IEnumerable<MenuItemViewModel> menuItems)
            {
                var ellipsisMenuItem = new MenuItemViewModel()
                {
                    Name = ". . .",
                    IsEllipsis = true,
                };

                foreach (var subItem in menuItems)
                    ellipsisMenuItem.SubItems.Add(subItem);

                return new CompositeCollection()
                {
                    new CollectionContainer()
                    {
                        Collection = menuItems,
                    },
                    ellipsisMenuItem,
                };
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => value;
    }
}
