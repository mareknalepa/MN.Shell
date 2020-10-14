using System;

namespace MN.Shell.Framework.ColorSchemes
{
    public class ColorScheme
    {
        public string Name { get; }

        public string LocalizedName { get; }

        public Uri ResourceDictionary { get; }

        public ColorScheme(string name, string localizedName, Uri resourceDictionary)
        {
            Name = name;
            LocalizedName = localizedName;
            ResourceDictionary = resourceDictionary;
        }
    }
}
