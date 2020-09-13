using System.Windows;
using System.Windows.Markup;

[assembly: XmlnsPrefix("https://github.com/mareknalepa/MN.Shell", "mnshell")]
[assembly: XmlnsDefinition("https://github.com/mareknalepa/MN.Shell", "MN.Shell.Behaviors")]
[assembly: XmlnsDefinition("https://github.com/mareknalepa/MN.Shell", "MN.Shell.Controls")]
[assembly: XmlnsDefinition("https://github.com/mareknalepa/MN.Shell", "MN.Shell.Core")]
[assembly: XmlnsDefinition("https://github.com/mareknalepa/MN.Shell", "MN.Shell.Framework")]

[assembly: ThemeInfo(
    ResourceDictionaryLocation.None, //where theme specific resource dictionaries are located
                                     //(used if a resource is not found in the page,
                                     // or application resource dictionaries)
    ResourceDictionaryLocation.SourceAssembly //where the generic resource dictionary is located
                                              //(used if a resource is not found in the page,
                                              // app, or any theme specific resource dictionaries)
)]
