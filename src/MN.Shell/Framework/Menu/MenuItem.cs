using System;
using System.Windows.Input;

namespace MN.Shell.Framework.Menu
{
    public class MenuItem
    {
        public string Name { get; set; }

        public string[] Path { get; set; }

        public int GroupId { get; set; }

        public int GroupOrder { get; set; }

        public ICommand Command { get; set; }

        public Uri Icon { get; set; }
    }
}
