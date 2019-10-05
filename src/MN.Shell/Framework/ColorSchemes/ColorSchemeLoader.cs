﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace MN.Shell.Framework.ColorSchemes
{
    public class ColorSchemeLoader : IColorSchemeLoader
    {
        public IEnumerable<ColorScheme> AvailableBaseColors { get; }

        public IEnumerable<ColorScheme> AvailableAccentColors { get; }

        public ColorScheme CurrentBaseColors { get; private set; }

        public ColorScheme CurrentAccentColors { get; private set; }

        public ColorSchemeLoader()
        {
            AvailableBaseColors = new List<ColorScheme>()
            {
                new ColorScheme("Dark", new Uri("pack://application:,,,/MN.Shell;component/Themes/BaseColors/Dark.xaml")),
                new ColorScheme("Light", new Uri("pack://application:,,,/MN.Shell;component/Themes/BaseColors/Light.xaml")),
            };

            AvailableAccentColors = new List<ColorScheme>()
            {
                new ColorScheme("Blue", new Uri("pack://application:,,,/MN.Shell;component/Themes/AccentColors/Blue.xaml")),
                new ColorScheme("Orange", new Uri("pack://application:,,,/MN.Shell;component/Themes/AccentColors/Orange.xaml")),
            };

            LoadBaseColors(AvailableBaseColors.First());
            LoadAccentColors(AvailableAccentColors.First());
        }

        public void LoadBaseColors(ColorScheme baseColorsScheme)
        {
            RemoveResourceDictionaries("pack://application:,,,MN.Shell;component/Themes/BaseColors/");

            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
            {
                Source = baseColorsScheme.ResourceDictionary
            });

            CurrentBaseColors = baseColorsScheme;
        }

        public void LoadAccentColors(ColorScheme accentColorsScheme)
        {
            RemoveResourceDictionaries("pack://application:,,,MN.Shell;component/Themes/AccentColors/");

            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
            {
                Source = accentColorsScheme.ResourceDictionary
            });

            CurrentAccentColors = accentColorsScheme;
        }

        private void RemoveResourceDictionaries(string uriStartsWith)
        {
            var resourceDictionaries = new Queue<ResourceDictionary>();
            resourceDictionaries.Enqueue(Application.Current.Resources);

            while (resourceDictionaries.Count > 0)
            {
                var resourceDictionary = resourceDictionaries.Dequeue();

                if (resourceDictionary.MergedDictionaries != null)
                {
                    foreach (var mergedDictionary in resourceDictionary.MergedDictionaries)
                    {
                        if (mergedDictionary.Source != null)
                        {
                            if (mergedDictionary.Source.OriginalString.StartsWith(uriStartsWith))
                                resourceDictionary.Remove(mergedDictionary);
                        }
                        else
                        {
                            resourceDictionaries.Enqueue(mergedDictionary);
                        }
                    }
                }
            }
        }
    }
}
