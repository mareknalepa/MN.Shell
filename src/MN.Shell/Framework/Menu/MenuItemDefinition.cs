using MN.Shell.PluginContracts;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace MN.Shell.Framework.Menu
{
    public class MenuItemDefinition : IMenuItemBuilder
    {
        public MenuItemDefinition(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public string LocalizedName { get; set; } = string.Empty;

        public int Section { get; private set; }

        public int Order { get; private set; }

        public ICommand Command { get; private set; }

        public bool IsCheckbox { get; private set; }

        public List<MenuItemDefinition> SubItems { get; } = new List<MenuItemDefinition>();

        public IMenuItemBuilder SetPlacement(int section, int order)
        {
            Section = section;
            Order = order;

            return this;
        }

        public IMenuItemBuilder SetCommand(ICommand command)
        {
            if (SubItems.Count != 0 || IsCheckbox)
                throw new InvalidOperationException("Cannot reconfigure menu item for different purpose");

            Command = command;

            return this;
        }

        public IMenuItemBuilder SetCheckbox(bool? isCheckedByDefault, bool isThreeState = false)
        {
            if (SubItems.Count != 0 || Command != null)
                throw new InvalidOperationException("Cannot reconfigure menu item for different purpose");

            IsCheckbox = true;

            return this;
        }
    }
}
