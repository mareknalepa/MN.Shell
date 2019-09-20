using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MN.Shell.Controls
{
    public class XamlIcon : Image
    {
        static XamlIcon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(XamlIcon),
                new FrameworkPropertyMetadata(typeof(XamlIcon)));
        }

        #region "Dependency Properties"

        public Brush PrimaryBrush
        {
            get { return (Brush)GetValue(PrimaryBrushProperty); }
            set { SetValue(PrimaryBrushProperty, value); }
        }

        public static readonly DependencyProperty PrimaryBrushProperty =
            DependencyProperty.Register(nameof(PrimaryBrush), typeof(Brush), typeof(XamlIcon),
                new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender));


        public Brush SecondaryBrush
        {
            get { return (Brush)GetValue(SecondaryBrushProperty); }
            set { SetValue(SecondaryBrushProperty, value); }
        }

        public static readonly DependencyProperty SecondaryBrushProperty =
            DependencyProperty.Register(nameof(SecondaryBrush), typeof(Brush), typeof(XamlIcon),
                new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender));

        #endregion
    }
}
