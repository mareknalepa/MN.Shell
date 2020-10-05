using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace MN.Shell.PluginContracts
{
    public interface IMenuItem
    {
        string Name { get; }

        IEnumerable<string> Path { get; }

        int GroupId { get; }

        int GroupOrder { get; }

        Uri Icon { get; }

        bool IsCheckable { get; }

        Action<bool> OnIsCheckedChanged { get; }

        ICommand Command { get; }
    }
}
