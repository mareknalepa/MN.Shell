using System.Collections.Generic;

namespace MN.Shell.Framework.ColorSchemes
{
    public interface IColorSchemeLoader
    {
        IEnumerable<ColorScheme> AvailableBaseColors { get; }

        IEnumerable<ColorScheme> AvailableAccentColors { get; }

        ColorScheme CurrentBaseColors { get; }

        ColorScheme CurrentAccentColors { get; }

        void LoadBaseColors(ColorScheme baseColorsScheme);

        void LoadAccentColors(ColorScheme accentColorsScheme);
    }
}
