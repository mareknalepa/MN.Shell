using System;

namespace MN.Shell.Framework.ColorSchemes
{
    public class ColorScheme
    {
        public string Name { get; }

        public Uri ResourceDictionary { get; }

        public ColorScheme(string name, Uri resourceDictionary)
        {
            Name = name;
            ResourceDictionary = resourceDictionary;
        }
    }
}
